using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_Clustering
{
    class WriterHelper
    {
        static FileStream fileLog, fileElement, fileCluster;
        static StreamWriter writerC, writerE;
        string outputPath, v;
        StreamWriter writerL;
        public WriterHelper(string a, string b)
        {
            this.outputPath = a;
            this.v = b;
            //fileLog = new FileStream(outputPath + "\\log_" + v, FileMode.OpenOrCreate);
            fileElement = new FileStream(outputPath + "\\element_" + v, FileMode.OpenOrCreate);
            fileCluster = new FileStream(outputPath + "\\cluster_" + v, FileMode.OpenOrCreate);
            writerC = new StreamWriter(fileCluster);
            writerE = new StreamWriter(fileElement);
            writerL = new StreamWriter(new FileStream(outputPath + "\\log_" + v, FileMode.OpenOrCreate));
        }

        public async void Write(LogType type,string content)
        {
            switch (type)
            {
                case LogType.CLUSTER:
                    lock(writerC)
                    {
                        writerC.WriteLineAsync(content);
                    }
                    break;
                case LogType.ELEMENT:
                        writerE.WriteLineAsync(content);
                    break;
                case LogType.MOVE:
                    await writerL.WriteLineAsync(content);
                    await writerL.FlushAsync();
                    break;

            }

        }
    }
}
