using DEiXTo.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Views
{
    public interface IAddSiblingOrderView
    {
        AddSiblingOrderPresenter Presenter { get; set; }

        void ApplyVisibilityStateInOrdering(bool checkedState);
        int GetStartIndex { get; set; }
        int GetStepValue { get; set; }
        bool CareAboutSiblingOrder { get; set; }
        void Exit();
    }
}
