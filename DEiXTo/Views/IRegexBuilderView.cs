using System;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public interface IRegexBuilderView
    {
        event Action AddRegex;
        event Action<KeyEventArgs> KeyDownPress;

        string GetRegexText();
        void SetRegexText(string regex);
        void ShowInvalidRegexMessage();
        void Exit();
    }
}
