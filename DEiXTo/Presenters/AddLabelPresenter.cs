using DEiXTo.Services;
using DEiXTo.Views;
using System;
using System.Windows.Forms;

namespace DEiXTo.Presenters
{
    public class AddLabelPresenter
    {
        private TreeNode _node;

        public AddLabelPresenter(IAddLabelView view, TreeNode node)
        {
            View = view;
            _node = node;
            View.Presenter = this;
        }

        public IAddLabelView View { get; set; }

        public void AddLabel()
        {
            string label = View.LabelText;

            if (String.IsNullOrWhiteSpace(label))
            {
                View.ShowInvalidLabelMessage();
                View.Exit();
                return;
            }

            _node.SetLabel(label);
            string labeledTag = string.Format("{0}:{1}", _node.Text, label);
            _node.Text = labeledTag;
            View.Exit();
        }

        public void KeyDownPress(Keys key)
        {
            if (EnterPressed(key))
            {
                AddLabel();
            }
        }

        private bool EnterPressed(Keys key)
        {
            return key == Keys.Enter;
        }
    }
}
