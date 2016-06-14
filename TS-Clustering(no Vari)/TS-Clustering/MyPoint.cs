using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS_Clustering
{
    class MyPoint
    {
        private double x,y,z;
        public  MyPoint()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public double Z
        {
            get
            {
                return z;
            }

            set
            {
                z = value;
            }
        }
    }
}
