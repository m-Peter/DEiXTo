using DEiXTo.Services;
using DEiXTo.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Presenters
{
    public class AddSiblingOrderPresenter
    {
        private IAddSiblingOrderView _view;
        private TreeNode _node;

        public AddSiblingOrderPresenter(IAddSiblingOrderView view, TreeNode node)
        {
            _view = view;
            _node = node;

            _view.SiblingOrderCheckboxChanged += siblingOrderCheckboxChanged;
            _view.AddSiblingOrder += addSiblingOrder;
        }

        void addSiblingOrder()
        {
            if (!_view.CareAboutSiblingOrder)
            {
                _view.Exit();
                return;
            }

            int startIndex = _view.GetStartIndex;
            int stepValue = _view.GetStepValue;
            bool careAboutSO = _view.CareAboutSiblingOrder;

            _node.SetCareAboutSiblingOrder(careAboutSO);
            _node.SetStartIndex(startIndex);
            _node.SetStepValue(stepValue);

            _node.ForeColor = Color.CadetBlue;

            _view.Exit();
        }

        void siblingOrderCheckboxChanged(bool checkedState)
        {
            _view.ApplyVisibilityStateInOrdering(checkedState);
        }
    }
}
