using System;
//using MyMathLib;
using System.Windows.Forms;

namespace OptimalManaging
{
    public partial class Form1 : Form
    {
        OptimalManaging optm;
        int GRID_SIZE = 100;
        int TIME_SIZE = 100;
        int Iter = 0;
        Vector y,x,tau,u_old;
        PickAlpha pick;

        //--------------------
        double R = 7;
        double a = 1;
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
                return -2d;
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
            label1.Text = "Информация " + Environment.NewLine;
            label1.Text += "J = " + J + Environment.NewLine;
            label1.Text += "||u - u_old|| = " + (u_old - optm.calc_u).Norm + Environment.NewLine;
            label1.Text += "Количество итераций: " + ITER;


            DrawOM.Draw(chart1, x, optm.calc_u, 1);
            DrawOM.DrawHeatMap(chart2, optm.manage_f, 0);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            pick = PickAlpha.Lipshiz;
            Reload();
            //optm.pickAlpha = PickAlpha.Lagrange;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            R = Double.Parse( textBox1.Text );
            a = Double.Parse(textBox4.Text);
            GRID_SIZE = Int32.Parse(textBox3.Text);
            TIME_SIZE = Int32.Parse(textBox2.Text);
            Reload();
        }

        private void radioButton6_CheckedChanged_1(object sender, EventArgs e)
        {
            pick = PickAlpha.Divide;
            Reload();
            optm.pickAlpha = PickAlpha.Divide;

        }

        private void radioButton5_CheckedChanged_1(object sender, EventArgs e)
        {
            pick = PickAlpha.Lipshiz_CG;
            Reload();
            optm.pickAlpha = PickAlpha.Lipshiz_CG;

        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            pick = PickAlpha.SUM;
            Reload();
            optm.pickAlpha = PickAlpha.SUM;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {

            pick = PickAlpha.Projection;
            Reload();
            optm.pickAlpha = PickAlpha.Projection;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Vector calc_p = new Vector();
            for (int i = 0; i < 1; i++)
            {
                calc_p = optm.CalculateIteration();

            }
            
            DrawOM.Draw(chart1, x, optm.calc_u, 1);
            //DrawOM.Draw(chart2, tau, calc_p, 0);
            DrawOM.DrawHeatMap(chart2, optm.manage_f, 0);
            Iter++;
            double J = optm.Functional_J(optm.calc_u);
            label1.Text = "Информация " + Environment.NewLine;
            label1.Text += "J(u_"+Iter+") = " + J + Environment.NewLine;
            label1.Text += "||u - u_old|| = " + (u_old - optm.calc_u).Norm + Environment.NewLine;
            label1.Text += "Итерация: " + Iter + Environment.NewLine;
            label1.Text += "alpha = " + optm.alpha_old;
            u_old = optm.calc_u;

        }


        private void Reload()
        {
            optm = new OptimalManaging(1d, 1d, a, 1d, TIME_SIZE, GRID_SIZE, 0.1, 0.1,
    TestFunction1.y, TestFunction1.p, TestFunction1.phi, TestFunction1.f, -100, 100, R);
            optm.pickAlpha = pick;
            DrawOM.SetNSeries(chart1, 2);
            DrawOM.SetHeatMap(chart2);
            y = MyMath.GetVectorFunction(GRID_SIZE, 0, 1d, TestFunction1.y);
            u_old = new Vector(GRID_SIZE);
            x = new Vector(MyMath.CreateUniformGrid(GRID_SIZE, 0, 1d));

            tau = new Vector(MyMath.CreateUniformGrid(TIME_SIZE, 0, 1d));
            Iter = 0;
            DrawOM.Draw(chart1, x, y, 0);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            radioButton7.Checked = true;
            Reload();

            chart2.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
            chart2.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = false;

            chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
        }
    }
 }

