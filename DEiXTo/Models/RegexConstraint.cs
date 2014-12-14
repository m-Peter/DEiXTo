﻿using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DEiXTo.Models
{
    public enum ConstraintAction
    {
        Match,
        MatchAndExtract
    }

    public class RegexConstraint
    {
        private string _input;
        private string _pattern;
        private string _value;
        private ConstraintAction _action;

        public RegexConstraint(string pattern, ConstraintAction action = ConstraintAction.MatchAndExtract)
        {
            _pattern = pattern;
            _value = String.Empty;
            _action = action;
        }

        public string Input
        {
            get { return _input; }
        }

        public string Pattern
        {
            get { return _pattern; }
        }

        public ConstraintAction Action
        {
            get { return _action; }
        }

        public bool Evaluate(string input)
        {
            _input = input;
            var match = Regex.Match(input, _pattern);

            if (!match.Success)
            {
                return false;
            }

            var sb = new StringBuilder();
            sb.Append(match.Value);
            while (match.Success)
            {
                match = match.NextMatch();
                sb.Append(" " + match.Value);
            }

            _value = sb.ToString(0, sb.Length - 1);
            return true;
        }

        public string Value
        {
            get
            {
                if (_action == ConstraintAction.Match)
                {
                    return _input;
                }

                return _value;
            }
        }
    }
}