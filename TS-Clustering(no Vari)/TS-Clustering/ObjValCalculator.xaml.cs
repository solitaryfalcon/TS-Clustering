using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TS_Clustering
{
    /// <summary>
    /// ObjValCalculator.xaml 的交互逻辑
    /// 给定聚类结果和输入数据，依此计算ObjVal
    /// </summary>
    public partial class ObjValCalculator : Window
    {
        string dataFilePath,resultFilePath;
        double[,] inputScore;
        public ObjValCalculator()
        {
            InitializeComponent();
        }

        private void btn_data_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();
            openFileDialog1.Filter = "file(*.*)|*.in;*.txt";
            if (openFileDialog1.ShowDialog() == true)
            {
                if (openFileDialog1.FileName != "")
                {
                    dataFilePath = openFileDialog1.FileName;
                    this.textBox1.Text = dataFilePath;
                }
            }
        }

        private void btn_result_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();
            openFileDialog1.Filter = "file(*.*)|*.in;*.txt";
            if (openFileDialog1.ShowDialog() == true)
            {
                if (openFileDialog1.FileName != "")
                {
                    resultFilePath = openFileDialog1.FileName;
                    this.textBox2.Text = resultFilePath;
                }
            }
        }



        //private void btn_start_Click(object sender, RoutedEventArgs e)
        //{
            
        //    bool ifPoints;
        //    StreamReader sr = new StreamReader(dataFilePath);
        //    string line, FileName;
        //    int ElementNum, ClusterNum;

        //    //读距离数据 
        //    #region//load the inputdata
        //    try
        //    {
        //        FileName = sr.ReadLine();
        //        string[] filepathname = FileName.Split('.');
        //        string filename = filepathname[0];
        //        if (filename.Contains("TP"))
        //        {
        //            ifPoints = true;
        //        }
        //        else {
        //            ifPoints = false;
        //        }
        //        line = sr.ReadLine();
        //        string[] information = line.Split(' ');
        //        // reject the duplicate ' '
        //        int i = 0;
        //        while (information[i] == "")
        //        {
        //            i++;
        //        }
        //        ElementNum = Convert.ToInt32(information[i]);
        //        i++;
        //        while (information[i] == "")
        //        {
        //            i++;
        //        }
        //        ClusterNum = Convert.ToInt32(information[i]);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //    inputScore = new double[ElementNum, ElementNum];
        //    NameFormater nf = new NameFormater(ElementNum);
        //    if (!ifPoints)
        //    {
        //        while (!(line = sr.ReadLine()).Contains("-999"))
        //        {
        //            string[] sarray = line.Split(' ');
        //            int temp = sarray.Count();
        //            int x = 0, y = 0;
        //            double value = 0;
        //            int count = 0;
        //            for (int i = 0; i < temp; i++)
        //            {
        //                if (sarray[i] != "" && sarray[i] != "\t" && count == 0)
        //                {
        //                    x = nf.indexName(sarray[i])-1;
        //                    count++;
        //                }
        //                else if (sarray[i] != "" && sarray[i] != "\t" && count == 1)
        //                {
        //                    y = nf.indexName(sarray[i])-1;
        //                    count++;
        //                }
        //                else if (sarray[i] != "" && sarray[i] != "\t" && count == 2)
        //                {
        //                    value = Convert.ToDouble(sarray[i]);
        //                    count++;
        //                }
        //            }
        //            if (value > 0)
        //            {
        //                inputScore[x, y] = value;
        //                inputScore[y, x] = value;
        //            }
        //            else
        //            {
        //                inputScore[x, y] = -value;
        //                inputScore[y, x] = -value;
        //            }
        //        }
        //    }
        //    else//输入参数为点集
        //    {
        //        System.Windows.MessageBox.Show("error format in input file ");
        //        return;
        //    }
        //    if (sr != null) sr.Close();
        //    Cluster[] clusters = new Cluster[ClusterNum];
        //    for(int i = 0; i < ClusterNum; i++)
        //    {
        //        clusters[i] = new Cluster(i);
        //    }
        //    Element[] elements = new Element[ElementNum];
        //    #endregion
        //    //读结果数据
        //    #region// load the clustering result
        //    sr = new StreamReader(resultFilePath);
        //    try
        //    {
        //        for(int i = 0; i < ElementNum; i++)
        //        {
        //            line = sr.ReadLine();
        //            string[] sarray = line.Split('\t');
        //            int eid, cid;

        //            eid = nf.indexName(sarray[0]) - 1;

        //            cid = Convert.ToInt32( sarray[1])-1;
        //            Element tempelement = new Element();
        //            tempelement.setElementID(eid);
        //            tempelement.setClusterID(cid);
        //            elements[eid] = tempelement;
        //            clusters[cid].Add(elements[eid]);
        //        }
        //    }
        //    catch(Exception)
        //    {
        //        throw ;
        //    }
        //    if (sr != null) sr.Close();
        //    #endregion
        //    double AVG = 0;
        //    double VARI = 0;
        //    String detail = "CID\t p\t w\t\t q\t\t w/q\t\t Variance\n";
        //    for(int i = 0; i < ClusterNum; i++)
        //    {
        //        double tempAVG= getAvg(clusters[i]);
        //        AVG += tempAVG * clusters[i].NumberLinks;
        //        double tempVARI= getVari(clusters[i], tempAVG);
        //        VARI += tempVARI;
        //        detail += (i+1).ToString("") + " \t " + clusters[i].num.ToString("") + "\t" + (tempAVG*clusters[i].NumberLinks).ToString("0.00") + "\t\t" + clusters[i].NumberLinks + "\t\t" + tempAVG.ToString("0.0000") + "\t\t" + tempVARI.ToString("0.0000") + '\n';
        //    }
        //    this.tb_aver.Text = AVG.ToString("0.000000");
        //    this.tb_vari.Text = VARI.ToString("0.000000");
        //    this.tb_detail.Text = detail;

        //}

        //private double getVari(Cluster cluster,double fullval)
        //{
        //    double total = 0;
        //    Element eleA = cluster.First;
        //    while (eleA != null)
        //    {
        //        Element eleB = eleA.After;
        //        while (eleB != null)
        //        {
        //            double diff = inputScore[eleA.ElementID, eleB.ElementID] - fullval;
        //            total += diff*diff;
        //            eleB = eleB.After;
        //        }
        //        eleA = eleA.After;
        //    }
        //    return total / cluster.NumberLinks;

        //}

        //private double getAvg(Cluster cluster)
        //{
        //    double total = 0;
        //    Element eleA = cluster.First;
        //    while (eleA != null)
        //    {
        //        Element eleB = eleA.After;
        //        while (eleB != null)
        //        {
        //            total += inputScore[eleA.ElementID, eleB.ElementID];
        //            eleB = eleB.After;
        //        }
        //        eleA = eleA.After;
        //    }
        //    return total / cluster.NumberLinks;
        //}

        private void tb_a2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            autoResult();
        }

        private void tb_a1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            autoResult();
        }
        private void tb_a3_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            autoResult();
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {

        }

        private void autoResult()
        {
            try
            {
                double a1 = Convert.ToDouble(tb_a1.Text);
                double a2 = Convert.ToDouble(tb_a2.Text);
                double a3 = Convert.ToDouble(tb_a3.Text);
                double sum = Convert.ToDouble(tb_sum.Text);
                double avg = Convert.ToDouble(tb_aver.Text);
                double vari = Convert.ToDouble(tb_vari.Text);

                double result = a1 * sum + a2 * avg+a3*vari;
                this.tb_result.Text = result.ToString("0.0000000");
            }
            catch(Exception e)
            {
                this.tb_result.Text = e.ToString();
            }
        }
    }
}
