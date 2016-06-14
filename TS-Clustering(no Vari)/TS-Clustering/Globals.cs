using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_Clustering
{
    class Globals
    {
        //parameter setting file
        private static int maxIter = 0;
        private static int noImproveLimit = 0;
        private static float cutOffParam = 1;
        private static float relaxFactor = 1;               //used in building candidate list

        //data file 
        private static int eleNum;
        private static int cluNum;
        private static string fileName;
        //panel settings
        private static double a1, a2;
        private static int uBound, lBound;

        //internal settings
        private static float cutOff;
        private static float average;

        public static int MaxIter
        {
            get
            {
                return maxIter;
            }

            set
            {
                maxIter = value;
            }
        }

        public static int NoImproveLimit
        {
            get
            {
                return noImproveLimit;
            }

            set
            {
                noImproveLimit = value;
            }
        }

        public static float CutOffParam
        {
            get
            {
                return cutOffParam;
            }

            set
            {
                cutOffParam = value;
            }
        }

        public static float Average
        {
            get
            {
                return average;
            }

            set
            {
                average = value;
            }
        }

        public static float RelaxFactor
        {
            get
            {
                return relaxFactor;
            }
            set
            {
                relaxFactor = value;
            }
        }

        public static float CutOff
        {
            get
            {
                return cutOff;
            }
            set
            {
                cutOff = value;
            }
        }
        public static int CluNum
        {
            get
            {
                return cluNum;
            }

            set
            {
                cluNum = value;
            }
        }

        public static int EleNum
        {
            get
            {
                return eleNum;
            }

            set
            {
                eleNum = value;
            }
        }

        public static string FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                fileName = value;
            }
        }

        public static double A1
        {
            get
            {
                return a1;
            }

            set
            {
                a1 = value;
            }
        }

        public static double A2
        {
            get
            {
                return a2;
            }

            set
            {
                a2 = value;
            }
        }

        public static int UBound
        {
            get
            {
                return uBound;
            }

            set
            {
                uBound = value;
            }
        }
        
        public static int LBound
        {
            get
            {
                return lBound;
            }

            set
            {
                lBound = value;
            }
        }

        public static int NoImprovePhase { get; internal set; }
    }
}
