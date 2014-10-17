using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Views
{
    public interface IRegexBuilderView
    {
        event Action AddRegex;

        string GetRegexText();
        void SetRegexText(string regex);
        void ShowInvalidRegexMessage();
    }
}
