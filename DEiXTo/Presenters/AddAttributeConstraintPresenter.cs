using DEiXTo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DEiXTo.Services;
using DEiXTo.Models;

namespace DEiXTo.Presenters
{
    public class AddAttributeConstraintPresenter
    {
        private TreeNode _node;

        public AddAttributeConstraintPresenter(IAddAttributeConstraintView view, TreeNode node)
        {
            View = view;
            _node = node;
            View.Presenter = this;
            var constraint = _node.GetAttributes().Klass;
            View.Constraint = constraint;
        }

        public IAddAttributeConstraintView View { get; set; }

        public void AddConstraint()
        {
            string constraint = View.Constraint;
            var attrConstraint = new AttributeConstraint { Attribute = "class", Value = constraint };
            _node.SetAttrConstraint(attrConstraint);
            View.Exit();
        }
    }
}
