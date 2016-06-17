using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_Clustering
{
    class ClusterList
    {
        private double[] addPenalty, dropPenalty;

        private ArrayList[] candidateList;
        public ArrayList valList;
        public Cluster[] clusters;
        public Element[] elements;

        private double[,] addVal, moveVal, move_Sum_Val, move_Aver_Val;
        private double[,] tabuEnd;
        private double[,] score;

        private double[] sumValue;
        private double ObjVal, Obj_Sum_Val, Obj_Aver_Val;//target Value

        private const double DropPenVal = 0.5 * double.MaxValue;
        private const double AddPenVal = 0.5 * double.MaxValue;
        private const double AddInduceVal = -double.MaxValue / 5;
        private const double DropInduceVal = -double.MaxValue / 5;
        private int NumUBViolate, NumLBViolate, NumAllViolate;
        private double SmallTabuTenure, TabuTenure;

        public ClusterList()
        {
            clusters = new Cluster[Globals.CluNum + 1];
            elements = new Element[Globals.EleNum];
            //intialize clusters 
            for (int i = 0; i <= Globals.CluNum; i++)
            {
                clusters[i] = new Cluster(i);
            }
            //initialize elements and assign them to the dummy cluster
            for (int i = 0; i < Globals.EleNum; i++)
            {
                elements[i] = new Element(i);
                clusters[0].Add(elements[i]);
            }
            //initialize candidate list
            candidateList = new ArrayList[Globals.CluNum + 1];//there are CluNum Clusters and a dummy cluster 
            valList = new ArrayList();//stores the improvement存储目标值的变化
            for (int i = 0; i <= Globals.CluNum; i++)
            {
                candidateList[i] = new ArrayList();
            }
            //other values
            addVal = new double[Globals.EleNum, Globals.CluNum + 1];//注意其实有s个cluster' 和1个Dcluster
            sumValue = new double[Globals.CluNum + 1];

            moveVal = new double[Globals.EleNum, Globals.CluNum + 1];//注意其实有s个cluster' 和1个Dcluste
            move_Sum_Val = new double[Globals.EleNum, Globals.CluNum + 1];//注意其实有s个cluster' 和1个Dcluster
            move_Aver_Val = new double[Globals.EleNum, Globals.CluNum + 1];//注意其实有s个cluster' 和1个Dcluster

            tabuEnd = new double[Globals.EleNum, Globals.CluNum + 1];//TS中的数组
            dropPenalty = new double[Globals.CluNum + 1];
            addPenalty = new double[Globals.CluNum + 1];
        }

        internal  void  execTSPhase(BackgroundWorker bgw,Diversification diverMethod)
        {
            bool BestImprove = true;
            int LastImproveIter = 0;
            int Iter = 1;
            double BestObjVal = ObjVal;
            int LastBestIter = 0;

            bool localOpt = false;
            double BestMoveVal, Aspiration;
            Cluster clusterSBest = null;
            Element elementTBest = null;
            int T0, S0, SA;
            int[] bestClusterID = new int[Globals.EleNum];
            bestClusterID[0] = -1;//FLAG FOR IF RECORD 
            int TabuIter;
            while (Iter < Globals.MaxIter) //main TS loop
            {
                if (((Iter - LastImproveIter) < Globals.NoImproveLimit && !localOpt) ||BestImprove)
                {
                    #region improvement phase
                    if (BestImprove)
                    {
                        TabuIter = int.MaxValue;
                    }
                    else
                    {
                        TabuIter = Iter;
                    }
                    // Identify the best non-tabu move 
                    BestMoveVal = double.MaxValue;
                    elementTBest = null;
                    clusterSBest = null;
                    Aspiration = BestObjVal - ObjVal;

                    #region #4.80 find the best elementT and ClusterS
                    bool Drop0Feas, Drop1Feas;
                    for (int s = 1; s <= Globals.CluNum; s++)
                    {
                        foreach (Candidate candidate in candidateList[s])
                        {
                            T0 = candidate.EleID;
                            S0 = elements[T0].ClusterID;

                            if (clusters[S0].Num > 2)
                            {
                                if (tabuEnd[T0, S0] < TabuIter)
                                {
                                    if (s != S0)
                                    {
                                        double TrueMoveVal = moveVal[T0, s];
                                        double TestMoveVal;
                                        TestMoveVal = TrueMoveVal + addPenalty[s] + dropPenalty[S0];

                                        if (TestMoveVal < BestMoveVal)
                                        {
                                            Drop0Feas = false;
                                            Drop1Feas = false;
                                            //if element can be moved from S0
                                            if (clusters[S0].Num > Globals.LBound)
                                            {
                                                if (NumAllViolate == 0
                                                    ||
                                                    (NumAllViolate == 1 && clusters[S0].Num == Globals.UBound + 1))
                                                {
                                                    Drop0Feas = true;
                                                }
                                                else
                                                {
                                                    if (NumAllViolate <= 2)
                                                    {
                                                        if (NumLBViolate == 1)
                                                        {
                                                            if (NumAllViolate == 1 || clusters[S0].Num == Globals.UBound + 1)
                                                            {
                                                                Drop1Feas = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (tabuEnd[T0, s] < TabuIter
                                                || ((TrueMoveVal < Aspiration)
                                                         && ((Drop0Feas == true && clusters[s].Num < Globals.UBound)
                                                                ||
                                                                (Drop1Feas == true && clusters[s].Num == Globals.LBound - 1)
                                                            )
                                                    )
                                                )
                                            {
                                                BestMoveVal = TestMoveVal;
                                                elementTBest = elements[T0];
                                                clusterSBest = clusters[s];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (elementTBest == null)
                    {//no candidates selected
                        //在目前的候选者集中找不到可用的陷入局部最优
                        localOpt = true;
                         Util.WriteLine("Searching falls into local optimum");
                        Iter += 50;
                        continue;
                    }
                    #endregion

                    #region #4.9 Move and Update Sum_Values
                    //source
                    T0 = elementTBest.ID;
                    S0 = elements[T0].ClusterID;
                    //target
                    SA = clusterSBest.ID;

                    elements[T0].MoveTo(this, SA);
                    sumValue[SA] += addVal[T0, SA];
                    sumValue[S0] -= addVal[T0, S0];
                    

                    double ValA = sumValue[SA];
                    double Val0 = sumValue[S0];

                    double FullVal0 = Val0;

                    double FullValA = ValA;

                    #region//Update Violates
                    if (clusters[S0].Num >= Globals.UBound)
                    {
                        NumUBViolate--;
                        NumAllViolate--;
                    }
                    else if (clusters[S0].Num < Globals.LBound)
                    {
                        NumLBViolate++;
                        NumAllViolate++;
                    }
                    if (clusters[SA].Num > Globals.UBound)
                    {
                        NumUBViolate++;
                        NumAllViolate++;
                    }
                    else if (clusters[SA].Num <= Globals.LBound)
                    {
                        NumLBViolate--;
                        NumAllViolate--;
                    }
                    #endregion

                    double DeltaVal = moveVal[T0, SA];
                    double Delta_Sum_Val = move_Sum_Val[T0, SA];
                    double Delta_Aver_Val = move_Aver_Val[T0, SA];       
                    
                    ObjVal += DeltaVal;
                    Obj_Sum_Val += Delta_Sum_Val;
                    Obj_Aver_Val += Delta_Aver_Val;

                    //// Store new Best solution if solution improves
                    if (NumAllViolate == 0)
                    {
                        if (DeltaVal < 0)
                        {
                            if (ObjVal <= BestObjVal)
                            {
                                LastImproveIter = Iter;
                                BestImprove = true;
                                BestObjVal = ObjVal;
                            }
                            else
                            {
                                BestImprove = false;
                            }
                        }
                        else if (BestImprove == true)
                        {
                            //store the best solution
                            Element tempElement;
                            for (int t = 0; t < Globals.EleNum; t++)
                            {
                                tempElement = elements[t];
                                bestClusterID[t] = tempElement.ClusterID;
                            }
                            bestClusterID[T0] = S0;

                            BestImprove = false;
                            LastBestIter = Iter - 1;
                        }
                    }
                    #endregion

                    #region #4.10 Compute New AddVal and MoveVal quantities for Next Tabu Iteration
                    //Update All AddVal and Candidates
                    for (int s2 = 1; s2 <= Globals.CluNum; s2++)
                    {
                        Element elementT2 = clusters[s2].First;
                        while (elementT2 != null)
                        {
                            addVal[elementT2.ID, S0] -= score[elementT2.ID, T0];
                            addVal[elementT2.ID, SA] += score[elementT2.ID, T0];
                            elementT2 = elementT2.After;
                        }
                    }
                    int[] clusternum = new int[Globals.CluNum];
                    for (int s = 1; s <= Globals.CluNum; s++)
                    {                        
                        clusternum[s - 1] = clusters[s].Num;
                        candidateList[s].Clear();
                    }
                    for (int t2 = 0; t2 < Globals.EleNum; t2++)
                    {
                        int s2 = elements[t2].ClusterID;
                        int Link2 = clusters[s2].NumberLinks;
                        int NewLink2 = Link2 - (clusters[s2].Num - 1);
                        double Val2 = sumValue[s2];
                        double FullVal2 = Val2 / Link2;

                        double[] tempfullVal = new double[Globals.CluNum];
                        double tempfullTotal = 0;
                        for (int s = 1; s <= Globals.CluNum; s++)
                        {

                            tempfullVal[s - 1] = addVal[t2, s] / clusternum[s - 1];
                            tempfullTotal += tempfullVal[s - 1];
                        }
                        tempfullTotal = tempfullTotal / Globals.CluNum;       // tempfullTotal / (Globals.CluNum - 1); BY 06/12/2016
                        for (int s = 1; s <= Globals.CluNum; s++)
                        {
                            if (s != s2)
                            {
                                if (!isCandidateValid(elements[t2], s))
                                    continue;

                                if (tempfullVal[s - 1] < tempfullTotal)
                                {
                                    candidateList[s].Add(new Candidate(t2, s));
                                    
                                    int Link1 = clusters[s].NumberLinks;
                                    int NewLink1 = Link1 + clusters[s].Num;
                                    double Val1 = sumValue[s];
                                    double FullVal1 = Val1 / Link1;


                                    double NewFullVal1 = (Val1 + addVal[t2, s]) / NewLink1;
                                    double DeltaAdd1 = NewFullVal1 - FullVal1;

                                    double NewFullVal2 = (Val2 - addVal[t2, s2]) / NewLink2;
                                    double DeltaDrop2 = NewFullVal2 - FullVal2;

                                    move_Aver_Val[t2, s] = DeltaAdd1 + DeltaDrop2;
                                    move_Sum_Val[t2, s] = addVal[t2, s] - addVal[t2, s2];
                                    
                                    moveVal[t2, s] = Globals.A1 * move_Sum_Val[t2, s] + Globals.A2 * move_Aver_Val[t2, s] ;                                    
                                }
                            }
                        }
                    }
                    //log 
                    Util.WriteLine("#Success in Iter No." + Iter.ToString());
                    Util.WriteLine("#Move Element(" + T0 + ") from Cluster(" + S0 + ") to Cluster(" + SA + ")" + "  MoveVal = " + DeltaVal.ToString("0.0000"));
                    Util.WriteLine("#Now ObjVal = " + ObjVal.ToString("0.0000"));

                    valList.Add(new double[3] { Obj_Sum_Val, Obj_Aver_Val, ObjVal });
                    #endregion

                    #region #4.11 Update Penalties and Inducements for sA and s0
                    if (clusters[SA].Num >= Globals.UBound)
                    {
                        int AddViolation1 = 1 + clusters[SA].Num - Globals.UBound;
                        addPenalty[SA] = AddPenVal * AddViolation1;
                        if (clusters[SA].Num > Globals.UBound)
                        {
                            int AddViolation0 = clusters[SA].Num - Globals.UBound;
                            dropPenalty[SA] = DropInduceVal * AddViolation0;
                        }
                        else
                        {
                            dropPenalty[SA] = 0;
                        }
                    }
                    else if (clusters[SA].Num <= Globals.LBound)
                    {
                        int DropViolation1 = 1 + Globals.LBound - clusters[SA].Num;
                        dropPenalty[SA] = DropPenVal * DropViolation1;
                        if (clusters[SA].Num < Globals.LBound)
                        {
                            int DropViolation0 = Globals.LBound - clusters[SA].Num;
                            addPenalty[SA] = AddInduceVal * DropViolation0;
                        }
                        else
                        {
                            addPenalty[SA] = 0;
                        }
                    }
                    else
                    {
                        addPenalty[SA] = 0;
                        dropPenalty[SA] = 0;
                    }

                    if (clusters[S0].Num >= Globals.UBound)
                    {
                        int AddViolation1 = 1 + clusters[S0].Num - Globals.UBound;
                        addPenalty[S0] = AddPenVal * AddViolation1;
                        if (clusters[S0].Num > Globals.UBound)
                        {
                            int AddViolation0 = clusters[S0].Num - Globals.UBound;
                            dropPenalty[S0] = DropInduceVal * AddViolation0;
                        }
                        else
                        {
                            dropPenalty[S0] = 0;
                        }
                    }
                    else if (clusters[S0].Num <= Globals.LBound)
                    {
                        int DropViolation1 = 1 + Globals.LBound - clusters[S0].Num;
                        dropPenalty[S0] = DropPenVal * DropViolation1;
                        if (clusters[S0].Num < Globals.LBound)
                        {
                            int DropViolation0 = Globals.LBound - clusters[S0].Num;
                            addPenalty[S0] = AddInduceVal * DropViolation0;
                        }
                        else
                        {
                            addPenalty[S0] = 0;
                        }
                    }
                    else
                    {
                        addPenalty[S0] = 0;
                        dropPenalty[S0] = 0;
                    }
                    tabuEnd[T0, SA] = Iter + TabuTenure;
                    tabuEnd[T0, S0] = Iter + SmallTabuTenure;
                    Iter++;
                    #endregion
                    #endregion
                }
                else
                {
                    if ((Iter - LastBestIter) < Globals.NoImproveLimit * Globals.NoImprovePhase)                       //actually we will use  Globals.NoImprovePhase = 1, BY 06/12/2016
                    {//when the last noImprovateLimit Iter didn't find a better result, goto diversification
                        #region diversification phase
                        Util.WriteLine("\r\nImprovementPhase ended.Best solution in Iter No." + (LastBestIter).ToString());
                        Util.WriteLine("Start the diversification phase.\r\n");
                        bgw.ReportProgress(50 * Iter / Globals.MaxIter + 30, "Diversification..\n");
                        switch (diverMethod)
                        {
                            //balance the Cluster to get a new solution.
                            case Diversification.BALANCE:
                                balanceCluster();
                                break;
                                //get a new solution by Hierarchical
                            case Diversification.RERHIE:
                                reHierCluster();
                                break;
                        }


                        initTSPhase();

                        bgw.ReportProgress(50 * Iter / Globals.MaxIter + 30, "Executing Tabu Search...");
                        BestImprove = false;
                        localOpt = false;
                        LastImproveIter = Iter;
                        #endregion
                    }
                    else
                    {// 2 more improvementphases but didn't find best, then end
                        Iter = Globals.MaxIter;
                    }
                }
            }
            if (bestClusterID[0] == -1)
            {
                for (int t = 0; t < Globals.EleNum; t++)
                {
                    bestClusterID[t] = elements[t].ClusterID;
                }
                LastBestIter = Iter - 1;
                
            }
            Util.WriteLine("\r\n TS completed!Best solution in Iter No." + (LastBestIter).ToString());
            //roll back to the best solution
            for (int i = 0; i < Globals.EleNum; i++)
            {
                Element tempelement = elements[i];
                tempelement.MoveTo(this, bestClusterID[i]);
            }
            //recalculate All the values:Val[s] ,ObjVal,AddVal,MoveVal   etc.
            bgw.ReportProgress(80, "TS-Clustering Done.");
        }

        internal void exportDistribution(StreamWriter sw)
        {
            throw new NotImplementedException();
        }

        internal void exportResult(StreamWriter sw,double[,] data)
        {
            score =(double[,]) data.Clone();//不可逆，谨慎
            AllValuesCalculator();
            sw.WriteLine("Best solution in the TS-Clustering.");
            sw.WriteLine("CID\t p\t w\t\t q\t\t w/q\t\t ");
            double tempAver = 0;
            double tempSum = 0;
            for (int i = 1; i <= Globals.CluNum; i++)
            {
                double weight = sumValue[i] / clusters[i].NumberLinks;
                sw.WriteLine(i.ToString("") + " \t " + clusters[i].Num.ToString("") + "\t" + sumValue[i].ToString("0.00") + "\t\t" + clusters[i].NumberLinks.ToString("") + "\t\t" + weight.ToString("0.0000") + "\t\t"  + '\n');
                tempAver += weight;
                tempSum += sumValue[i];
            }
            sw.WriteLine("Best ObjValue = " + Globals.A1.ToString() + "×" + tempSum.ToString("0.0000") + " + " + Globals.A2.ToString() + "×" + tempAver.ToString("0.0000") + " = " + (tempSum * Globals.A1 + tempAver * Globals.A2 ).ToString("0.0000"));
            sw.WriteLine(ObjVal.ToString("0.00000"));
        }

        private void balanceCluster()
        {
            Random ra = new Random();
            int basenum = Globals.EleNum / Globals.CluNum;
            int[] temp = new int[Globals.CluNum+1];
            for (int i = 1; i <= Globals.CluNum; i++)
            {
                Cluster tempCluster = clusters[i] ;
                temp[i] = tempCluster.Num - basenum;
            }
            //move extra element to dummy Cluster
            for (int i = 1; i <= Globals.CluNum; i++)
            {
                Cluster curCluster = clusters[i];
                while (temp[i] > 0)
                {
                    int randomOrder = ra.Next(1, curCluster.Num + 1);
                    Element curElement;
                    curElement = curCluster.First;
                    for (int j = 0; j < randomOrder - 1; j++)
                    {
                        curElement = curElement.After;
                    }
                    curElement.MoveTo(this, 0);
                    temp[i]--;
                }
            }
            //move elements from dummyCluster to the cluster in short 
            Cluster dumCluster = clusters[0];
            Element dumElement = null;
            for (int i = 0; i < Globals.CluNum; i++)
            {
                //Cluster curCluster = getClusterbyID(i + 1);
                while (temp[i] < 0)
                {
                    dumElement = dumCluster.First;
                    dumElement.MoveTo(this, i + 1);
                    temp[i]++;
                }
            }
            dumElement = dumCluster.First;
            while (dumElement != null)
            {
                dumElement.MoveTo(this, ra.Next(1, Globals.CluNum + 1));
                dumElement = dumCluster.First;
            }
        }

        private void reHierCluster()
        {
            //使用层次聚类产生新解
            List<Cluster> oriClusters = new List<Cluster>();
            //create K restricted Clusters
            for (int i = 1;i<= Globals.CluNum; i++){
                Cluster tempCluster = new Cluster(0);
                Element temElement = clusters[i].removeElement(0);
                tempCluster.Add(temElement);
                oriClusters.Add(tempCluster);
            }
            //assign the rest
            for (int i = 1; i <= Globals.CluNum; i++)
            {
                while (clusters[i].Num > 0)
                {
                    Cluster tempCluster = new Cluster(0);
                    Element temElement = clusters[i].removeElement(0);
                    tempCluster.Add(temElement);
                    oriClusters.Add(tempCluster);
                }
            }
            HierarchicalClustering hc = new HierarchicalClustering(Globals.CluNum, Globals.EleNum, Globals.LBound, Globals.UBound, this.score);
            List<Cluster> result = hc.doRHieClustering(oriClusters);
            for (int i = 1; i <= Globals.CluNum; i++)
            {
                Cluster tarCluster = clusters[i];
                while (result[i - 1].Num > 0)
                {
                    tarCluster.Add(result[i - 1].removeElement(0));
                }
            }

        }
        internal void doInitial(Initialization initMethod)
        {
            //compute the average distances
            float avgDist = computeAverageDistance();                    //can be adjusted upon the problems
            Globals.CutOff = avgDist * Globals.CutOffParam;
            Globals.Average = avgDist;                                      //will be used for building candidate list

            switch (initMethod)
            {
                case Initialization.NORMAL:
                    doInitSolution();
                    break;
                case Initialization.HIERARCHICHAL:
                    doHieInitSolution();
                    break;
                case Initialization.RHIE:
                    doRHieInitSolution();
                    break;
            }
        }

        internal void initTSPhase()
        {
            AllValuesCalculator();
            valList.Add(new double[3] { Obj_Sum_Val, Obj_Aver_Val, ObjVal });

            initCandidateList();

            #region //here Prepare info(MoveValue etc.) for s1 to evaluate adding some element t2 to it.
            //value pairs (t2,s1) in CandidateList[s1]
            for (int s1 = 1; s1 <= Globals.CluNum; s1++)
            {
                int Link1 = clusters[s1].NumberLinks;
                int NewLink1 = Link1 + clusters[s1].Num;
                double Val1 = sumValue[s1];
                double FullVal1 = Val1 / Link1;
                foreach (Candidate candidate in candidateList[s1])
                {
                    int t2 = candidate.EleID;
                    int s2 = elements[t2].ClusterID;
                    if (s1 == s2)
                    {
                        moveVal[t2, s1] = 0;
                        move_Sum_Val[t2, s1] = 0;
                        move_Aver_Val[t2, s1] = 0;
                    }
                    else if (clusters[s2].Num == 2)//set penalty if only has 2 elements
                    {
                        moveVal[t2, s1] = DropPenVal;
                        move_Sum_Val[t2, s1] = DropPenVal;
                        move_Aver_Val[t2, s1] = DropPenVal;
                    }
                    else// set new Value
                    {
                        int Link2 = clusters[s2].NumberLinks;
                        int NewLink2 = Link2 - (clusters[s2].Num - 1);
                        double Val2 = sumValue[s2];
                        double FullVal2 = Val2 / Link2;

                        double NewFullVal1 = (Val1 + addVal[t2, s1]) / NewLink1;
                        double DeltaAdd1 = NewFullVal1 - FullVal1;

                        double NewFullVal2 = (Val2 - addVal[t2, s2]) / NewLink2;
                        double DeltaDrop2 = NewFullVal2 - FullVal2;

                        move_Aver_Val[t2, s1] = DeltaAdd1 + DeltaDrop2;
                        move_Sum_Val[t2, s1] = addVal[t2, s1] - addVal[t2, s2];
                        moveVal[t2, s1] = Globals.A1 * move_Sum_Val[t2, s1] + Globals.A2 * move_Aver_Val[t2, s1];
                    }
                }
            }
            #endregion

            #region 4.7# Prepare Penalties and Inducements for Tabu Search
            NumUBViolate = 0;
            NumLBViolate = 0;

            for (int s = 1; s <= Globals.CluNum; s++)
            {
                if (clusters[s].Num >= Globals.UBound)
                {
                    int AddViolation1 = 1 + clusters[s].Num - Globals.UBound;
                    //////////////////NumUBViolate??????????????????????????????????????????????????????????????????????????
                    //NumUBViolate += AddViolation1;
                    addPenalty[s] = AddPenVal * AddViolation1;
                    if (clusters[s].Num > Globals.UBound)
                    {
                        int AddViolation0 = clusters[s].Num - Globals.UBound;
                        NumUBViolate += AddViolation0;
                        dropPenalty[s] = DropInduceVal * AddViolation0;
                    }
                    else
                    {
                        dropPenalty[s] = 0;
                    }
                }
                else if (clusters[s].Num <= Globals.LBound)
                {
                    int DropViolation1 = 1 + Globals.LBound - clusters[s].Num;
                    dropPenalty[s] = DropPenVal * DropViolation1;
                    if (clusters[s].Num < Globals.LBound)
                    {
                        int DropViolation0 = Globals.LBound - clusters[s].Num;
                        NumLBViolate += DropViolation0;
                        addPenalty[s] = AddInduceVal * DropViolation0;
                    }
                    else
                    {
                        addPenalty[s] = 0;
                    }
                }
                else
                {
                    addPenalty[s] = 0;
                    dropPenalty[s] = 0;
                }
            }
            NumAllViolate = NumLBViolate + NumUBViolate;
            #endregion

            #region 4.8# TabuTenure for TS

            SmallTabuTenure = MathHelper.Min(3, Globals.CluNum / 3);
            SmallTabuTenure = MathHelper.Max(SmallTabuTenure, 1);
            ///////????????????
            double TabuTenureLim = 1 + 0.4 * (Globals.EleNum - SmallTabuTenure) * (Globals.CluNum - 1);
            double TabuTenurePref = 1 + 0.25 * (Globals.EleNum - SmallTabuTenure) * (Globals.CluNum - 1);

            TabuTenure = MathHelper.Max(TabuTenurePref, 15);
            TabuTenure = MathHelper.Min(TabuTenure, TabuTenureLim);

            #endregion
        }

        private bool isCandidateValid(Element fromElement, int toCusterID)
        {                                                                       //we don't want to add an element that is too far away from the toCluster, BY 06/12/2016
            bool retValue = true;

            for (int i = 0; i < clusters[toCusterID].Num; i++)
            {
                Element elem = clusters[toCusterID].getElement(i);

                float tmpDist = (float)score[fromElement.ID, elem.ID];

                if (tmpDist > Globals.RelaxFactor * Globals.Average)
                {
                    retValue = false;
                    break;
                }
            }

            return retValue;
        }


        private void initCandidateList()
        {
            for (int s = 1; s <= Globals.CluNum; s++)
            {
                candidateList[s].Clear();
            }
            for (int t2 = 0; t2 < Globals.EleNum; t2++)
            {
                Element elementT2 = elements[t2];
                int s2 = elementT2.ClusterID;
                Cluster clusterS2 = clusters[s2];
                double Val2 = sumValue[s2];
                double FullVal2 = Val2;

                double[] tempfullVal = new double[Globals.CluNum];
                double tempfullTotal = 0;
                for (int s = 1; s <= Globals.CluNum; s++)
                {

                    tempfullVal[s - 1] = addVal[t2, s] / clusters[s].Num;
                    tempfullTotal += tempfullVal[s - 1];
                }
                tempfullTotal = tempfullTotal / Globals.CluNum;         // (Globals.CluNum - 1);    we need a stronger constraint, BY 06/12/2016
                for (int s = 1; s <= Globals.CluNum; s++)
                {
                    if (s != s2)
                    {
                        if (!isCandidateValid(elementT2, s))
                            continue;                                       //should not be a candidate for the cluster

                        if (tempfullVal[s - 1] <= tempfullTotal)
                        {
                            candidateList[s].Add(new Candidate(t2, s));
                        }
                    }
                }
            }
        }


        
        private double VariValCalculator(Cluster c)
        {
            //计算当前VariVal，基于SumVal，需保证SumVal的更新
            double fullvalue = sumValue[c.ID] / c.NumberLinks;
            double varivalue = 0;
            Element ele1, ele2;
            ele1 = c.First;
            while (ele1 != null)
            {
                ele2 = ele1.After;
                while (ele2 != null)
                {
                    double temval = score[ele1.ID, ele2.ID];
                    varivalue += (temval - fullvalue) * (temval - fullvalue);
                    ele2 = ele2.After;
                }
                ele1 = ele1.After;
            }
            return varivalue / c.NumberLinks;

        }

        private void SumValsCalculator()
        {
            for (int s = 0; s <= Globals.CluNum; s++)
            {
                Cluster curCluster = clusters[s];
                double ValS = 0;
                Element t1 = curCluster.First;
                Element t2 = null;
                while (t1 != null)
                {
                    t2 = t1.After;
                    while (t2 != null)
                    {
                        ValS += score[t1.ID, t2.ID];
                        t2 = t2.After;
                    }
                    t1 = t1.After;
                }
                sumValue[s] = ValS;
            }
        }

        private void AddValsCalculator()
        {
            //计算所有的AddVal
            //reset
            for (int t = 0; t < Globals.EleNum; t++)
            {
                for (int s = 0; s <= Globals.CluNum; s++)
                {
                    addVal[t, s] = 0;
                }
            }
            //calculate
            for (int t = 0; t < Globals.EleNum; t++)
            {
                int sA = elements[t].ClusterID;
                for (int t0 = 0; t0 < Globals.EleNum; t0++)
                {
                    addVal[t0, sA] += score[t0, t];
                }
            }
        }

        private void AllValuesCalculator()
        {
            AddValsCalculator();//recalculate all addVal[T,S]
            SumValsCalculator();//recalculate all sumVal[S]
            //reclaculate all variVal[S]; precondition:sumVal[S] updated
            
            //recalculate all the target Values
            ObjVal = 0;
            
            Obj_Aver_Val = 0;
            Obj_Sum_Val = 0;
            for (int i = 1; i <= Globals.CluNum; i++)
            {
                Obj_Sum_Val +=  sumValue[i];
                Obj_Aver_Val += sumValue[i] / clusters[i].NumberLinks;
            
            }
            ObjVal = Globals.A1 * Obj_Sum_Val + Globals.A2 * Obj_Aver_Val;
        }

        private void doInitSolution()
        {
            //by greedy strategy
            //论文中最初的初始解生成策略
            //4.3 First create 2-element clusters.
            //calculate the CheckBound
            Cluster DCluster = clusters[0];
            int CheckBound = Globals.EleNum / Globals.CluNum;
            if (CheckBound >= 7)
                CheckBound = CheckBound - 2;
            CheckBound = MathHelper.Min(CheckBound, 10);//每个cluster中最多有10个元素

            Element elementT0Best = null, elementT1Best = null, elementT2Best = null, elementT3Best = null;
            Element elementT0 = null, elementT1 = null, elementT2 = null, elementT3 = null;
            #region 填装所有CLuster至Checkbound
            for (int s = 1; s <= Globals.CluNum; s++)
            {
                double BestSumScore = double.MaxValue;
                elementT0 = DCluster.First;
                #region 当此次循环结束时当前Cluster所有点的最优组合（t1,t2,t3,t0）就已找到。
                while (elementT0 != null)
                {
                    double Best1 = double.MaxValue;
                    double Best2 = double.MaxValue;
                    double Best3 = double.MaxValue;
                    elementT1 = null;
                    elementT2 = null;
                    elementT3 = null;
                    // for each t0, find the three best scores Score(t0,t1), Score(t0,t2) and Score(t0,t3). 
                    Element elementTCheck = DCluster.First;
                    #region 当这个while循环结束后对于固定t0已经找到了最优的组合(t0,t1)(t0,t2)(t0,t3)
                    while (elementTCheck != null)
                    {
                        if (elementTCheck != elementT0)
                        {
                            double TrialVal = score[elementT0.ID, elementTCheck.ID];
                            if (TrialVal < Best3)
                            {
                                if (TrialVal < Best2)
                                {
                                    Best3 = Best2;
                                    elementT3 = elementT2;
                                    if (TrialVal < Best1)
                                    {
                                        Best2 = Best1;
                                        elementT2 = elementT1;
                                        Best1 = TrialVal;
                                        elementT1 = elementTCheck;
                                    }
                                    else
                                    {
                                        Best2 = TrialVal;
                                        elementT2 = elementTCheck;
                                    }
                                }
                                else
                                {
                                    Best3 = TrialVal;
                                    elementT3 = elementTCheck;
                                }
                            }
                        }
                        elementTCheck = elementTCheck.After;
                    }
                    #endregion

                    if (elementT3 == null)
                    {
                        Best3 = 0;
                    }
                    if (elementT2 == null)
                    {
                        Best2 = 0;
                    }
                    if (elementT1 == null)
                    {
                        Best1 = 0;
                    }

                    double SumScore = Best1 + Best2 + Best3;// Score(t0,t1), Score(t0,t2) ,Score(t0,t3)
                    if (SumScore < BestSumScore)
                    {//若此时的SumScore小于之前的最优和
                        BestSumScore = SumScore; //则用新的结果更新最优和
                        elementT0Best = elementT0;
                        elementT1Best = elementT1;
                        elementT2Best = elementT2;
                        elementT3Best = elementT3;//记录t0的最优组合
                    }
                    elementT0 = elementT0.After;
                }
                #endregion
                double Try1 = double.MaxValue;
                double Try2 = double.MaxValue;
                double Try3 = double.MaxValue;
                //only has t0 left
                if (elementT1Best != null && elementT2Best != null)
                {
                    Try1 = score[elementT0Best.ID, elementT1Best.ID] + score[elementT0Best.ID, elementT2Best.ID] + score[elementT1Best.ID, elementT2Best.ID];
                }
                if (elementT1Best != null && elementT3Best != null)
                {
                    Try2 = score[elementT0Best.ID, elementT1Best.ID] + score[elementT0Best.ID, elementT3Best.ID] + score[elementT1Best.ID, elementT3Best.ID];
                }
                if (elementT2Best != null && elementT3Best != null)
                {
                    Try3 = score[elementT0Best.ID, elementT2Best.ID] + score[elementT0Best.ID, elementT3Best.ID] + score[elementT2Best.ID, elementT3Best.ID];
                }
                #region 找出当前t0下最优的t1和t2
                elementT0 = elementT0Best;
                if (Try1 <= Try2 && Try1 <= Try3)//try1 is the best
                {
                    elementT1 = elementT1Best;
                    elementT2 = elementT2Best;
                }
                else
                {
                    if (Try2 <= Try1 && Try2 <= Try3)//try2 is the best
                    {
                        elementT1 = elementT1Best;
                        elementT2 = elementT3Best;
                    }
                    else
                    {
                        if (Try3 <= Try1 && Try3 <= Try2)//try3 is the best
                        {
                            elementT1 = elementT2Best;
                            elementT2 = elementT3Best;
                        }
                    }
                }
                // Now the pair t0,t1 creates the next 2-element Cluster(s)
                // Remove t0 and t1 from the dummy Cluster(0)
                #endregion

                #region 找出T1、T2中相对更优的T1
                if (score[elementT0.ID, elementT1.ID] > score[elementT0.ID, elementT2.ID])
                {
                    elementT1 = elementT2;
                }
                #endregion

                //对于当前s构造仅有两个元素的Cluster[s]
                // Now the pair t0,t1 creates the next 2-element Cluster(s)
                // Remove t0 and t1 from the dummy Cluster(0)
                elementT0.MoveTo(this, s);
                elementT1.MoveTo(this, s);

                //更新当前s下的addVal
                Element t = DCluster.First;
                while (t != null)
                {
                    addVal[t.ID, s] = score[t.ID, elementT0.ID] + score[t.ID, elementT1.ID];
                    t = t.After;
                }

                #region Fill rest of Cluster s up to size CheckBound
                //该循环将当前Cluster填装至CheckBound
                Cluster sCluster = clusters[s];
                while (sCluster.Num < CheckBound)
                {
                    double BestVal = double.MaxValue;
                    Element tBest = null;
                    Element tCheck = DCluster.First;
                    double TrialVal;
                    while (tCheck != null)
                    {
                        TrialVal = addVal[tCheck.ID, s];
                        if (TrialVal < BestVal)
                        {
                            BestVal = TrialVal;
                            tBest = tCheck;
                        }
                        tCheck = tCheck.After;
                    }
                    // Now add tBest to Cluster(s) by first dropping from Cluster(s0) 
                    tBest.MoveTo(this, s);

                    // Now update AddVal array.
                    t = DCluster.First;
                    while (t != null)
                    {
                        addVal[t.ID, s] += score[t.ID, tBest.ID];
                        t = t.After;
                    }
                }
                #endregion
            }
            #endregion
            //所有Cluster填装完毕
            //如果CheckBound大于2 则将其他元素回归
            #region 回归大于2的元素
            if (CheckBound > 2)
            {
                for (int s = 1; s <= Globals.CluNum; s++)
                {
                    Cluster sCluster = clusters[s];
                    for (int CheckSet = 0; CheckSet < CheckBound - 2; CheckSet++)
                    {
                        Element t = sCluster.First;
                        t.MoveTo(this, 0);
                    }
                }
            }
            #endregion
            #region 对仅有两个元素的Cluster遍历求得移动预期
            //Create arrays for each 2-element Cluster s
            for (int s = 1; s <= Globals.CluNum; s++)
            {
                Cluster sCluster = clusters[s];
                Element t0 = sCluster.First;
                Element t1 = t0.After;
                double sValue = score[t0.ID, t1.ID];
                sumValue[s] = sValue;

                ObjVal += sValue;

                sCluster.Num = 2;
                sCluster.NumberLinks = 1;

                for (int i = 0; i <= Globals.CluNum; i++)
                {
                    Cluster tempCluster = clusters[i];
                    Element tempElement = tempCluster.First;
                    while (tempElement != null)
                    {
                        addVal[tempElement.ID, s] = score[tempElement.ID, t0.ID] + score[tempElement.ID, t1.ID];
                        moveVal[tempElement.ID, s] = (sValue + addVal[tempElement.ID, s]) / 3.0 - sValue;
                        tempElement = tempElement.After;
                    }
                }
            }
            #endregion
            #region 完成初始化
            int tleft = Globals.EleNum - 2 * Globals.CluNum;
            int BndLeft = 0;
            BndLeft = (Globals.LBound - 2) * Globals.CluNum;
            int NumLeft = tleft;

            DCluster = clusters[0];
            #region 找到做小regret MinRegret 的element tBest和对应的cluster sBest
            // Each iteration assigns an element t0 of Cluster(0) to some Cluster(s) by Min Regret.
            for (int i = 0; i < tleft; i++)
            {
                if (BndLeft == NumLeft)
                {
                    CheckBound = Globals.LBound;
                }
                else
                {
                    CheckBound = Globals.UBound;
                }
                double MinRegret = double.MaxValue;
                Element tCheck = DCluster.First;
                Element tBest = tCheck; ;
                int sBest = 0;

                while (tCheck != null)
                {
                    double BestVal1 = double.MaxValue;
                    double BestVal2 = double.MaxValue;
                    int s1Best = 0;
                    int s2Best = 0;
                    for (int s = 1; s <= Globals.CluNum; s++)
                    {
                        Cluster curCluster = clusters[s];
                        if (curCluster.Num < CheckBound)
                        {
                            double TrialVal = moveVal[tCheck.ID, s];
                            if (TrialVal < BestVal2)
                            {
                                if (TrialVal < BestVal1)
                                {
                                    BestVal2 = BestVal1;
                                    s2Best = s1Best;
                                    BestVal1 = TrialVal;
                                    s1Best = s;//replace s2 with s1 ,s1 with s
                                }
                                else
                                {
                                    BestVal2 = TrialVal;
                                    s2Best = s;//replace s2with s
                                }
                            }
                        }
                    }//find the best 2 Cluster s1best and s2best

                    double Regret = BestVal1 - BestVal2;
                    if (Regret < MinRegret)
                    {
                        MinRegret = Regret;
                        tBest = tCheck;
                        sBest = s1Best;
                    }
                    tCheck = tCheck.After;
                }
                #endregion

                //MOve tBest from cLuster(0) to Cluster(sBest)
                tBest.MoveTo(this, sBest);
                sumValue[sBest] += addVal[tBest.ID, sBest];
                double DeltaVal = moveVal[tBest.ID, sBest];

                ObjVal += DeltaVal;

                Cluster Sa = clusters[sBest];
                double SAValue = sumValue[sBest];
                double PresentFullValueA = SAValue / Sa.NumberLinks;
                //update values
                for (int t = 0; t < Globals.EleNum; t++)
                {
                    Element elementT = elements[t];
                    addVal[t, sBest] += score[elementT.ID, tBest.ID];
                    double FullAddValA = ((SAValue + addVal[t, sBest]) / Sa.NumberLinks) - PresentFullValueA;
                    moveVal[t, sBest] = FullAddValA;
                }//update done
                // Moreover, we only move elements from the dummy cluster, so remaining 
                // MoveVal(t,sA) values are irrelevant until all elements are finally assigned.

                NumLeft = NumLeft - 1;
                if (Sa.Num <= Globals.LBound)
                {
                    BndLeft--;
                }
            }
            #endregion     
        }

        private void doHieInitSolution()
        {   //使用AGENES层次聚类 
            //Do Clustering by AGENES
            List<Cluster> oriClusters = new List<Cluster>();
            for (int i = 0; i < Globals.EleNum; i++)
            {
                Cluster tempCluster = new Cluster(0);
                Element temElement = clusters[0].removeElement(0);
                tempCluster.Add(temElement);
                oriClusters.Add(tempCluster);
            }
            HierarchicalClustering hc = new HierarchicalClustering(Globals.CluNum, Globals.EleNum, Globals.LBound, Globals.UBound, this.score);
            List<Cluster> result = hc.doClustering(oriClusters);
            for (int i = 1; i <= Globals.CluNum; i++)
            {
                Cluster tarCluster = clusters[i];
                while (result[i - 1].Num > 0)
                {
                    tarCluster.Add(result[i - 1].removeElement(0));
                }
            }
        }

        private float computeAverageDistance()             //we will use this to decide an element can be a seed or not
        {
            float avgDist = 0;
            int[] tempElemID = new int[clusters[0].Num];    //store the element IDs

            for (int i = 0; i < clusters[0].Num; i++)
            {
                tempElemID[i] = -1;
                
            }
            Element tempElem = clusters[0].First;
            int count = 0;
            while (tempElem != null)
            {
                tempElemID[count] = tempElem.ID;
                tempElem = tempElem.After;
                count++;
            }
            int distCount = 0;
            float dist = 0;
            for (int i = 0; i < count - 1; i++)
            {
                int fromID = tempElemID[i];
                if (fromID < 0)
                    continue;
                for (int j = i + 1; j < count; j++)
                {
                    int toID = tempElemID[j];
                    if (toID < 0)
                        continue;

                    dist += (float)score[fromID, toID];
                    distCount++;

                }
            }

            avgDist = distCount > 0 ? dist / distCount : 0;
            return avgDist;

        }

        private void doRHieInitSolution()
        {
            //使用层次聚类 但限定聚合条件
            List<Cluster> oriClusters = new List<Cluster>();
            //create K restricted Clusters
            Cluster tempCluster = new Cluster(0);
            Element temElement = clusters[0].removeElement(0);
            tempCluster.Add(temElement);
            oriClusters.Add(tempCluster);

            while (oriClusters.Count < Globals.CluNum)
            {
                double bestDis = 0;
                Element bestEle = null;
                temElement = clusters[0].First;
                while (temElement != null)
                {
                    double dis = 0;
                    bool valid = true;
                    for (int i = 0; i < oriClusters.Count; i++)
                    {
                        Element tarEle = oriClusters[i].First;
                        double tempDist = score[temElement.ID, tarEle.ID];
                        if (tempDist < Globals.CutOff)
                        {                                       //not a good candidate
                            valid = false;
                            break;
                        } 
                        dis += tempDist;
                    }
                    if (dis > bestDis && valid)
                    {
                        bestDis = dis;
                        bestEle = temElement;
                    }
                    temElement = temElement.After;
                }
                tempCluster = new Cluster(0);
                if (bestEle == null)
                {
                    throw new Exception("No valid element in constructing intial solution\n(by RHie with the Cutoff = "+Globals.CutOff+")");
                }
                clusters[0].remove(bestEle);
                tempCluster.Add(bestEle);
                oriClusters.Add(tempCluster);
            }

            //assign the rest
            while (clusters[0].Num > 0)
            {
                tempCluster = new Cluster(0);
                temElement = clusters[0].removeElement(0);
                tempCluster.Add(temElement);
                oriClusters.Add(tempCluster);
            }
            HierarchicalClustering hc = new HierarchicalClustering(Globals.CluNum, Globals.EleNum, Globals.LBound, Globals.UBound, this.score);
            List<Cluster> result = hc.doRHieClustering(oriClusters);
            for (int i = 1; i <= Globals.CluNum; i++)
            {
                Cluster tarCluster = clusters[i];
                while (result[i - 1].Num > 0)
                {
                    tarCluster.Add(result[i - 1].removeElement(0));
                }
            }
        }

        internal void importData(double[,] inputScore, double[,] normlizedScore)
        {
            this.score = (double[,])normlizedScore.Clone();
        }
        private Element getCentroid(Cluster c,bool ifInit)
        {
            if (ifInit)
            {
                AddValsCalculator();
            }
            Element restulE = null;
            double sumDis = double.MaxValue;
            Element temp = c.First;
            while (temp != null)
            {
                if (addVal[temp.ID, c.ID] < sumDis)
                {
                    sumDis = addVal[temp.ID, c.ID];
                    restulE = temp;
                }
                temp = temp.After;
            }
            return restulE;
        }
    }
}