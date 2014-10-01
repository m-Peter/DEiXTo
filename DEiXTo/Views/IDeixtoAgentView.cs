using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Views
{
    public interface IDeixtoAgentView
    {
        event Action BrowseToUrl;
        
        string Url { get; }

        void ShowWarningMessage();
        void NavigateTo(string url);
    }
}
