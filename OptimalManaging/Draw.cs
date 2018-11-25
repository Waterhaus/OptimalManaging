using System;
using System.Collections.Generic;
using System.Linq;
using MyMathLib;
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

        public static void Draw(Chart chart, Vector x, Vector y, int SeriesNumer)
        {
            chart.Series[SeriesNumer].Points.Clear();
            for (int i = 0; i < y.Length; i++)
                chart.Series[SeriesNumer].Points.AddXY(x[i], y[i]);
        }


        




    }
}
