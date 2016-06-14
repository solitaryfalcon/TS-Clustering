using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_Clustering
{
    class Cluster
    {
        private int num;
        private int numberLinks;
        private readonly int id;
        private Element first;

 

        public int ID
        {
            get
            {
                return id;
            }
        }

        internal Element First
        {
            get
            {
                return first;
            }

            set
            {
                first = value;
            }
        }

        public int Num
        {
            get
            {
                return num;
            }

            set
            {
                num = value;
            }
        }

        public int NumberLinks
        {
            get
            {
                return numberLinks;
            }

            set
            {
                numberLinks = value;
            }
        }

        public Cluster(int id)
        {
            this.id = id;
            this.Num = 0;
            this.NumberLinks = 0;
            this.first = null;
        }

        internal void Add(Element element)
        {
            element.ClusterID = id;
            if (first == null)
            {
                first = element;
            }
            else
            {
                first.Before = element;
                element.After = first;
                first = element;
            }
            NumberLinks += Num++;
        }
        public Element getElement(int index)
        {
            Element tempElement = First;
            for (int i = 0; i < index; i++)
            {
                tempElement = tempElement.After;
            }
            return tempElement;
        }
        internal Element removeElement(int index)
        {
            Element tempElement = getElement(index);

            return remove(tempElement);
        }
        internal Element remove(Element tempElement)
        {
            if (tempElement.ClusterID != this.id)
            {
                throw new Exception("no such element in this cluster");
            }
            if (tempElement.Before == null)
            {
                First = tempElement.After;
                if (First != null) First.Before = null;
            }
            else
            {
                tempElement.Before.After = tempElement.After;
                if (tempElement.After != null) tempElement.After.Before = tempElement.Before; 
            }
            tempElement.After = null;
            tempElement.Before = null;
            NumberLinks -= --Num;
            tempElement.ClusterID = -1;
            return tempElement;
        }
    }
}
