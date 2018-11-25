using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalManaging
{

    public struct Pair
    {
        public int n;
        public int m;

    }
    public class Matrix
    {

        double[,] M;
        private int n, m;
        public Pair Length
        {
            get
            {
                Pair l = new Pair();
                l.n = n;
                l.m = m;
                return l;
            }
        }

        public double NORMF
        {
            get { return VecNorm(2d); }
        }

        public Matrix()
        {
            M = new double[1, 1];
            M[0, 0] = 0;
            n = 0; m = 0;
        }
        public Matrix(int _N, int _M)
        {
            M = new double[_N, _M];
            n = _N;
            m = _M;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    M[i, j] = 0;
                }

        }

        public Matrix(int N)
        {
            M = new double[N, N];
            n = m = N;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    M[i, j] = 0;
                }

        }

        public Matrix(double[,] Matr)
        {
            n = Matr.GetLength(0);
            m = Matr.GetLength(1);
            M = new double[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    M[i, j] = Matr[i, j];
                }
        }

        public Matrix(Vector w)
        {
            M = new double[w.Length, w.Length];
            n = m = w.Length;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    M[i, j] = w[i] * w[j];

        }

        public Matrix(Vector w, Vector u)
        {
            M = new double[w.Length, u.Length];
            n = w.Length;
            m = u.Length;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    M[i, j] = w[i] * u[j];

        }

        public double this[int i, int j]
        {
            //Метод доступа для чтения
            get
            {
                return M[i, j];
            }
            //Метод доступа для установки
            set
            {
                M[i, j] = value;
            }
        }

        public double VecNorm(double p)
        {
            double S = 0d;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    S += Math.Pow(M[i, j], p);
                }
            }
            return Math.Pow(S, 1d / p);
        }

        public bool EqualLength(Matrix B)
        {
            return (n == B.n) && (m == B.m);
        }

        public static Matrix operator +(Matrix A, Matrix B)
        {
            if (A.EqualLength(B))
            {
                Matrix C = new Matrix(A.Length.n, A.Length.m);

                for (int i = 0; i < A.Length.n; i++)
                    for (int j = 0; j < A.Length.m; j++)
                        C[i, j] = A[i, j] + B[i, j];

                return C;
            }
            return new Matrix();
        }

        public static Matrix operator -(Matrix A, Matrix B)
        {
            if (A.EqualLength(B))
            {
                Matrix C = new Matrix(A.Length.n, A.Length.m);

                for (int i = 0; i < A.Length.n; i++)
                    for (int j = 0; j < A.Length.m; j++)
                        C[i, j] = A[i, j] - B[i, j];

                return C;
            }
            return new Matrix();
        }


        public static Vector operator *(Matrix A, Vector x)
        {
            if (A.m == x.Length)
            {
                Vector b = new Vector(A.n);
                //A*x = b
                for (int i = 0; i < A.Length.n; i++)
                {
                    for (int j = 0; j < A.Length.m; j++)
                    {
                        b[i] += A[i, j] * x[j];
                    }
                }


                return b;
            }
            return new Vector();
        }

        public static Vector operator *(Vector x, Matrix A)
        {
            if (A.m == x.Length)
            {
                Vector b = new Vector(A.n);

                for (int j = 0; j < b.Length; j++)
                {
                    for (int i = 0; i < x.Length; i++)
                        b[j] += A[j, i] * x[i];
                }
                return b;
            }
            return new Vector();
        }

        public static Matrix operator *(double a, Matrix A)
        {


            for (int j = 0; j < A.n; j++)
            {
                for (int i = 0; i < A.m; i++)
                    A[j, i] = a * A[j, i];
            }
            return A;


        }

        public static Matrix operator *(Matrix A, Matrix B)
        {
            int l = A.n;
            int m = A.m;
            int n = B.m;
            if ((A.m == B.n))
            {
                Matrix C = new Matrix(l, n);

                for (int i = 0; i < l; i++)
                    for (int j = 0; j < n; j++)
                        for (int r = 0; r < m; r++)
                            C[i, j] += A[i, r] * B[r, j];

                return C;
            }
            return new Matrix();
        }



        public static Matrix Identity(int N)
        {
            Matrix E = new Matrix(N);

            for (int i = 0; i < N; i++)
                E[i, i] = 1;

            return E;
        }

        public override string ToString()
        {
            string S = " { " + Environment.NewLine + "";
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    S += M[i, j].ToString("0.000") + ", ";
                }
                S += ">, " + Environment.NewLine;
            }
            S += " };";
            return S;
        }
        public string ToString(string s)
        {
            string S = " { " + Environment.NewLine + "";
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    S += M[i, j].ToString(s) + ", ";
                }
                S += ">, " + Environment.NewLine;
            }
            S += " };";
            return S;
        }

        public Vector Column(int k)
        {
            Vector c = new Vector(m);
            for (int j = 0; j < m; j++)
                c[j] = M[k, j];
            return c;
        }

        public Vector Row(int k)
        {
            Vector c = new Vector(n);
            for (int i = 0; i < n; i++)
                c[i] = M[i, k];
            return c;
        }


        public Matrix HessenBerg()
        {
            if (n == m)
            {
                Matrix A = this;
                Matrix Q = Identity(n);
                Matrix V;
                for (int i = 0; i < n - 1; i++)
                {

                    Vector w = Haushold(Row(i), i + 1);
                    

                    double a = 2 / w.Norm;
                    V = Identity(n) - a * (new Matrix(w));



                    Q = Q * V;

                    //Ai = (Ai -  (new Matrix((2 / Math.Pow((w.Norma),2)) * w , (w*Ai)))   );
                }
                return Q * A * Q;

            }
            return new Matrix();
        }

        public static Matrix Hessenberg(Matrix A)
        {
            int n = A.Length.n;
            Matrix Q = Identity(n);
            Matrix H = Identity(n);
            Matrix R = new Matrix(n);

            for (int i = 0; i < n - 2; i++)
            {
                int ii = i + 1;

                //2.1
                double S = 0;
                for (int k = ii; k < n; k++) S += A[k, i] * A[k, i];
                //2.2
                if (S != 0)
                {
                    //2.3
                    double betta = -sign(A[ii, i]) * Math.Sqrt(S);
                    //  MessageBox.Show("betta = " + betta.ToString());
                    //2.4
                    double gamma = 1.0f / (betta * (betta - A[ii, i]));
                    // MessageBox.Show("gamma = " + gamma.ToString());
                    //2.5
                    A[ii, i] = A[ii, i] - betta;
                    //2.6
                    H = Identity(n);
                    //2.7
                    for (int M = ii; M < n; M++)
                        for (int j = ii; j < n; j++)
                        {
                            if (j != M) H[M, j] = -gamma * A[M, i] * A[j, i];
                            else H[M, j] = 1 - gamma * A[M, i] * A[M, i];
                            H[j, M] = H[M, j];
                        }
                    //2.8
                    Matrix T = new Matrix(n);

                    for (int M = ii; M < n; M++)
                        for (int j = i + 1; j < n; j++)
                        {
                            double sum = 0;
                            for (int k = ii; k < n; k++) sum += A[k, i] * A[k, j];

                            T[M, j] = A[M, j] - gamma * A[M, i] * sum;
                        }
                    //2.9
                    for (int M = ii; M < n; M++)
                        for (int j = i + 1; j < n; j++)
                        {
                            A[M, j] = T[M, j];
                        }

                    //2.10
                    A[ii, i] = betta;
                    //2.11
                    for (int M = ii + 1; M < n; M++)
                        A[M, i] = 0;
                    //2.12
                    for (int M = 0; M < n; M++)
                        for (int j = 0; j < n; j++)
                        {
                            double sum = 0;
                            for (int k = i; k < n; k++) sum += Q[M, k] * H[k, j];
                            T[M, j] = sum;
                        }
                    string str = "< " + Q[0, 1].ToString() + " , " + Q[1, 1].ToString() + " , " + Q[2, 1].ToString() + " > Итерация № " + i.ToString();
                    str += "  // 2.12";
                    //MessageBox.Show(str);
                    //2.13
                    for (int M = 0; M < n; M++)
                        for (int j = i; j < n; j++)
                            Q[M, j] = T[M, j];

                    str = "< " + Q[0, 1].ToString() + " , " + Q[1, 1].ToString() + " , " + Q[2, 1].ToString() + " > Итерация № " + i.ToString();
                    str += "  // 2.13";
                    // MessageBox.Show(str);
                    //2.14
                    for (int M = 0; M < n; M++)
                        for (int j = 0; j < n; j++)
                            R[M, j] = A[M, j];

                    str = "< " + Q[0, 1].ToString() + " , " + Q[1, 1].ToString() + " , " + Q[2, 1].ToString() + " > Итерация № " + i.ToString();
                    str += "  // 2.14";
                    // MessageBox.Show(str);
                }

            }

            return R * Q;
        }

        public static Matrix Givens(Matrix B)
        {
            double EPS = 0.0000000000001f;
            int n = B.Length.n;
            double c = 0;
            double s = 0;
            double t = 0;
            Matrix Q = Matrix.Identity(n);
            Matrix G = Matrix.Identity(n);


            for (int i = 0; i < n - 1; i++)
            {
                if (abs(B[i, i]) < EPS)
                {
                    c = 1;
                    s = 0;
                }
                else
                {

                    t = B[i + 1, i] / B[i, i];
                    c = 1 / Math.Sqrt(1 + t * t);
                    s = t * c;
                    //  MessageBox.Show("c == "+ c.ToString()+" s == "+s.ToString());
                }




                for (int j = i; j < n; j++)
                {
                    double X, Y;
                    X = B[i, j];
                    Y = B[i + 1, j];

                    B[i, j] = c * X + s * Y;

                    B[i + 1, j] = c * Y - s * X;

                }

                G[i, i] = c; G[i, i + 1] = -s;
                G[i + 1, i] = s; G[i + 1, i + 1] = c;

                if (i == 0)
                {
                    Q[i, i] = c; Q[i, i + 1] = -s;
                    Q[i + 1, i] = s; Q[i + 1, i + 1] = c;

                }
                else
                {
                    Q = Q * G;

                }
                G[i, i] = 1; G[i, i + 1] = 0;
                G[i + 1, i] = 0; G[i + 1, i + 1] = 1;
            }
            return B * Q;
        }

        public static Matrix GivensFast(Matrix B)
        {
            double EPS = 0.0000000000001f;
            int n = B.Length.n;
            double c = 0;
            double s = 0;
            double t = 0;
            Matrix Q = Matrix.Identity(n);
            Matrix G = Matrix.Identity(n);

            Vector[] Tr = new Vector[n - 1];

            for (int i = 0; i < n - 1; i++)
            {
                if (abs(B[i, i]) < EPS)
                {
                    c = 1;
                    s = 0;
                }
                else
                {

                    t = B[i + 1, i] / B[i, i];
                    c = 1 / Math.Sqrt(1 + t * t);
                    s = t * c;
                    //  MessageBox.Show("c == "+ c.ToString()+" s == "+s.ToString());
                }

                Tr[i] = new Vector(2);
                Tr[i][0] = c;
                Tr[i][1] = s;


                for (int j = i; j < n; j++)
                {
                    double X, Y;
                    X = B[i, j];
                    Y = B[i + 1, j];

                    B[i, j] = c * X + s * Y;

                    B[i + 1, j] = c * Y - s * X;

                }


            }

            for (int j = 0; j < n - 1; j++)
                for (int i = 0; i < n; i++)
                {
                    double X, Y;
                    X = B[i, j];
                    Y = B[i, j + 1];

                    B[i, j] = Tr[j][0] * X + Tr[j][1] * Y;

                    B[i, j + 1] = -Tr[j][1] * X + Tr[j][0] * Y;

                }

            return B;
        }

        private static void TransMult(ref Matrix A, Matrix B)
        {
            int l = A.n;
            int m = A.m;
            int n = B.m;
            if ((A.m == B.n))
            {
                Matrix C = new Matrix(l, n);

                for (int i = 0; i < l; i++)
                    for (int j = 0; j < n; j++)
                        for (int r = 0; r < m; r++)
                            C[i, j] += A[j, r] * B[j, r];

                A = C;
            }


        }

        public static Matrix transpose(Matrix A)
        {
            int n = A.Length.n;
            int m = A.Length.m;
            Matrix B = new Matrix(A.Length.m,A.Length.n);

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    B[j, i] = A[i, j];

            return B;
        }

        // public static void transpose() { }

        static int sign(double x)
        {
            if (x < 0) return -1;
            return 1;
        }

        Vector Haushold(Vector x, int k)
        {
            for (int i = 0; i < k; i++) x[i] = 0;

            double gamma = -Math.Sign(x[k]) * x.Norm;



            Vector x0 = x;
            x0[k] = x0[k] - gamma;
            double alpha = x0.Norm;



            Vector w = new Vector(x.Length);
            w = (1 / alpha) * x0;


            return w;
        }

     
     

        static double abs(double x)
        {
            if (x < 0) return -x;
            return x;
        }

        public static double maxdiag(Matrix A, Matrix B)
        {
            double max = 0;
            int n = A.Length.n;

            for (int i = 0; i < n; i++)
            {
                double x = abs(A[i, i] - B[i, i]);
                if (x > max) max = x;
            }
            return max;
        }

        public static double maxpoddiag(Matrix A, Matrix B)
        {
            Vector v = new Vector(A.Length.n);

            for (int i = 0; i < v.Length - 1; i++)
            {
                v[i] = Math.Abs(A[i + 1, i] - B[i + 1, i]);
            }
            return v.Norm;
        }

        public static void copy(ref Matrix A, Matrix B)
        {
            A = new Matrix(A.Length.n, A.Length.m);
            for (int i = 0; i < A.Length.n; i++)
                for (int j = 0; j < A.Length.n; j++)
                    A[i, j] = B[i, j];
        }


        public string ToTextBoxString()
        {
            string S = "";

            string number;

            for (int i = 0; i < n; i++)
            {
                S += "<";
                for (int j = 0; j < m; j++)
                {

                    number = M[i, j].ToString();
                    number += "  ";
                    number = number.Substring(0, 3);
                    S += number;
                    S += "; ";

                }
                S += ">" + Environment.NewLine;
            }
            S += " /";
            return S;
        }

        public double max_element()
        {
            double max = M[0, 0];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    if (max < M[i, j]) max = M[i, j];
                }
            return max;
        }

        public double min_element()
        {
            double min = M[0, 0];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    if (min > M[i, j]) min = M[i, j];
                }
            return min;
        }

    }
}
