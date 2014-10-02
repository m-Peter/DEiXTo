using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public interface IDeixtoAgentView
    {
        event Action BrowseToUrl;
        event Action<KeyEventArgs> KeyDownPress;
        event Action<Boolean> AutoFillChanged;
        event Action<Boolean> CrawlingChanged;
        event Action BrowserCompleted;
        event Action<HtmlElement> DocumentMouseOver;
        event Action<HtmlElement> DocumentMouseLeave;
        
        string Url { get; }

        void ShowWarningMessage();
        void NavigateTo(string url);
        void NavigateForward();
        void NavigateBack();
        void ApplyVisibilityStateInAutoFill(bool state);
        void ApplyVisibilityStateInCrawling(bool state);
        HtmlElement GetHTMLElement();
        void FillDomTree(TreeNode node);
        bool HighlightModeEnabled();
    }
}
