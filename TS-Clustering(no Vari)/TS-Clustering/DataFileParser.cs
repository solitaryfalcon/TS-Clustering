using System;
using System.IO;
using System.Linq;

namespace TS_Clustering
{
    class DataFileParser
    {
        public NameFormater nameFormater;
        public double[,] inputScore;
        public double[,] normlizedScore;
        public MyPoint[] points;

        private DataType dataType;
        private Normalization normMethod;
      
        public DataFileParser(StreamReader sr, DataType dataType, Normalization normMethod)
        {
            this.dataType = dataType;
            this.normMethod = normMethod;
            nameFormater = new NameFormater(Globals.EleNum);
            inputScore = new double[Globals.EleNum, Globals.EleNum];
            switch (dataType)
            {
                case DataType.DISTANCE:
                    ParseDistance(sr);
                    break;
                case DataType.MATRIX_2D:
                    ParseMatrix2D(sr);
                    break;
            }
            switch (normMethod)
            {
                case Normalization.NONE:
                    normlizedScore = (double[,])inputScore.Clone();
                    break;
                case Normalization.MIN_MAX:
                    normlizedScore = MinMaxNormalization(Globals.EleNum);
                    break;
                default:
                    break;
            }

        }

        private double[,] MinMaxNormalization(int eleNum)
        {
            double max = inputScore[0, 0];
            double min = inputScore[0, 0];
            foreach (double a in inputScore)
            {
                if (a > max)
                {
                    max = a;
                }
                else if (a < min)
                {
                    min = a;
                }
            }
            double temp = max - min;
            double[,] result = new double[eleNum, eleNum];
            for (int i = 0; i < eleNum; i++)
            {
                for (int j = 0; j < eleNum; j++)
                {
                    result[i, j] = (inputScore[i, j] - min) / temp;
                }
            }
            return result;
        }

        private void ParseMatrix2D(StreamReader sr)
        {
            points = new MyPoint[Globals.EleNum];
            string line;
            int num = 0;
            while (!(line = sr.ReadLine()).Contains("-999"))
            {
                string[] str_temp = line.Split('(');
                string[] name = str_temp[0].Split(' ');
                int x = nameFormater.indexName(name[0]);
                if (x != num)
                {
                    throw new Exception("Duplicate point name!");
                }
                str_temp = str_temp[1].Split(')');
                str_temp = str_temp[0].Split(',');
                points[num] = new MyPoint();
                points[num].X = double.Parse(str_temp[0]);
                points[num].Y = double.Parse(str_temp[1]);
                if (str_temp.Count() > 2)
                {
                    points[num].Z = double.Parse(str_temp[2]);
                }
                num++;
            }
            if (num != Globals.EleNum)
            {
                throw new Exception("Data Missing!");
            }
            for (int i = 0; i < Globals.EleNum; i++)
            {
                for (int j = 0; j < Globals.EleNum; j++)
                {
                    inputScore[i, j] = MathHelper.Distance(points[i], points[j]);
                }
            }
        }

        private void ParseDistance(StreamReader sr)
        {
            string line;
            while (!(line = sr.ReadLine()).Contains("-999"))
            {
                string[] sarray = line.Split(' ');
                int temp = sarray.Count();
                int x = 0, y = 0;
                double value = 0;
                int count = 0;
                for (int i = 0; i < temp; i++)
                {
                    if (sarray[i] != "" && sarray[i] != "\t" && count == 0)
                    {
                        x = nameFormater.indexName(sarray[i]);
                        count++;
                    }
                    else if (sarray[i] != "" && sarray[i] != "\t" && count == 1)
                    {
                        y = nameFormater.indexName(sarray[i]);
                        count++;
                    }
                    else if (sarray[i] != "" && sarray[i] != "\t" && count == 2)
                    {
                        value = Convert.ToDouble(sarray[i]);
                        count++;
                    }
                }
                if (value > 0)
                {
                    inputScore[x, y] = value;
                    inputScore[y, x] = value;
                }
                else
                {
                    inputScore[x, y] = -value;
                    inputScore[y, x] = -value;
                }
            }
        }
    }
}
