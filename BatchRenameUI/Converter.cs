using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Contract;

namespace BatchRenameUI
{
    class Converter : IValueConverter
    {
        public List<IRule> RuleList { get; set; } = new List<IRule>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = (string)value;

            foreach(var rule in RuleList)
            {
                result = rule.Rename(result);
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
