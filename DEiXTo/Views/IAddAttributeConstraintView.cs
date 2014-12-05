using DEiXTo.Models;
using DEiXTo.Presenters;
using System.Collections.Generic;

namespace DEiXTo.Views
{
    public interface IAddAttributeConstraintView
    {
        AddAttributeConstraintPresenter Presenter { get; set; }
        void LoadAttribute(TagAttribute attribute);
        string Constraint { get; set; }
        void Exit();
    }
}
