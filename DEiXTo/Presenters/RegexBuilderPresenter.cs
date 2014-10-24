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
        private IRegexBuilderView _view;
        private TreeNode _node;
        #endregion

        #region Constructors
        public RegexBuilderPresenter(IRegexBuilderView view, TreeNode node)
        {
            _view = view;
            _node = node;

            _view.AddRegex += addRegex;
            _view.SetRegexText(node.GetContent());
            _view.KeyDownPress += keyDownPress;
        }
        #endregion

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

        #region Private Events
        void keyDownPress(KeyEventArgs e)
        {
            if (EnterPressed(e.KeyCode))
            {
                addRegex();
            }
        }

        void addRegex()
        {
            string regex = _view.GetRegexText();

            if (String.IsNullOrWhiteSpace(regex))
            {
                _view.ShowInvalidRegexMessage();
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

            EventHub eventHub = EventHub.Instance;
            eventHub.Publish(new RegexAdded(regex, _node));

            _view.Exit();
        }
        #endregion
    }
}
