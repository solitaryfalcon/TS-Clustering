using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_Clustering
{
    class Element
    {
        public int ClusterID;
        public readonly int ID;
        public Element Before, After;
       

        public Element(int i)
        {
            this.ID = i;
            Before = null;
            After = null;
        }

        internal void MoveTo(ClusterList s, int toID)
        {//移动元素操作
            Cluster from = s.clusters[ClusterID];
            Cluster to = s.clusters[toID];
            //if it's the first element
            if (Before == null)
            {
                from.First = After;
                if (After != null)
                {
                    After.Before = null;
                }
            }
            else
            {
                Before.After = After;
                if (After != null)
                {
                    After.Before = Before;
                }
            }
            from.Num--;
            from.NumberLinks = from.NumberLinks - from.Num;
            Before = null;
            After = null;
            //remove then insert
            to.Add(this);
        }
    }
}
