using System;
//using MyMathLib;
using System.Windows.Forms;

namespace OptimalManaging
{
    public partial class Form1 : Form
    {
        OptimalManaging optm;
        int GRID_SIZE = 100;
        int TIME_SIZE = 1000;
        Vector y,x,tau,u_old;
        public Form1()
        {
            InitializeComponent();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        public class TestFunction1
        {
            public static double aa = 1d;
            public static double U(double x, double t)
            {
                return x*x;
            }

            public static double phi(double x)
            {
                return x*x;
            }

            public static double f(double x, double t)
            {
                return 2;
            }

            public static double p(double t)
            {
                return  3;
            }

            
            public static FunctionLib.Function y = linear;
        };
        public static double linear(double x)
        {
            return x*x*x;
        }
        public static double periodic(double x)
        {
            return 0.5d*Math.Cos(x*7d);
        }
        public static double exp(double x)
        {
            return 0.5d * Math.Exp(x*x/2d);
        }
        public static double constant(double x)
        {
            return 7d;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            TestFunction1.y = linear;
            Reload();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            TestFunction1.y = periodic;
            Reload();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            TestFunction1.y = exp;
            Reload();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            TestFunction1.y = constant;
            Reload();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Vector calc_p = optm.CalculateIteration();
            double J = optm.Functional_J(optm.calc_u);
            
            int ITER = 0;
            while (J > 0.001d && (u_old - optm.calc_u).Norm > 0.001d)
            {
                u_old = optm.calc_u;
                optm.CalculateIteration();
                J = optm.Functional_J(optm.calc_u);
                ITER++;
            }
            calc_p = optm.CalculateIteration();
            label1.Text = "J = " + J + Environment.NewLine;
            label1.Text += "||u - u_old|| = " + (u_old - optm.calc_u).Norm + Environment.NewLine;
            label1.Text += " ВСЕГО ИТЕРАЦИЙ: " + ITER;


            DrawOM.Draw(chart1, x, optm.calc_u, 1);
            DrawOM.Draw(chart2, tau, calc_p, 0);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label2.Text = "a^2 = " + (double)(trackBar1.Value) / 11d;
            TestFunction1.aa = (double)(trackBar1.Value) / 11d;
            Reload();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Reload();
                optm.pickAlpha = PickAlpha.Divide;
            }else
            {
                Reload();
                optm.pickAlpha = PickAlpha.Lagrange;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Vector calc_p = new Vector();
            for (int i = 0; i < 1; i++)
            {
                calc_p = optm.CalculateIteration();

            }

            DrawOM.Draw(chart1, x, optm.calc_u, 1);
            DrawOM.Draw(chart2, tau, calc_p, 0);

             
            double J = optm.Functional_J(optm.calc_u);
            label1.Text = "J = " + J;
            

            }


        private void Reload()
        {
            optm = new OptimalManaging(1d, 1d, TestFunction1.aa, 1d, TIME_SIZE, GRID_SIZE, 0.1, 0.1,
    TestFunction1.y, TestFunction1.p, TestFunction1.phi, TestFunction1.f, -100, 100, 50);
            DrawOM.SetNSeries(chart1, 2);
            DrawOM.SetNSeries(chart2, 1);
            y = MyMath.GetVectorFunction(GRID_SIZE, 0, 1d, TestFunction1.y);
            u_old = new Vector(GRID_SIZE);
            x = new Vector(MyMath.CreateUniformGrid(GRID_SIZE, 0, 1d));

            tau = new Vector(MyMath.CreateUniformGrid(TIME_SIZE, 0, 1d));

            DrawOM.Draw(chart1, x, y, 0);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Reload();


        }
    }
 }

