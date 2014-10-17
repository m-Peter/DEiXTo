using DEiXTo.Services;
using DEiXTo.Views;
using System;
using System.Windows.Forms;

namespace DEiXTo.Presenters
{
    public class AddLabelPresenter
    {
        private IAddLabelView _view;
        private TreeNode _node;

        public AddLabelPresenter(IAddLabelView view, TreeNode node)
        {
            _view = view;
            _node = node;

            _view.AddLabel += addLabel;
            _view.KeyDownPress += keyDownPress;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void keyDownPress(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                addLabel();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void addLabel()
        {
            string label = _view.GetLabelText();

            if (String.IsNullOrWhiteSpace(label))
            {
                _view.ShowInvalidLabelMessage();
                _view.Exit();
                return;
            }

            // If we got this far, we can publish our LabelAdded event subject
            EventHub eventHub = EventHub.Instance;
            eventHub.Publish(new LabelAdded(label, _node));
            _view.Exit();
        }
    }
}
