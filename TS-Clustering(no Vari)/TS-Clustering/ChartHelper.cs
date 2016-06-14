using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Visifire.Charts;

namespace TS_Clustering
{
    class ChartInformation
    {
        public Thickness m_BorderThickness;
        public string m_Theme;
        public bool m_View3D;
        public string m_axisXTitle;
        public string m_axisYTitle;
        //public int m_axisYMaximum;
        //public int m_axisYInterval;
        public DataSeriesCollection dsc;
        public ChartInformation()
        {

            m_BorderThickness = new Thickness(1.0);
            m_Theme = "Theme1";
            m_View3D = false;
            m_axisXTitle = "Iterations";
            m_axisYTitle = "Value";
        }
        public void ImportLineData(ArrayList values)
        {
            dsc = new DataSeriesCollection();
            DataSeries ds = new DataSeries();
            ds.RenderAs = RenderAs.Line;
            foreach (Object value in values)
            {
                DataPoint dp = new DataPoint();
                dp.YValue = (double)value;
                ds.DataPoints.Add(dp);
            }

            dsc.Add(ds);

        }
        public void ImportLineData(ArrayList values1, ArrayList values2,ArrayList values4)
        {
            dsc = new DataSeriesCollection();
            DataSeries ds1 = new DataSeries();
            ds1.RenderAs = RenderAs.Line;
            ds1.LegendText = "Sum";
            for (int i = 0; i < values1.Count; i = i + 4)
            {
                DataPoint dp = new DataPoint();
                dp.YValue = (double)values1[i];
                dp.XValue = i;
                ds1.DataPoints.Add(dp);
            }

            DataSeries ds2 = new DataSeries();
            ds2.RenderAs = RenderAs.Line;
            ds2.LegendText = "Average";
            for (int i = 0; i < values2.Count; i = i + 4)
            {
                DataPoint dp = new DataPoint();
                dp.YValue = (double)values2[i];
                dp.XValue = i;
                ds2.DataPoints.Add(dp);
            }
            //DataSeries ds3 = new DataSeries();
            //ds3.RenderAs = RenderAs.Line;
            //ds3.LegendText = "Variance";
            //for (int i = 0; i < values3.Count; i = i + 4)
            //{
            //    DataPoint dp = new DataPoint();
            //    dp.YValue = (double)values3[i];
            //    dp.XValue = i;
            //    ds3.DataPoints.Add(dp);
            //}
            DataSeries ds4 = new DataSeries();
            ds4.RenderAs = RenderAs.Line;
            ds4.LegendText = "ObjValue";
            for (int i = 0; i < values4.Count; i = i + 4)
            {
                DataPoint dp = new DataPoint();
                dp.YValue = (double)values4[i];
                dp.XValue = i;
                ds4.DataPoints.Add(dp);
            }
            dsc.Add(ds1);
            dsc.Add(ds2);
            //dsc.Add(ds3);
            dsc.Add(ds4);


        }
    }
    class ChartHelper
    {

        private Chart m_chart;
        public Chart CreateChart(ChartInformation ci)
        {
            m_chart = new Chart();

            m_chart.ToolBarEnabled = true;
            m_chart.ZoomingEnabled = true;
            m_chart.ZoomingMode = ZoomingMode.MouseDragAndWheel;

            m_chart.BorderThickness = ci.m_BorderThickness;
            m_chart.Theme = ci.m_Theme;
            m_chart.View3D = ci.m_View3D;

            Axis m_axisX = new Axis();
            m_axisX.Title = ci.m_axisXTitle;
            m_chart.AxesX.Add(m_axisX);

            Axis m_asixY = new Axis();
            m_asixY.Title = ci.m_axisYTitle;
            m_asixY.Enabled = true;
            m_asixY.StartFromZero = true;
            m_asixY.AxisType = AxisTypes.Primary;
            //m_asixY.AxisMaximum = ci.m_axisYMaximum;
            //m_asixY.Interval = ci.m_axisYInterval;
            m_chart.AxesY.Add(m_asixY);
            for (int i = 0; i < ci.dsc.Count; i++)
            {
                DataSeries ds = new DataSeries();
                ds.LegendText = ci.dsc[i].LegendText;
                ds.RenderAs = ci.dsc[i].RenderAs;
                ds.AxisYType = ci.dsc[i].AxisYType;
                ds.DataPoints = new DataPointCollection(ci.dsc[i].DataPoints);
                m_chart.Series.Add(ds);
            }
            m_chart.Rendered += new EventHandler(chart_Rendered);
            //添加图例点击响应事件
            SetLegendwithClickHandler();
            return m_chart;
        }
        private void SetLegendwithClickHandler()
        {
            foreach (var serie in m_chart.Series)
            {
                Legend legend = new Legend();

                legend.MouseLeftButtonDown += new EventHandler<LegendMouseButtonEventArgs>(legend_MouseLeftButtonDown);
                m_chart.Legends.Add(legend);
            }
        }
        void legend_MouseLeftButtonDown(object sender, LegendMouseButtonEventArgs e)
        {
            DataSeries ds = e.DataSeries;
            if (ds != null)
            {
                if (ds.Opacity == 1)
                {
                    ds.Opacity = 0;
                }
                else
                {
                    ds.Opacity = 1;
                }
            }
        }
        void chart_Rendered(object sender, EventArgs e)
        {
            Chart c = sender as Chart;
            Legend legend = c.Legends[0];
            Grid root = legend.Parent as Grid;
            int i = root.Children.Count;
            root.Children.RemoveAt(i - 7);
        }

    }
}
