using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalManaging
{
    class MyMath
    {
        //Возвращает шаг сетки на отрезке [a b] с соответсвующим числом точек
        public static double GetStep(int GridSize, double a_border, double b_border)
        {

            if (a_border.Equals(b_border))
                throw new System.ArgumentException("Ошибка: границы отрезка равны. a=b");

            if (a_border > b_border)
            {
                double temp = a_border;
                a_border = b_border;
                b_border = temp;
            }

            return (b_border - a_border) / (GridSize - 1);
        }



        public static double SumArray(double[] arr)
        {
            double S = 0;

            if (arr == null)
            {
                throw new System.ArgumentNullException("Parameter cannot be null", "arr");
            }

            for (int i = 0; i < arr.Length; i++)
            {
                S = S + arr[i];
            }
            return S;
        }


        public static double RiemannSum(double[] y, double h)
        {
            double S = MyMath.SumArray(y);

            return S * h;
        }

        


        public static double TrapezoidMethod(Vector f, double h)
        {
            double S = 0;

            for (int i = 1; i < f.Length - 1; i++)
            {
                S += f[i];
            }
            S += (f[0] + f.Last) / 2d;
            S *= h;

            return S;
        }


        public static double IntegrateSurface(Matrix F, double h_i, double h_j)
        {
            double S = 0;

            Vector I = new Vector(F.Length.n);
            int n = F.Length.m - 1;
            for (int i = 0; i < F.Length.n; i++)
            {
                for (int j = 1; j < F.Length.m - 1; j++)
                {
                    I[i] += F[i, j];
                }
                I[i] += (F[i, 0] + F[i, n]) / 2d;
                I[i] *= h_j;
            }

            S = TrapezoidMethod(I, h_i);
            return S;
        }

        public static Vector TridiagonalMatrixAlgorithm(Vector d_under, Vector d_main, Vector d_upper, Vector f)
        {
            int n = d_main.Length;
            Vector x = new Vector(n);
            double m;
            d_under[0] = 0;
            // d_upper.Last = 0;
            Vector copy = new Vector(d_main.ToArray);
            for (int i = 1; i < n; i++)
            {
                m = d_under[i] / d_main[i - 1];
                d_main[i] = d_main[i] - m * d_upper[i - 1];
                f[i] = f[i] - m * f[i - 1];
            }

            x[n - 1] = f[n - 1] / d_main[n - 1];

            for (int i = n - 2; i >= 0; i--)
                x[i] = (f[i] - d_upper[i] * x[i + 1]) / d_main[i];


            return x;
        }

        public static Vector GetVectorFunction(int GridSize, double a_border, double b_border, FunctionLib.Function F)
        {
            Vector f = new Vector(GridSize);
            double h = GetStep(GridSize, a_border, b_border);

            for (int i = 0; i < GridSize; i++)
            {
                f[i] = F(a_border + i * h);
            }
            return f;
        }


        public static double[] CreateUniformGrid(int GridSize, double a, double b)
        {
            double[] grid = new double[GridSize];
            double h = GetStep(GridSize, a, b);

            for (int i = 0; i < GridSize; i++)
                grid[i] = a + i * h;

            return grid;
        }

    }
}
