using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Contract;

namespace Rules
{
    public class AddSuffix : IRule
    {
        public string Name => "AddSuffix";

        public string Suffix { get; set; } = null;


        public string Rename(string filename)
        {
            string result = filename;
            try
            {
                if (Suffix == null)
                {
                    throw new ArgumentNullException();
                }

                string[] tokens = result.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

                if (!tokens[0].Contains(Suffix))
                {
                    result = tokens[0] + " " + Suffix + Path.GetExtension(filename);
                }
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
            string suffix = ruleInfo.Replace(temp, null);
            if (suffix.Length >= 1)
            {
                suffix = suffix.Remove(0, 1);
            }
            Suffix = suffix;

            return this;
        }

        public void ResetCount()
        {
            
        }
    }
}
