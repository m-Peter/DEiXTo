using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Views
{
    public interface IAddSiblingOrderView
    {
        event Action<Boolean> SiblingOrderCheckboxChanged;
        event Action AddSiblingOrder;

        void ApplyVisibilityStateInOrdering(bool checkedState);
        int GetStartIndex { get; }
        int GetStepValue { get; }
        bool CareAboutSiblingOrder { get; }
        void Exit();
    }
}
