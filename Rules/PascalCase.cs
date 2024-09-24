using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Contract;

namespace Rules
{
    public class PascalCase : IRule
    {
        public string Name => "PascalCase";

        public string Rename(string origin)
        {
            string result = "";

            StringBuilder builder = new StringBuilder();
            string[] split = Regex.Split(origin, @" +");
            _ = builder.Append("");


            foreach (string temp in split)
            {
                string name = temp.ToString().ToLower();
                builder.Append(name + " ");
            }
            result = builder.ToString();
            split = Regex.Split(result, @" +");


            builder = new StringBuilder();
            foreach (string temp in split)
            {
                if (temp != "")
                {
                    string name = temp[0].ToString().ToUpper() + temp.Substring(1);
                    builder.Append(name);
                }
            }

            result = builder.ToString();

            return result;
        }


        public IRule Parse(string ruleInfo)
        {
            return this;
        }

        public void ResetCount()
        {
            
        }
    }
}
