using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Contract;

namespace Rules
{
    public class LowerCase : IRule
    {
        public string Name => "LowerCase";

        public string Rename(string origin)
        {
            string result = "";
            List<char> arr = origin.ToCharArray(0, origin.Length).ToList();


            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i] == ' ')
                {
                    arr.RemoveAt(i--);
                }
                else
                {
                    arr[i] = Char.ToLower(arr[i]);
                }
            }

            for (int i = 0; i < arr.Count; i++)
            {
                result += arr[i];
            }


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
