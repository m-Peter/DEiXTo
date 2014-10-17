using System;

namespace DEiXTo.Views
{
    public interface IRegexBuilderView
    {
        event Action AddRegex;

        string GetRegexText();
        void SetRegexText(string regex);
        void ShowInvalidRegexMessage();
        void Exit();
    }
}
