using DEiXTo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Presenters
{
    public class AddAttributeConstraintPresenter
    {
        private TreeNode _node;

        public AddAttributeConstraintPresenter(IAddAttributeConstraintView view, TreeNode node)
        {
            View = view;
            _node = node;
        }

        public IAddAttributeConstraintView View { get; set; }
    }
}
