using System;
using System.Windows.Forms;
namespace OptimalManaging
{

    public enum PickAlpha { Lipshiz, Divide , Lipshiz_CG, SUM, Projection};
    public class OptimalManaging
    {
        public PickAlpha pickAlpha = PickAlpha.Lipshiz;
        private double L;
        private double T;
        private double aa;
        private double nu;

        
        public int TIME_SIZE;
        public int GRID_SIZE;

        public double EPS1, EPS2;

        public double LIP;

        FunctionLib.Function y;
        FunctionLib.Function p;
        FunctionLib.Function phi;
        FunctionLib.Function2d f;

        int ITERATION;
        public Vector manage_p;
        public Matrix manage_f;
        public Vector calc_u;
        public Vector calc_u_old;
        double P_MIN, P_MAX;
        double R;
        public double alpha_old;



        private double Get_C0()
        {
            double a0 = (aa * aa * nu * nu + 2d * L) / (aa * nu);
            double a1 = (2d * L * L) / (aa);

            return Math.Max(a0, a1); 
        }

        private double Get_C1()
        {
            double a0 = (aa * nu) / EPS1;
            double a1 = 1d / (aa * EPS1);

            return Math.Max(a0, a1);
        }


        public OptimalManaging(double LengthVal, double TimeVal,
                                double sq_a, double Nu, int TIME_M, int LEN_N,
                                double eps1, double eps2,
                                FunctionLib.Function distrib_y,
                                FunctionLib.Function env_temperature_p,
                                FunctionLib.Function temperature0,
                                FunctionLib.Function2d temperature_sourses )
        {

            ITERATION = 0;
                L = LengthVal;
                T = TimeVal;
                aa = sq_a;
                nu = Nu;


                TIME_SIZE = TIME_M;
                GRID_SIZE = LEN_N;

            EPS1 = eps1;
            EPS2 = eps2;
            P_MAX = 1000;
            P_MIN = -1000;
            double C0 = Get_C0();
            double C1 = Get_C1();

            LIP =  Math.Sqrt(2d * C0 * C1);
            R = 10;
            manage_p = new Vector(TIME_SIZE);

            y = distrib_y;
            p = env_temperature_p;
            phi = temperature0;
            f = temperature_sourses;

            double tau = MyMath.GetStep(TIME_SIZE, 0d, T);
            double h = MyMath.GetStep(GRID_SIZE, 0d, L);
            for (int i = 0; i < TIME_SIZE; i++)
            {
                manage_p[i] = p(tau * i);
            }
            manage_f = new Matrix(TIME_SIZE, GRID_SIZE);
            for (int i = 0; i < TIME_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    manage_f[i, j] = f(h * j, tau * i);
                }
            }

            alpha_old = 5d;

        }
        public OptimalManaging(double LengthVal, double TimeVal,
                                double sq_a, double Nu, int TIME_M, int LEN_N,
                                double eps1, double eps2,
                                FunctionLib.Function distrib_y,
                                FunctionLib.Function env_temperature_p,
                                FunctionLib.Function temperature0,
                                FunctionLib.Function2d temperature_sourses,
                                double pmin, double pmax, double INT_R)
        {

            ITERATION = 0;
            L = LengthVal;
            T = TimeVal;
            aa = sq_a;
            nu = Nu;
            R = INT_R;

            TIME_SIZE = TIME_M;
            GRID_SIZE = LEN_N;

            EPS1 = eps1;
            EPS2 = eps2;
            P_MAX = pmax;
            P_MIN = pmin;
            double C0 = Get_C0();
            double C1 = Get_C1();

            LIP = Math.Sqrt(2d * C0 * C1);

            manage_p = new Vector(TIME_SIZE);

            y = distrib_y;
            p = env_temperature_p;
            phi = temperature0;
            f = temperature_sourses;

            alpha_old = 5d;

            double tau = MyMath.GetStep(TIME_SIZE, 0d, T);
            double h = MyMath.GetStep(GRID_SIZE, 0d, L);

            for (int i = 0; i < TIME_SIZE; i++)
            {
                manage_p[i] = p(tau * i);
            }

            manage_f = new Matrix(TIME_SIZE, GRID_SIZE);
            for (int i = 0; i < TIME_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    manage_f[i, j] = f(h * j, tau * i);
                }
            }

            //Vector temp = MyMath.GetVectorFunction(GRID_SIZE, 0d, LengthVal, y);
            //R = MyMath.TrapezoidMethod(temp, h);
           
        }
        double min(double a, double b)
        {
            if (a < b) return a; else return b;
        }
        public double ChooseAlpha_LipMethod()
        {
            double e1 = EPS1;
            double e2 = 2d / (LIP + 2d * EPS2);
            Random random = new Random();
            double t = 0.5d;//random.NextDouble();

            return t * e1 + (1d - t) * e2;
        }
        public double ChooseAlpha_Sum()
        {
            
            return 2d/(ITERATION + 1);
        }

        double Calculate_L2NORM(Matrix f, double h, double tau)
        {
            int N = f.Length.n;
            int M = f.Length.m;

            double NORM = 0;
           
            Matrix f2 = new Matrix(N, M);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    f2[i, j] = Math.Pow(Math.Abs(f[i, j]), 2);
                }
            }
            NORM = Math.Sqrt(MyMath.IntegrateSurface(f2, tau, h));
            return NORM;

        }

        public double ChooseAlpha_Projection(Matrix KSI, Matrix f, double alpha, double R, double tau, double h)
        {
            Vector u_temp = new Vector();
            double EPS = 0.001d;
            double NORM = 0;
            do {
                Matrix f_new = DifferentialEquation.GrdientProjectionByF(f, KSI, alpha, R, h, tau);
                f_new = Pu(f_new, R, h, tau);

                u_temp = DifferentialEquation.GetSolutionTask1(manage_p, f_new, phi, nu, L, T, aa, GRID_SIZE, TIME_SIZE);


                NORM = Calculate_L2NORM(f - f_new, h, tau);
                alpha = alpha / 2.0;


            } while (Functional_J(calc_u) - Functional_J(u_temp) < EPS*NORM*NORM );

            return alpha * 2.0;
        }
        public double ChooseAplpha_LipMethodForConditionalGradient(Matrix KSI, Matrix f,double alpha, double tau, double h)
        {
            double rho = ChooseAlpha_LipMethod();
            int N = f.Length.n;
            int M = f.Length.m;

            double NORM = 0;
            Matrix KSI2 = new Matrix(N, M);
            Matrix f2 = new Matrix(N, M);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    KSI2[i, j] = Math.Pow(Math.Abs(KSI[i, j]), 2);
                }
            }
            NORM = Math.Sqrt(MyMath.IntegrateSurface(KSI2, tau, h));

            double f_prime = 0,  temp = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    //R = 10;
                    f_prime = -R * KSI[i, j]/NORM;
                    temp = f_prime - f[i, j];
                    f2[i, j] = temp * temp;
                }
            }

            NORM = MyMath.IntegrateSurface(f2, tau, h);
            double dJ = DifferentialEquation.dJ_f(KSI, f, h, tau);
            //MessageBox.Show("dJ = " + dJ + " NORM = " + NORM);
            return min(1d, rho*dJ/(NORM));


        }

        public double ChooseAlpha_DivideMethod(double alpha)
        {
            double e_new = Functional_J(calc_u);
            double e_old = Functional_J(calc_u_old);

            

            if (e_new > e_old)
                return alpha / 2d;
            else return alpha;

            //Console.WriteLine( "alpha = " + alpha );  
        }

        public double Functional_J(Vector calc_u)
        {
            double h = MyMath.GetStep(GRID_SIZE, 0d, L);
            Vector calc_y = new Vector(GRID_SIZE);
            Vector err = new Vector(GRID_SIZE);

            for (int i = 0; i < GRID_SIZE; i++)
            {
                err[i] = (calc_u[i] - y(h * i)) * (calc_u[i] - y(h * i));
            }


            return MyMath.RiemannSum(err.ToArray, h);

        }



        public Matrix Pu(Matrix f,double R, double h, double tau)
        {
            int N = f.Length.n;
            int M = f.Length.m;

            double NORM = 0;
            Matrix f2 = new Matrix(N, M);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    f2[i, j] = Math.Pow(Math.Abs(f[i, j]), 2);
                }
            }
            NORM = Math.Sqrt(MyMath.IntegrateSurface(f2, tau, h));


            if (NORM < R)
            {
                return f;
            }else
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < M; j++)
                    {
                        f2[i, j] = R*f[i, j] / NORM;
                    }
                }
                return f2;
            }

        }





        public Vector CalculateIteration()
        {
            calc_u = DifferentialEquation.GetSolutionTask1(manage_p, manage_f, phi, nu, L, T, aa, GRID_SIZE, TIME_SIZE);

            Matrix KSI = DifferentialEquation.GetSolutionTask2(y, calc_u, nu, L, T, aa, GRID_SIZE, TIME_SIZE);

            Vector ksi_l = new Vector(TIME_SIZE);
            for (int i = 0; i < TIME_SIZE; i++)
            {
                ksi_l[i] = KSI[i, GRID_SIZE - 1];
            }
            double alpha = alpha_old;

            double tau = MyMath.GetStep(TIME_SIZE, 0d, T);
            double h = MyMath.GetStep(GRID_SIZE, 0d, L);

            switch (pickAlpha)
            {
                case PickAlpha.Lipshiz:
                    alpha = ChooseAlpha_LipMethod();
                    break;
                case PickAlpha.Divide:
                    if (ITERATION > 0)
                        alpha = ChooseAlpha_DivideMethod(alpha_old);
                    break;
                case PickAlpha.Lipshiz_CG:
                    if (ITERATION > 0)
                        alpha = ChooseAplpha_LipMethodForConditionalGradient(KSI, manage_f,alpha, tau, h);
                    //alpha = 1d / (ITERATION + 1);
                    break;
                case PickAlpha.SUM:
                    alpha = ChooseAlpha_Sum();
                    break;
                case PickAlpha.Projection:
                    alpha = ChooseAlpha_Projection(KSI, manage_f, alpha,R,tau,h);
                    break;
                default:
                    break;
            }

            
           

            manage_p = DifferentialEquation.GrdientProjection(manage_p, ksi_l,alpha, aa,P_MIN,P_MAX);
            //manage_f = DifferentialEquation.GrdientProjectionByF(manage_f, KSI, alpha, R, h, tau);
            ITERATION++;
            calc_u_old = calc_u;
            alpha_old = alpha;
            return manage_p;

        }



    }
}
