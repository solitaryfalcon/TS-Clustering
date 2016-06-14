using System;
using System.Collections.Generic;
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
using Visifire.Charts;

namespace TS_Clustering
{
    /// <summary>
    /// Interaction logic for WindowResultLineChart.xaml
    /// </summary>
    public partial class WindowResultLineChart : Window
    {
        public WindowResultLineChart(Chart m_chart)
        {
            InitializeComponent();
            GridLineChart.Children.Add(m_chart);
        }
        
    }
}
