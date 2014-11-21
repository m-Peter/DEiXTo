using DEiXTo.Services;
using DEiXTo.Models;
using DEiXTo.Views;
using System.Drawing;
using System.Windows.Forms;

namespace DEiXTo.Presenters
{
    public class AddSiblingOrderPresenter
    {
        private TreeNode _node;

        public AddSiblingOrderPresenter(IAddSiblingOrderView view, TreeNode node)
        {
            View = view;
            _node = node;
            View.Presenter = this;

            populateSiblingOrder();
        }

        public IAddSiblingOrderView View { get; set; }

        private void populateSiblingOrder()
        {
            if (_node.GetCareAboutSiblingOrder())
            {
                View.CareAboutSiblingOrder = true;
                View.StartIndex = _node.GetStartIndex();
                View.StepValue = _node.GetStepValue();
            }
        }

        public void AddSiblingOrder()
        {
            int startIndex = View.StartIndex;
            int stepValue = View.StepValue;
            bool careAboutSO = View.CareAboutSiblingOrder;

            _node.SetCareAboutSiblingOrder(careAboutSO);
            _node.SetStartIndex(startIndex);
            _node.SetStepValue(stepValue);

            if (!View.CareAboutSiblingOrder)
            {
                _node.ForeColor = Color.Black;
                View.Exit();
                return;
            }

            _node.ForeColor = Color.CadetBlue;

            View.Exit();
        }

        public void ChangeSiblingOrderVisibility(bool enable)
        {
            if (enable)
            {
                View.EnableSiblingOrderFields();
                return;
            }

            View.DisableSiblingOrderFields();
        }
    }
}
