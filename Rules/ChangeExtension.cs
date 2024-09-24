using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;

namespace Rules
{
    public class ChangeExtension : IRule
    {
        public string Name => "ChangeExtension";
        public string ExtensionTypes { get; set; } = null;

        public string Rename(string origin)
        {
            string result = origin;

           
            try
            {
                if (ExtensionTypes == null)
                {
                    throw new ArgumentNullException();
                }

                string[] tokens = result.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (Path.GetExtension(origin) != "")
                {
                    if (Path.GetExtension(origin) != ExtensionTypes)
                    {
                        string extension = Path.GetExtension(origin);
                        result = origin.Replace(extension, ExtensionTypes);
                    }
                }
            }
            catch (ArgumentNullException)
            {
                result = "ChangeExtensionRule's properties are null!";

            }

            return result;
        }

        //read ruleInfo to get Prefix
        public IRule Parse(string ruleInfo)
        {
            string name = ruleInfo.Replace(Name + " ", "");

            ExtensionTypes = name;

            return this;
        }

        public void ResetCount()
        {
            
        }
    }
}