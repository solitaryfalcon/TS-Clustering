using System.Collections.Generic;


namespace TS_Clustering
{
    class HierarchicalClustering
    {
        private int lBound;
        private int numS;
        private int numT;
        private double[,] score;
        private int uBound;


        public HierarchicalClustering(int numS, int numT, int lBound, int uBound, double[,] score)
        {
            this.numS = numS;
            this.numT = numT;
            this.lBound = lBound;
            this.uBound = uBound;
            this.score = score;
        }

        public List<Cluster> doClustering(List<Cluster> oriClusters)
        {
            List<Cluster> finalClusters = new List<Cluster>();
            finalClusters = oriClusters;
            while (finalClusters.Count > numS)
            {
                double min = double.MaxValue;
                int indexA = 0;
                int indexB = 0;
                for(int i= 0; i < finalClusters.Count; i++)
                {
                    for(int j = 0; j < finalClusters.Count; j++) 
                    {
                        if(i!= j)
                        {
                            Cluster clusterA = finalClusters[i];
                            Cluster clusterB = finalClusters[j];
                            double tempDis = calculateDistance(clusterA, clusterB);
                            if (tempDis < min)
                            {
                                min = tempDis;
                                indexA = i;
                                indexB = j;
                            }
                        }
                    }
                }
                //find the best CLusterA and ClusterB ,merge
                finalClusters = mergeClusters(finalClusters, indexA, indexB);
            }
            while(!balancedClusters(finalClusters));
            return finalClusters;
        }

        private bool balancedClusters(List<Cluster> finalClusters)
        {
            int maxIndex = -1;
            int minIndex = -1;
            int max = 0;
            int min = numT;
            for (int i = 0; i < numS;i++)
            {
                if (finalClusters[i].Num > max)
                {
                    max = finalClusters[i].Num;
                    maxIndex = i;
                }
                if(finalClusters[i].Num < min)
                {
                    min = finalClusters[i].Num;
                    minIndex = i;
                }
            }
            if (max > uBound || min < lBound)
            {
                finalClusters[minIndex].Add(finalClusters[maxIndex].removeElement(0));
                return false;
            }
            else
            {
                return true;
            }
        }

        private List<Cluster> mergeClusters(List<Cluster> finalClusters, int indexA, int indexB)
        {
            if (indexA != indexB)
            {
                Cluster clusterA = finalClusters[indexA];
                Cluster clusterB = finalClusters[indexB];
                while(clusterB.Num > 0)
                {
                    Element temF = clusterB.removeElement(0);
                    clusterA.Add(temF);
                }
                finalClusters.RemoveAt(indexB);
            }
            return finalClusters;

        }

        private double calculateDistance(Cluster clusterA, Cluster clusterB)
        {
            double distance = 0;
            for(int m = 0; m < clusterA.Num; m++)
            {
                for (int n=0;n<clusterB.Num; n++)
                {
                    distance += score[clusterA.getElement(m).ID, clusterB.getElement(n).ID];
                }
            }
            return distance;            
        }

        private double getAverageDistance(Cluster clusterA, Cluster clusterB)           //get the average distance between two clusters
        {
            double avgDist = 0;
            int count = 0;
            for (int m = 0; m < clusterA.Num; m++)
            {
                for (int n = 0; n < clusterB.Num; n++)
                {
                    avgDist += score[clusterA.getElement(m).ID, clusterB.getElement(n).ID];
                    count++;      
                }
            }
            return count > 0? avgDist / count : 0;
        }

        private double getMinDistance(Cluster clusterA, Cluster clusterB)           //get the mininal distance between two clusters
        {
            double minDist = double.MaxValue;

            for (int m = 0; m < clusterA.Num; m++)
            {
                for (int n = 0; n < clusterB.Num; n++)
                {
                    if (score[clusterA.getElement(m).ID, clusterB.getElement(n).ID] < minDist)
                        minDist = score[clusterA.getElement(m).ID, clusterB.getElement(n).ID];
                }
            }
            return minDist;
        }

        //以平均距离最小作为聚合准则
        internal List<Cluster> doRHieClustering(List<Cluster> oriClusters)
        {
            List<Cluster> finalClusters = new List<Cluster>();
            finalClusters = oriClusters;
            while (finalClusters.Count > numS)
            {
                double min = double.MaxValue;
                int indexA = 0;
                int indexB = 0;
                for (int i = 0; i < finalClusters.Count; i++)
                {
                    for (int j = numS; j < finalClusters.Count; j++)
                    {
                        if (i != j)
                        {
                            Cluster clusterA = finalClusters[i];
                            Cluster clusterB = finalClusters[j];
                            double tempDis = getAverageDistance(clusterA, clusterB);              //calculateDistance(clusterA, clusterB);  BY 06/12/2016
                            if (tempDis < min)
                            {
                                min = tempDis;
                                indexA = i;
                                indexB = j;
                            }
                        }
                    }
                }
                //find the best CLusterA and ClusterB ,merge
                finalClusters = mergeClusters(finalClusters, indexA, indexB);
            }
            while (!balancedClusters(finalClusters)) ;
            return finalClusters;
        }
    }
}
