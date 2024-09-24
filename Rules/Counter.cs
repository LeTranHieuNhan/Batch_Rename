using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Contract;

namespace Rules
{
    public class Counter : IRule
    {
        public string Name => "Counter";
        public int FileCount = 0;

        public string Rename(string origin)
        {
            string result = "";
            int temp = FileCount + 1;

            if (origin.Contains("."))
            {
                int dot = origin.LastIndexOf(".");
                string extension = origin.Substring(dot);
                string filename = origin.Substring(0, dot);
                result = filename + " (" + temp + ")" + extension;
            }
            else
            {
                result = origin + " (" + temp + ")";
            }
            FileCount++;
            return result;
        }

        //read ruleInfo to get Prefix
        public IRule Parse(string ruleInfo)
        {
            //FileCount = 0;
            return this;
        }

        public void ResetCount()
        {
            FileCount = 0;
        }
    }
}