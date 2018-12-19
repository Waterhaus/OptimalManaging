using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace OptimalManaging
{
    class MyColor
    {
        private double red, green, blue;
        
        public Color ToColor
        {
            get { return Convert(); }
        }

        public MyColor()
        {
            red = green = blue = 0;
        }

        public MyColor(double r, double g, double b)
        {
            red = r;
            green = g;
            blue = b;

            if (red > 1)   red = 1;
            if (blue > 1)  blue = 1;
            if (green > 1) green = 1;

            if (red < 0)   red = 0;
            if (blue < 0)  blue = 0;
            if (green < 0) green = 0;
        }

        private Color Convert()
        {
            int r = (int)(255 * red);
            int g = (int)(255 * green);
            int b = (int)(255 * blue);

            return Color.FromArgb(r, g, b);
        }

        public static MyColor operator *(double a, MyColor b)
        {


            b.red *= a;
            b.green *= a;
            b.blue *= a;

            if (b.red > 1) b.red = 1;
            if (b.blue > 1) b.blue = 1;
            if (b.green > 1) b.green = 1;

            if (b.red < 0) b.red = 0;
            if (b.blue < 0) b.blue = 0;
            if (b.green < 0) b.green = 0;

            return b;
        }

        public static MyColor operator + (MyColor a, MyColor b)
        {
            MyColor c = new MyColor();

            c.red = a.red + b.red;
            c.blue = a.blue + b.blue;
            c.green = a.green + b.green;

            if (c.red > 1) c.red = 1;
            if (c.blue > 1) c.blue = 1;
            if (c.green > 1) c.green = 1;

            if (c.red < 0) c.red = 0;
            if (c.blue < 0) c.blue = 0;
            if (c.green < 0) c.green = 0;

            return c;
        }

        public static MyColor Red
        {
            get { return new MyColor(1, 0, 0); }
        }

        public static MyColor Green
        {
            get { return new MyColor(0, 1, 0); }
        }

        public static MyColor Blue
        {
            get { return new MyColor(0, 0, 1); }
        }

        public static MyColor White
        {
            get { return new MyColor(1, 1, 1); }
        }

        public static MyColor Black
        {
            get { return new MyColor(0, 0, 0); }
        }
    }
}
