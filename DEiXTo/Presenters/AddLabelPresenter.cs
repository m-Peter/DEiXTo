using DEiXTo.Services;
using DEiXTo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

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
