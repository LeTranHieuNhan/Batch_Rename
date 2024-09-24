using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;

namespace Rules
{
    public class Replace : IRule
    {
        public string Name => "Replace";
      
        public string Rename(string filename)
        {
            string result = filename;
            try
            {
                result = result.Replace("-", " ");
                result = result.Replace("_", " ");
            }
            catch (ArgumentNullException)
            {
                result = "Replace Rule's properties are null!";
            }

            return result;
        }

        //read ruleInfo to get ReplaceWord and SpecialWord
        public IRule Parse(string ruleInfo)
        {
            return this;
        }

        public void ResetCount()
        {
            
        }
    }
}
