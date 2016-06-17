using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Visifire.Charts;

namespace TS_Clustering
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private string filePath;
        private string outputPath;        

        private Initialization initMethod = Initialization.NORMAL;
        private Normalization normMethod = Normalization.NONE;
        private DataType dataType = DataType.DISTANCE;
        private Diversification diverMethod = Diversification.BALANCE;

        private BackgroundWorker bgw;
        private DataFileParser dataFileParser;
        private ClusterList clusterList;
        private string m_LB = "0";
        private string m_UB = "1000";
        DateTime startTime,endTime;
        FileStream fileL;
        public MainWindow()
        {
            InitializeComponent();
            //根据配置文件初始化基本参数
            //Load paprameters from setting.xml
            InitializeParameter();
            //设置工作进程脱离UI控制
            //Release UI by Backgroundworker
            bgw = new BackgroundWorker();
            bgw.WorkerReportsProgress = true;
            bgw.ProgressChanged += bgw_ProgressChanged;
            bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
            bgw.DoWork += bgw_DoWork;
        }

        private  void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            bgw.ReportProgress(0, "Reading data file...");//读取in文件
            readDataFile();
            bgw.ReportProgress(10, "Initializing ClusterList...");//初始化聚类对象
            clusterList = new ClusterList();
            bgw.ReportProgress(15, "Loading data...");//导入数据
            clusterList.importData(dataFileParser.inputScore, dataFileParser.normlizedScore);
            //record the start time 记录算法开始时间
            startTime = DateTime.Now;
            
            //构造初始解
            bgw.ReportProgress(20, "Constructing initial solution...");
            clusterList.doInitial(initMethod);
            bgw.ReportProgress(25, "Initializing Tabu Search...");
            clusterList.initTSPhase();
            bgw.ReportProgress(30, "Executing Tabu Search...");


            for (int i = 0; i < Globals.EleNum; i++)
            {
                Util.WriteLine("Element(" + i + "):" + dataFileParser.nameFormater.getName(i));
            }
            Util.WriteLine("********************************\n");
            Util.WriteLine("Movelog.");
            clusterList.execTSPhase(bgw,diverMethod);


            endTime = DateTime.Now;
            FileStream fileCluster = new FileStream(outputPath + "\\cluster_" + Globals.FileName + "_" + startTime.ToString("yyyyMMddHHmmss") + ".txt", FileMode.OpenOrCreate);
            using (StreamWriter sw = new StreamWriter(fileCluster))
            {
                bgw.ReportProgress(85, "Writing Cluster Result..");
                clusterList.exportResult(sw, dataFileParser.normlizedScore);
                if (normMethod != Normalization.NONE)
                {
                     sw.WriteLine("\n\r\n\r");
                     sw.WriteLine("\nClustering Result mapping to the original data:");
                    clusterList.exportResult(sw, dataFileParser.inputScore);
                }
                 sw.WriteLine("\n\r\n\r");
                //输出界面参数
                sw.WriteLine("LBound = " + m_LB + "\tUbound = " + m_UB);
                sw.WriteLine("A1 = " + Globals.A1 + "\tA2 = " + Globals.A2+"\tA3 = 0");
                 sw.WriteLine("Time Cost = " + (endTime-startTime).ToString("g"));
            }
            FileStream fileElement = new FileStream(outputPath + "\\element_" + Globals.FileName + "_" + startTime.ToString("yyyyMMddHHmmss") + ".txt", FileMode.OpenOrCreate);
            using (StreamWriter sw = new StreamWriter(fileElement))
            {
                bgw.ReportProgress(90, "Writing Elements Distribution Result..");
                if (dataType!=DataType.DISTANCE)
                {
                     sw.WriteLine("CID\t EID\t EName\t \tX \tY \tZ");
                    for (int i = 1; i <= Globals.CluNum; i++)
                    {
                        for (int j = 0; j < Globals.EleNum; j++)
                        {
                            if (clusterList.elements[j].ClusterID == i)
                            {
                                MyPoint p = dataFileParser.points[j];
                                 sw.WriteLine(i + "\t" + j + "\t" + dataFileParser.nameFormater.getName(j) + "\t"+p.X+"\t"+p.Y+"\t"+p.Z+'\n');
                            }
                        }
                    }
                }
                else//非点集的输出
                {
                     sw.WriteLine("CID\t EID\t EName\t ");
                    for (int i = 1; i <= Globals.CluNum; i++)
                    {
                        for (int j = 0; j < Globals.EleNum; j++)
                        {
                            if (clusterList.elements[j].ClusterID == i)
                            {
                                 sw.WriteLine(i + "\t" + j + "\t" + dataFileParser.nameFormater.getName(j) + '\n');
                            }
                        }
                    }
                }
            }
            bgw.ReportProgress(100, "Done! Time Cost ="+(endTime-startTime).ToString("g"));
        }
        
        private void readDataFile()//read data file and Calculate the distance matrix
        {
            StreamReader sr = new StreamReader(filePath);
            string line;
            #region read patameters
            try
            {
                Globals.FileName = sr.ReadLine();
                string[] filepathname = Globals.FileName.Split('.');
                Globals.FileName = filepathname[0];
                if (Globals.FileName.StartsWith("CP"))
                {
                    dataType = DataType.DISTANCE;
                }
                else if(Globals.FileName.StartsWith("TP"))
                {
                    dataType = DataType.MATRIX_2D;
                }
                else
                {
                    throw new Exception("Unknown datatype!");
                }
                line = sr.ReadLine();
                string[] information = line.Split(' ');
                // reject the duplicate ' '
                int i = 0;
                while (information[i] == "")
                {
                    i++;
                }
                Globals.EleNum = Convert.ToInt32(information[i]);
                i++;
                while (information[i] == "")
                {
                    i++;
                }
                Globals.CluNum = Convert.ToInt32(information[i]);
                //Update LBound based on Chapter 4.2 
                //原文档4.2中提到更新下限到一个特定值
                //int newLBound = (Globals.EleNum / Globals.CluNum) / 2;
                //if (Globals.LBound < newLBound)
                //{
                //    Globals.LBound = newLBound;
                //}
            }
            catch (Exception)
            {
                throw;
            }
            #endregion
            int tempAVG = Globals.EleNum / Globals.CluNum;
            if (tempAVG < Globals.LBound || tempAVG >= Globals.UBound)
            {
                throw new Exception("Wrong Bounds setting for this input data!");
            }
            dataFileParser = new DataFileParser(sr, dataType, normMethod);
            if (sr != null) sr.Close();
        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Util.Flush();
            btn_start.IsEnabled = true;
            if(e.Error != null)
            {
                System.Windows.MessageBox.Show(e.Error.Message.ToString());
                tb_log.Text += e.Error.Message.ToString() + "\n";
            }
            if (clusterList != null)
            {
                ArrayList values1 = new ArrayList(); ArrayList values3 = new ArrayList(); ArrayList values2 = new ArrayList();
                ArrayList temp = clusterList.valList;
                for(int i= 0;i< temp.Count; i++){
                    double[] dt = (double[])temp[i];
                    values1.Add(dt[0]); values2.Add(dt[1]); values3.Add(dt[2]);
                }
                if (values1 != null && values2 != null&&values3!=null)
                {
                    ChartHelper ch = new ChartHelper();
                    ChartInformation ci = new ChartInformation();
                    ci.ImportLineData(values1, values2,values3);
                    Chart m_chart = ch.CreateChart(ci);
                    WindowResultLineChart m_window = new WindowResultLineChart(m_chart);
                    m_window.Show();                    
                }
            }
        }

        private void InitializeParameter()
        {
            string confFilePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "setting.xml";
            string xml = "";
            try
            {
                tb_log.Text += "Loading configuration file\n";
                StreamReader sr = new StreamReader(confFilePath);
                xml = sr.ReadToEnd();
                if (sr != null)
                {
                    sr.Close();
                }
            }
            catch (Exception)
            {
                tb_log.Text += "Error in Loading configuration file.\n";
            }
            try
            {
                tb_log.Text += "Parsing setting.xml.\n";
                Util.SetDefaultParameters(xml);
            }
            catch (Exception)
            {
                tb_log.Text += "Error in parsing setting.xml.\n";
            }
            tb_log.Text += "Ready.\n";
        }


        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            #region Confirm the format
            if (tb_inputFilePath.Text == String.Empty 
                || tb_outputPath.Text == String.Empty 
                || tb_lbound.Text == String.Empty 
                || tb_ubound.Text == String.Empty
                || tb_a1.Text == String.Empty
                || tb_a2.Text == String.Empty
                || tb_a3.Text==String.Empty)
            {
                System.Windows.MessageBox.Show("Please complete input!");
                return;
            }
            try
            {
                Globals.LBound = Convert.ToInt32(tb_lbound.Text);
                Globals.UBound = Convert.ToInt32(tb_ubound.Text);
                Globals.A1 = Convert.ToDouble(tb_a1.Text);
                Globals.A2 = Convert.ToDouble(tb_a2.Text);
               
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Input string was not in a correct format.");
                return;
            }
            if (Globals.LBound >= Globals.UBound)
            {
                System.Windows.MessageBox.Show("UBound is smaller than LBound!");
                return;
            }
            if ((Globals.A1 > 1 || Globals.A1 < 0) 
                ||(Globals.A2 > 1 || Globals.A2 < 0))
            {
                System.Windows.MessageBox.Show("The weight a1,a2,a3 ∈ (0,1]");
                return;
            }
            #endregion
            if (rb_hierarchical.IsChecked.Value)
            {
                initMethod = Initialization.HIERARCHICHAL;
            }
            else if (rb_RHier.IsChecked.Value)
            {
                initMethod = Initialization.RHIE;
            }else
            {
                initMethod = Initialization.NORMAL;
            }
            if (rb_minMax.IsChecked.Value)
            {
                normMethod = Normalization.MIN_MAX;
            }
            else
            {
                normMethod = Normalization.NONE;
            }
            if (rb_balance.IsChecked.Value)
            {
                diverMethod = Diversification.BALANCE;
            }
            else {
                diverMethod = Diversification.RERHIE;
            }
            fileL = new FileStream(outputPath + "\\log_" + Globals.FileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt", FileMode.OpenOrCreate,FileAccess.Write,FileShare.None,4096,true);
            Util.sw = new StreamWriter(fileL);
            m_LB = tb_lbound.Text;
            m_UB = tb_ubound.Text;
            //将聚类任务交给bgw，锁定开始按钮
            //assign the task to bgw, disable the start button
            bgw.RunWorkerAsync();
            btn_start.IsEnabled = false;

        }
        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pb_main.Value = e.ProgressPercentage;
            tb_log.Text += e.UserState.ToString()+"\n";
            tb_log.ScrollToEnd();
        }
        private void btn_outputFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openFolderDlg = new FolderBrowserDialog();
            if (this.tb_inputFilePath.Text != "")
            {
                openFolderDlg.SelectedPath = this.tb_inputFilePath.Text;
            }
            openFolderDlg.ShowDialog();
            if (openFolderDlg.SelectedPath != string.Empty)
                outputPath = openFolderDlg.SelectedPath;
            this.tb_outputPath.Text = outputPath;
        }

        private void btn_inputFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();
            openFileDialog1.Filter = "file(*.*)|*.in;*.txt";
            if (openFileDialog1.ShowDialog() == true)
            {
                if (openFileDialog1.FileName != "")
                {
                    filePath = openFileDialog1.FileName;
                    //outputpath=openFileDialog1
                    this.tb_inputFilePath.Text = filePath;
                    string[] temp = filePath.Split('\\');
                    outputPath = temp[0];
                    for (int i = 1; i < temp.Length - 1; i++)
                    {
                        outputPath += "\\" + temp[i];
                    }
                    this.tb_outputPath.Text = outputPath;
                }
            }
        }

        private void btn_calculator_Click(object sender, RoutedEventArgs e)
        {
            ObjValCalculator objwin = new ObjValCalculator();
            objwin.ShowDialog();
        }

    }
}
