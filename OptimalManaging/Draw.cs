using System;
using System.Collections.Generic;
using System.Linq;
//using MyMathLib;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace OptimalManaging
{
    class DrawOM
    {

        public static void SetNSeries(Chart chart, int N)
        {
            chart.Series.Clear();

            for (int i = 0; i < N; i++)
            {
                chart.Series.Add(new Series("Function " + i));
                chart.Series[i].ChartType = SeriesChartType.Line;
                chart.Series[i].BorderWidth = 4;
            }
        }

        public static void SetHeatMap(Chart chart)
        {
            chart.Series.Clear();

            
            
                chart.Series.Add(new Series("HeatMap "));
                chart.Series[0].ChartType = SeriesChartType.Point;
                //chart.Series[i].BorderWidth = 4;
            
        }

        public static void Draw(Chart chart, Vector x, Vector y, int SeriesNumer)
        {
            chart.Series[SeriesNumer].Points.Clear();
            for (int i = 0; i < y.Length; i++)
                chart.Series[SeriesNumer].Points.AddXY(x[i], y[i]);
        }


        public static void DrawHeatMap(Chart chart, Matrix A, int SeriesNumer)
        {

            chart.Series[SeriesNumer].Points.Clear();
            int N = A.Length.n;
            int M = A.Length.m;
            double min = A.min_element();
            double max = A.max_element();

            Vector x = Vector.CreateUniformGrid(N, 0, 1);
            Vector y = Vector.CreateUniformGrid(M, 0, 1);

            MyColor C = new MyColor();

            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                {
                    chart.Series[SeriesNumer].Points.AddXY(x[i], y[j]);
                    double a = 1;
                    if (min != max)
                    {

                        a = (A[i, j] - min) / (max - min);
                        C = (1 - a) * MyColor.Blue + a * MyColor.Red;
                        // MessageBox.Show(a.ToString());
                    }

                    chart.Series[SeriesNumer].Points[i * N + j].Color = C.ToColor;

                }
        }








    }
}
