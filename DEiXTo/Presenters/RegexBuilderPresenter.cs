using DEiXTo.Views;
using System;
using System.Windows.Forms;
using DEiXTo.Services;

namespace DEiXTo.Presenters
{
    public class RegexBuilderPresenter
    {
        private IRegexBuilderView _view;
        private TreeNode _node;

        public RegexBuilderPresenter(IRegexBuilderView view, TreeNode node)
        {
            _view = view;
            _node = node;

            _view.AddRegex += addRegex;
            _view.SetRegexText(node.GetContent());
        }

        void addRegex()
        {
            string regex = _view.GetRegexText();

            if (String.IsNullOrWhiteSpace(regex))
            {
                _view.ShowInvalidRegexMessage();
                _view.Exit();
                return;
            }

            // If we got this far, we can publish our RegexAdded event subject
            EventHub eventHub = EventHub.Instance;
            eventHub.Publish(new RegexAdded(regex, _node));
            _view.Exit();
        }
    }
}
