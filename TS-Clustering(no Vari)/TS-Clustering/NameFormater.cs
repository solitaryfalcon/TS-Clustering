using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS_Clustering
{
    class NameFormater
    {
        public ConcurrentDictionary<String,int> nameDic;
        int index;
        string[] elementName;
        public NameFormater(int elementNum)        {
            index = 0;//记录元素名的序号
            elementName = new string[elementNum];//记录元素名字
            nameDic = new ConcurrentDictionary<string, int>();
        }
        public int indexName(string name)
        {
            if (nameDic.ContainsKey(name))
            {
                return nameDic[name];
            }
            else
            {
                elementName[index] = name;
                nameDic.TryAdd(name, index);
                return index++;
            }
        }
        public string getName(int index)
        {
            return elementName[index];
        }

    }
}
