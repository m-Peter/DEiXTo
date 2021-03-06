﻿namespace DEiXTo.Models
{
    public class TagAttributeConstraint : IConstraint
    {
        private string _attribute;
        private RegexConstraint _constraint;

        public TagAttributeConstraint(string attribute, string value, NodeState state)
        {
            _attribute = attribute;
            _constraint = new RegexConstraint(value, state);
        }

        public string Attribute
        {
            get { return _attribute; }
        }

        public string Value
        {
            get { return _constraint.Value; }
        }

        public string Pattern
        {
            get { return _constraint.Pattern; }
        }

        public bool Evaluate(string input)
        {
            return _constraint.Evaluate(input);
        }

        public bool Evaluate(NodeInfo instance)
        {
            var attribute = instance.Attributes.GetByName(_attribute);
            return Evaluate(attribute.Value);
        }
    }
}
