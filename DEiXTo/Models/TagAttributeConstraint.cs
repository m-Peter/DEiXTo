namespace DEiXTo.Models
{
    public class TagAttributeConstraint
    {
        private string _attribute;
        private RegexConstraint _constraint;

        public TagAttributeConstraint(string attribute, string value, ConstraintAction action = ConstraintAction.MatchAndExtract)
        {
            _attribute = attribute;
            _constraint = new RegexConstraint(value, action);
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
    }
}
