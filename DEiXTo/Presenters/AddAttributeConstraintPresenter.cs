using DEiXTo.Views;
using System.Collections.Generic;
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

            var attributes = _node.GetAttributes();
            View.LoadAttributes(attributes.All);

            if (_node.HasAttrConstraint())
            {
                var constraint = _node.GetAttrConstraint();
                var selectedTag = GetSelectedAttribute(attributes.All, constraint.Attribute);
                
                View.SelectAttribute(selectedTag);
            }
        }

        private TagAttribute GetSelectedAttribute(List<TagAttribute> attributes, string tagAttribute)
        {
            foreach (var tag in attributes)
            {
                if (tag.Name == tagAttribute)
                {
                    return tag;
                }
            }

            return null;
        }

        public IAddAttributeConstraintView View { get; set; }

        public void AttributeChanged(TagAttribute tagAttribute)
        {
            View.Constraint = tagAttribute.Value;
        }

        public void AddConstraint(string attributeName)
        {
            string constraint = View.Constraint;
            var attrConstraint = new AttributeConstraint { Attribute = attributeName, Value = constraint };
            _node.SetAttrConstraint(attrConstraint);
            View.Exit();
        }
    }
}
