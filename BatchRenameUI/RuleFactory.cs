using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;

namespace BatchRenameUI
{
    class RuleFactory
    {
        //storing all the rules loaded from dll files
        static public List<IRule> Rules { get; set; } = new List<IRule>();

        public RuleFactory(List<IRule> rules)
        {
            Rules = rules;
        }


        //process the rule info in the file
        static public IRule Parse(string ruleInfo)
        {
            IRule rule = null;
            string ruleName = ruleInfo.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0];

            //- ask each rule to Parse the ruleInfo itself (encapsulation)
            //- the method does not need to worry about the type of each rule 
            //because each rule implement to the same interface (IRule) which make it has polymorphic
            foreach (var r in Rules)
            {
                if (ruleName == r.Name)
                {
                    rule = r.Parse(ruleInfo);
                }
            }

            return rule;
        }
    }
}
