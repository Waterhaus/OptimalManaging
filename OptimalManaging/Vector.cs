using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalManaging
{
    public class Vector
    {
        double[] v;
        private int length;
        public int Length
        {
            get { if (v == null) return 0; else return v.Length; }
        }
        public double Last
        {
            get { return v[length - 1]; }
            set { v[length - 1] = value; }
        }
        //вторая норма
        public double Norm
        {
            get
            {
                return Norm2();
            }
        }

        public double InfNorm
        {
            get
            {
                return NormI();
            }
        }

        public double[] ToArray
        {
            get
            {
                return v;
            }

        }

        public double MaxValue
        {
            get
            {
                double max = v[0];
                for (int i = 0; i < length; i++)
                {
                    double temp = v[i];
                    if (temp > max) max = temp;
                }

                return max;
            }
        }

        public double MinValue
        {
            get
            {
                double min = v[0];
                for (int i = 0; i < length; i++)
                {
                    double temp = v[i];
                    if (temp < min) min = temp;
                }

                return min;
            }
        }

        public Vector()
        {
            length = 0;
            v = null;
        }

        public Vector(int n)
        {
            length = n;
            v = new double[n];
            for (int i = 0; i < n; i++) v[i] = 0;
        }

        public Vector(double[] vec)
        {
            v = new double[vec.Length];
            length = v.Length;
            for (int i = 0; i < vec.Length; i++)
            {
                v[i] = vec[i];
            }
        }

        private double Norm2()
        {
            double S = 0;
            for (int i = 0; i < length; i++)
            {
                S += v[i] * v[i];
            }
            return Math.Sqrt(S);
        }

        //бесконечная норма
        private double NormI()
        {
            double max = Math.Abs(v[0]);


            for (int i = 0; i < Length; i++)
            {
                if (Math.Abs(v[i]) > max) max = Math.Abs(v[i]);
            }

            return max;
        }


        private double Minimum()
        {
            double min = v[0];


            for (int i = 0; i < Length; i++)
            {
                if (v[i] < min) min = v[i];
            }

            return min;
        }

        private double Maximum()
        {
            double max = v[0];


            for (int i = 0; i < Length; i++)
            {
                if (v[i] > max) max = v[i];
            }

            return max;
        }


        public double this[int i]
        {
            //Метод доступа для чтения
            get
            {
                return v[i];
            }
            //Метод доступа для установки
            set
            {
                v[i] = value;
            }
        }



        public static Vector operator +(Vector a, Vector b)
        {
            Vector c = new Vector(a.Length);

            for (int i = 0; i < a.Length; i++)
            {
                c[i] = a[i] + b[i];
            }
            return c;
        }

        public static Vector operator -(Vector a, Vector b)
        {
            Vector c = new Vector(a.Length);

            for (int i = 0; i < a.Length; i++)
            {
                c[i] = a[i] - b[i];
            }
            return c;
        }

        public static double operator *(Vector a, Vector b)
        {
            double S = 0;
            if (a.Length == b.Length)
                for (int i = 0; i < a.Length; i++)
                {
                    S += a[i] * b[i];
                }
            return S;
        }

        public static Vector operator *(double a, Vector b)
        {

            Vector c = new Vector(b.Length);

            for (int i = 0; i < b.Length; i++)
            {
                c[i] = a * b[i];
            }
            return c;
        }


        public static void copy(ref Vector x, Vector y)
        {
            x = new Vector(y.Length);
            if (x.Length == y.Length)
            {
                for (int i = 0; i < x.Length; i++)
                {
                    x[i] = y[i];
                }
            }
        }

        public static Vector GetEn(int i, int n)
        {
            Vector a = new Vector(n);
            a[i] = 1;
            return a;
        }

        public static Vector GetConstVector(double a, int n)
        {
            Vector vec = new Vector(n);

            for (int i = 0; i < n; i++)
                vec[i] = a;

            return vec;
        }

        public override string ToString()
        {
            string s = "< ";
            for (int i = 0; i < length; i++)
                s += v[i].ToString("0.000") + " ,";
            s += " >";
            return s;
        }

        public string ToString(string str)
        {
            string s = "< ";
            for (int i = 0; i < length; i++)
                s += v[i].ToString(str) + " ,";
            s += " >";
            return s;
        }
        public static Vector RandomVector(int n)
        {
            Vector v = new Vector(n);
            Random r = new Random();

            for (int i = 0; i < n; i++)
                v[i] = r.Next(0, 20)/10d;

            return v;
        }

        public static Vector CreateUniformGrid(int GridSize, double a, double b)
        {
            return new Vector(MyMath.CreateUniformGrid(GridSize, a, b));
        }

        public Vector GetPartVector(int start, int end)
        {
            int N = end - start + 1;
            Vector vec = new Vector(N);
            for (int i = 0  ; i < N && i < length; i++)
            {
                vec[i] = v[i + start];
            }

            return vec;
        }
    }

}
