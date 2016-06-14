using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TS_Clustering
{
    public class Util
    {
        public static StreamWriter sw;
        internal static void SetDefaultParameters(string xml)
        {
            XmlDocument source_xmldoc = new XmlDocument();
            source_xmldoc.LoadXml(xml);
            XmlNode rootNode = source_xmldoc.DocumentElement;

            XmlNode XNode = rootNode.SelectSingleNode("noImproveLimit");
            if (XNode != null)
                Globals.NoImproveLimit = Convert.ToInt32(XNode.InnerText);

            XNode = rootNode.SelectSingleNode("maxIter");
            if (XNode != null)
                Globals.MaxIter = Convert.ToInt32(XNode.InnerText); 

            XNode = rootNode.SelectSingleNode("noImprovePhase");
            if (XNode != null)
                Globals.NoImprovePhase = Convert.ToInt32(XNode.InnerText);

            XNode = rootNode.SelectSingleNode("cutOff");
            if (XNode != null)
                Globals.CutOffParam= Convert.ToSingle(XNode.InnerText);


            XNode = rootNode.SelectSingleNode("relaxFactor");
            if (XNode != null)
                Globals.RelaxFactor = Convert.ToSingle(XNode.InnerText);
        }

        public static async void WriteLine(String content)
        {
            try
            {
                await sw.WriteLineAsync(content);
            }
            catch (Exception) { }
            
        }

        internal static void Flush()
        {
            sw.Flush();
            sw.Close();
        }
    }
}
