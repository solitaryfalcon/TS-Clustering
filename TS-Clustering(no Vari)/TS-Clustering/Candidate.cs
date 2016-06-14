using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS_Clustering
{
    class Candidate
    {
        public Candidate(int elementID, int to_ClusterID)
        {
            EleID = elementID;
            TOCluID = to_ClusterID;
        }
        public int EleID;
        public int TOCluID;
    }
}
