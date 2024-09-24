using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;

namespace Rules
{
    public class AddPrefix : IRule
    {
        public string Name => "AddPrefix";

        public string Prefix { get; set; } = null;


        public string Rename(string filename)
        {
            string result = filename;
            try
            {
                if (Prefix == null)
                {
                    throw new ArgumentNullException();
                }

                string[] tokens = result.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                
                result = Prefix + " " + filename;
                
            }
            catch (ArgumentNullException)
            {
                result = "AddPrefixRule's properties are null!";
                
            }

            return result;
        }

        //read ruleInfo to get Prefix
        public IRule Parse(string ruleInfo)
        {
            string temp = Name;
            string prefix = ruleInfo.Replace(temp, string.Empty);
            if (prefix.Length >= 1)
            {
                prefix = prefix.Remove(0, 1);
            }
            Debug.WriteLine(prefix);
            Prefix = prefix;

            return this;
        }

        public void ResetCount()
        {
            
        }
    }
}
