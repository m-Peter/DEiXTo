﻿using DEiXTo.Views;
using System.Collections.Generic;
using System.Windows.Forms;
using DEiXTo.Services;
using DEiXTo.Models;
using System.Linq;

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

            populateAttributes();
        }

        private void populateAttributes()
        {
            var attributes = _node.GetAttributes();
            
            if (attributes == null)
            {
                return;
            }
            
            View.LoadAttribute(attributes.All[0]);

            if (_node.HasAttrConstraint())
            {
                selectNodeConstraint(attributes.All);
            }
        }

        private void selectNodeConstraint(List<TagAttribute> attributes)
        {
            var constraint = _node.GetAttrConstraint();
            var selectedTag = GetSelectedAttribute(attributes, constraint.Attribute);
        }

        private TagAttribute GetSelectedAttribute(List<TagAttribute> attributes, string tagAttribute)
        {
            return attributes.FirstOrDefault(attr => attr.Name == tagAttribute);
        }

        public IAddAttributeConstraintView View { get; set; }

        public void AttributeChanged(TagAttribute tagAttribute)
        {
            View.Constraint = tagAttribute.Value;
        }

        public void AddConstraint(string attributeName)
        {
            var constraint = View.Constraint;
            var attrConstraint = new TagAttributeConstraint(attributeName, constraint, NodeState.Grayed);
            _node.SetConstraint(attrConstraint);
            View.Exit();
        }

        public void KeyDown(Keys key, string attributeName)
        {
            if (EnterPressed(key))
            {
                AddConstraint(attributeName);
            }
        }

        private bool EnterPressed(Keys key)
        {
            return key == Keys.Enter;
        }
    }
}
