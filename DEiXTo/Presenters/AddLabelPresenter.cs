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
            if (EnterPressed(e.KeyCode))
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

            _node.SetLabel(label);
            string labeledTag = string.Format("{0}:{1}", _node.Text, label);
            _node.Text = labeledTag;
            _view.Exit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool EnterPressed(Keys key)
        {
            return key == Keys.Enter;
        }
    }
}
