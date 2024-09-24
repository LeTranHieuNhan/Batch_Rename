using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;

namespace Rules
{
    public class OneSpaceOnly : IRule
    {
        public string Name => "OneSpaceOnly";


        public string Rename(string filename)
        {
            var words = filename.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string result = "";

            foreach (var word in words)
            {
                result += word + " ";
            }

            return result;
        }

        //return itself for incapsulation purpose
        public IRule Parse(string ruleInfo)
        {
            return this;
        }

        public void ResetCount()
        {
            
        }
    }
}
