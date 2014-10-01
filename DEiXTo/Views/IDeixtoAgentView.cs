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
        
        string Url { get; }

        void ShowWarningMessage();
        void NavigateTo(string url);
        void NavigateForward();
        void NavigateBack();
    }
}
