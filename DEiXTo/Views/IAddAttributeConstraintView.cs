using DEiXTo.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Views
{
    public interface IAddAttributeConstraintView
    {
        AddAttributeConstraintPresenter Presenter { get; set; }
        string Constraint { get; set; }
        void Exit();
    }
}
