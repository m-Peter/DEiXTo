using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Presenters
{
    public interface IMainWindowPresenter
    {
        void CreateNewAgent();
        void CascadeAgentWindows();
        void CloseAgentWindows();
        void FloatAgentWindows();
        void WindowClosing(FormClosingEventArgs e);
        void UpdateBrowserVersion();
        void ResetBrowserVersion();
    }
}
