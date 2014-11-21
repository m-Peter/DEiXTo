using DEiXTo.Views;
using DEiXTo.Models;
using System;
using System.Windows.Forms;
using DEiXTo.Services;
using System.Drawing;

namespace DEiXTo.Presenters
{
    public class RegexBuilderPresenter
    {
        private TreeNode _node;
        private IEventHub _eventHub;

        public RegexBuilderPresenter(IRegexBuilderView view, TreeNode node, IEventHub eventHub)
        {
            View = view;
            _node = node;
            View.Presenter = this;
            _eventHub = eventHub;

            populateRegex();
        }

        public IRegexBuilderView View { get; set; }

        private void populateRegex()
        {
            if (_node.HasRegex())
            {
                View.RegexText = _node.GetRegex();
                View.InverseRegex = _node.InverseRegex();
                return;
            }

            View.RegexText = _node.GetContent();
        }

        public void AddRegex()
        {
            string regex = View.RegexText;
            bool inverse = View.InverseRegex;

            if (String.IsNullOrWhiteSpace(regex))
            {
                View.ShowInvalidRegexMessage();
                return;
            }

            _node.SetRegex(regex);
            _node.SetInverse(inverse);

            if (_node.NodeFont != null)
            {
                var font = _node.NodeFont;
                _node.NodeFont = new Font(font, FontStyle.Underline | font.Style);
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

        private bool EnterPressed(Keys key)
        {
            return key == Keys.Enter;
        }
    }
}
