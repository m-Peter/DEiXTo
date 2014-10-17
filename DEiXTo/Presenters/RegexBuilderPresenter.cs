using DEiXTo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            }
        }
    }
}
