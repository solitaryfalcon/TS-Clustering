using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_Clustering
{
    class MathHelper
    {
        static public double Distance(MyPoint X, MyPoint Y)
        {
            double xd = X.X - Y.X;
            double yd = X.Y - Y.Y;
            double zd = X.Z - Y.Z;
            double d = xd * xd + yd * yd + zd * zd;
            d = Math.Sqrt(d);
            return d;
        }

        static public double Min(double a, double b)
        {
            if (a > b)
            {
                return b;
            }
            else
            {
                return a;
            }
        }
        static public double Max(double a, double b)
        {
            if (a < b)
            {
                return b;
            }
            else
            {
                return a;
            }
        }
        static public int Min(int X, int Y)
        {
            if (X > Y)
                return Y;
            return X;
        }
    }
}
