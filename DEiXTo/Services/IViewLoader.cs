using DEiXTo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public interface IViewLoader
    {
        void LoadMainView();
        void LoadAgentView(string title, IMainView parent);
    }
}
