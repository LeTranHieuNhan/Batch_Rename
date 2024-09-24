using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public interface IRule
    {
        string Name { get; }
        string Rename(string name);

        IRule Parse(string ruleInfo);
        void ResetCount();
    }
}
