using DEiXTo.Views;
using System;
using System.Windows.Forms;
using DEiXTo.Services;
using System.Drawing;

namespace DEiXTo.Presenters
{
    public class RegexBuilderPresenter
    {
        #region Instance Variables
        private TreeNode _node;
        private IEventHub _eventHub;
        #endregion

        #region Constructors
        public RegexBuilderPresenter(IRegexBuilderView view, TreeNode node, IEventHub eventHub)
        {
            View = view;
            _node = node;
            View.Presenter = this;
            _eventHub = eventHub;

            View.RegexText = node.GetContent();
        }
        #endregion

        public IRegexBuilderView View { get; set; }

        public void AddRegex()
        {
            string regex = View.RegexText;

            if (String.IsNullOrWhiteSpace(regex))
            {
                View.ShowInvalidRegexMessage();
                return;
            }

            _node.SetRegex(regex);
            if (_node.NodeFont != null)
            {
                var font = _node.NodeFont;
                _node.NodeFont = new Font(font, FontStyle.Underline | FontStyle.Bold);
            }
            else
            {
                var font = new Font(FontFamily.GenericSansSerif, 8.25f);
                _node.NodeFont = new Font(font, FontStyle.Underline);
            }

            _eventHub.Publish(new RegexAdded(_node));

            View.Exit();
        }

        public void KeyDownPress(Keys key)
        {
            if (EnterPressed(key))
            {
                AddRegex();
            }
        }

        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool EnterPressed(Keys key)
        {
            return key == Keys.Enter;
        }
        #endregion
    }
}
