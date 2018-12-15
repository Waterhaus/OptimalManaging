using System;

namespace OptimalManaging
{
    class DifferentialEquation 
    {
        
        //du/dt = d2u/d2s + f(t,s); du/dt = 0; 
        public static Vector SolveSliceTask1(Vector u_old, Vector f, Vector p, double nu, double h, double tau, double aa, int iter_t)
        {
            int n = u_old.Length;
            Vector u_mid = new Vector(n);
            Vector u_up = new Vector(n);
            Vector u_down = new Vector(n);
            Vector F = new Vector(n);
            double left = -aa * tau / (h * h);
            double right = -aa * tau / (h * h);
            double middle = 1d + 2d * aa * tau / (h * h);

            for (int i = 1; i < n - 1; i++)
            {
                F[i] = tau * f[i] + u_old[i];
                u_mid[i] = middle;
                u_up[i] = left;
                u_down[i] = right;
            }
            u_down[0] = 0;
            u_mid[0] = 1d + (h*h)/(aa*2d*tau); //y[0]
            u_up[0] = -1d; //y[1]
            F[0] = (f[0] + u_old[0]/tau)*(h*h)/2d*aa ;

            u_down[n - 1] = -1d; //y[N - 1]
            u_mid[n - 1] = 1d + (h * h) / (aa * 2d * tau) + h*nu; //y[N]
            u_up[n-1] = 0;
            double gamma_t = nu * p[iter_t];
            F[n-1] = ( f[n-1] + u_old[n-1]/tau  )*(h*h)/(2d*aa) +  gamma_t*h;

            Vector u = MyMath.TridiagonalMatrixAlgorithm(u_down, u_mid, u_up, F);
            return u;

        }

        // du/dt = -aa*d2u/d2s 
        public static Vector SolveSliceTask2(Vector u_old, double nu, double h, double tau, double aa)
        {
            int n = u_old.Length;
            Vector u_mid = new Vector(n);
            Vector u_up = new Vector(n);
            Vector u_down = new Vector(n);
            
            double left = -aa * tau / (h * h);
            double right = -aa * tau / (h * h);
            double middle = 1d + 2d * aa * tau / (h * h);

            for (int i = 1; i < n - 1; i++)
            {
                
                u_mid[i] = middle;
                u_up[i] = left;
                u_down[i] = right;
            }
            u_down[0] = 0;
            u_mid[0] = -(1d - h*h/(2d*aa*tau)); //y[0]
            u_up[0] = 1d;// y[1]
            u_old[0] = u_old[0] * (h * h / (2d * aa * tau));

            u_down[n - 1] = 1d; //y[N-1]
            u_mid[n - 1] = -1d - nu*h + (h*h)/(2d*aa*tau); //y[N]
            u_up[n-1] = 0;
            u_old[n - 1] = u_old[n - 1] * (h * h) / (2d * aa * tau);

            Vector u = MyMath.TridiagonalMatrixAlgorithm(u_down, u_mid, u_up, u_old);
            return u;
        }


        public static Vector GetSolutionTask1(Vector p, Matrix f, FunctionLib.Function phi,
                                        double nu, double L, double TIME, 
                                        double aa, int N, int M)
        {
            double h = MyMath.GetStep(N, 0d, L);
            double tau = MyMath.GetStep(M, 0d, TIME);
            Vector u0 = new Vector(N);
            Vector fu = new Vector(N);

            for (int i = 0; i < N; i++)
            {
                u0[i] = phi(i * h);
            }

            for (int iter_t = 0; iter_t < M; iter_t++)
            {
                for (int i = 0; i < N; i++)
                {
                    fu[i] = f[iter_t,i];
                }
                u0 = SolveSliceTask1(u0, fu, p, nu, h, tau, aa, iter_t);
            }

            return u0;

        }


        public static Matrix GetSolutionTask2(FunctionLib.Function y, Vector x_T,
                                        double nu, double L, double TIME,
                                        double aa, int N, int M)
        {
            double h = MyMath.GetStep(N, 0d, L);
            double tau = MyMath.GetStep(M, 0d, TIME);
            Vector u0 = new Vector(N);
            Vector fu = new Vector(N);
            Matrix KSI = new Matrix(M, N);
            for (int i = 0; i < N; i++)
            {
                u0[i] = 2d*( x_T[i] - y(i*h) );
                KSI[M - 1, i] = u0[i];
            }

            for (int iter_t = 1; iter_t < M; iter_t++)
            {
                for (int j = 0; j < N; j++)
                {
                    KSI[M - 1 - iter_t, j] = u0[j];
                }

                u0 = SolveSliceTask2(u0,nu,h,tau,aa);

                
            }

            return KSI;

        }

        public static Vector GrdientProjection(Vector p_old, Vector ksi_l, double alpha, double aa)
        {
            double p_max = p_old.MaxValue;
            double p_min = p_old.MinValue;
            Vector p = new Vector(p_old.Length);

            double temp = 0;
            for (int i = 0; i < p.Length; i++)
            {
                temp = p_old[i] - alpha * aa * ksi_l[i];
                if (temp > p_max)
                {
                    p[i] = p_max;
                    continue;
                }
                if (temp < p_min)
                {
                    p[i] = p_min;
                    continue;
                }
                p[i] = temp;

            }

            return p;
        }

        public static Vector GrdientProjection(Vector p_old, Vector ksi_l, double alpha, double aa, double pmin, double pmax)
        {
            double p_max = pmax;
            double p_min = pmin;
            Vector p = new Vector(p_old.Length);

            double temp = 0;
            for (int i = 0; i < p.Length; i++)
            {
                temp = p_old[i] - alpha * aa * ksi_l[i];
                if (temp > p_max)
                {
                    p[i] = p_max;
                    continue;
                }
                if (temp < p_min)
                {
                    p[i] = p_min;
                    continue;
                }
                p[i] = temp;

            }

            return p;
        }

        public static Matrix GrdientProjectionByF(Matrix f_old, Matrix KSI, double alpha, double R, double h, double tau)
        {
            int N = f_old.Length.n;
            int M = f_old.Length.m;

            double I = 0;
            Matrix T = new Matrix(N, M);
            Matrix f_new = new Matrix(N, M);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    T[i, j] = Math.Pow(Math.Abs(f_old[i, j] - alpha * KSI[i, j]), 2);
                }
            }
            I = MyMath.IntegrateSurface(T, tau, h);

            if (I <= R * R)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < M; j++)
                    {
                        f_new[i, j] = f_old[i, j] - alpha * KSI[i, j];
                    }
                }
            }
            else
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < M; j++)
                    {
                        f_new[i, j] = R*(f_old[i, j] - alpha * KSI[i, j])/Math.Sqrt(I);
                    }
                }
            }


            return f_new;
        }


        public static double dJ_f(Matrix KSI, Matrix f, double h, double tau)
        {
            int N = f.Length.n;
            int M = f.Length.m;

            double I = 0;
            Matrix KSI2 = new Matrix(N, M);
            
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    KSI2[i, j] = KSI[i, j]*f[i,j];
                }
            }
            I = MyMath.IntegrateSurface(KSI2, tau, h);

            return Math.Abs(I);
        }



        public static Matrix ConditionalGradientByF(Matrix f_old, Matrix KSI, double alpha, double R, double h, double tau)
        {
            int N = f_old.Length.n;
            int M = f_old.Length.m;

            double NORM = 0;
            Matrix KSI2 = new Matrix(N, M);
            Matrix f_new = new Matrix(N, M);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    KSI2[i, j] = Math.Pow(Math.Abs(KSI[i, j]), 2);
                }
            }
            NORM = Math.Sqrt(MyMath.IntegrateSurface(KSI2, tau, h));

            double f_prime = 0; 
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    //R = 10;
                    f_prime = -R * KSI[i, j] / NORM;
                    
                    f_new[i, j] = (1d - alpha)*f_old[i, j] + alpha * f_prime;
                }
            }

            return f_new;
        }




    }
}
