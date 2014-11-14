using DEiXTo.Services;
using DEiXTo.Views;
using System.Drawing;
using System.Windows.Forms;

namespace DEiXTo.Presenters
{
    public class AddSiblingOrderPresenter
    {
        #region Instance Variables
        private TreeNode _node;
        #endregion

        #region Constructors
        public AddSiblingOrderPresenter(IAddSiblingOrderView view, TreeNode node)
        {
            View = view;
            _node = node;
            View.Presenter = this;

            populateSiblingOrder();
        }
        #endregion

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
            if (!View.CareAboutSiblingOrder)
            {
                View.Exit();
                return;
            }

            int startIndex = View.StartIndex;
            int stepValue = View.StepValue;
            bool careAboutSO = View.CareAboutSiblingOrder;

            _node.SetCareAboutSiblingOrder(careAboutSO);
            _node.SetStartIndex(startIndex);
            _node.SetStepValue(stepValue);

            _node.ForeColor = Color.CadetBlue;

            View.Exit();
        }

        public void ChangeSiblingOrderVisibility(bool state)
        {
            View.ApplyVisibilityStateInOrdering(state);
        }
    }
}
