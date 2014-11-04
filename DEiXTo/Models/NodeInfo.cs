namespace DEiXTo.Models
{
    /// <summary>
    /// Maintains various necessary properties for a TreeNode of DOM Tree.
    /// </summary>
    public class NodeInfo
    {
        #region Properties
        public int SourceIndex { get; set; }
        public string Path { get; set; }
        public string Content { get; set; }
        public string Source { get; set; }
        public string Label { get; set; }
        public string Regex { get; set; }
        public bool IsRoot { get; set; }
        public NodeState State { get; set; }
        public bool CareAboutSiblingOrder { get; set; }
        public int SiblingOrderStart { get; set; }
        public int SiblingOrderStep { get; set; }
        public AttributeCollection Attributes { get; set; }
        public AttributeConstraint AttrConstraint { get; set; }

        public class Builder
        {
            private int _sourceIndex;
            private string _path;
            private string _content;
            private string _source;
            private string _label;
            private string _regex;
            private bool _isRoot;
            private NodeState _state;
            private bool _careAboutSiblingOrder;
            private int _siblingOrderStart;
            private int _siblingOrderStep;

            public Builder()
            {

            }

            public Builder SetSourceIndex(int sourceIndex)
            {
                _sourceIndex = sourceIndex;
                return this;
            }

            public Builder SetPath(string path)
            {
                _path = path;
                return this;
            }

            public Builder SetContent(string content)
            {
                _content = content;
                return this;
            }

            public Builder SetSource(string source)
            {
                _source = source;
                return this;
            }

            public Builder SetLabel(string label)
            {
                _label = label;
                return this;
            }

            public Builder SetRegex(string regex)
            {
                _regex = regex;
                return this;
            }

            public Builder SetRoot(bool isRoot)
            {
                _isRoot = isRoot;
                return this;
            }

            public Builder SetState(NodeState state)
            {
                _state = state;
                return this;
            }

            public Builder SetCareAboutSO(bool value)
            {
                _careAboutSiblingOrder = value;
                return this;
            }

            public Builder SetStartIndex(int index)
            {
                _siblingOrderStart = index;
                return this;
            }

            public Builder SetStepValue(int value)
            {
                _siblingOrderStep = value;
                return this;
            }

            public NodeInfo Build()
            {
                return new NodeInfo(this);
            }

            public int SourceIndex
            {
                get { return _sourceIndex; }
            }

            public string Path
            {
                get { return _path; }
            }

            public string Content
            {
                get { return _content; }
            }

            public string Source
            {
                get { return _source; }
            }

            public string Label
            {
                get { return _label; }
            }

            public string Regex
            {
                get { return _regex; }
            }

            public bool IsRoot
            {
                get { return _isRoot; }
            }

            public NodeState State
            {
                get { return _state; }
            }

            public bool CareAboutSO
            {
                get { return _careAboutSiblingOrder; }
            }

            public int StartIndex
            {
                get { return _siblingOrderStart; }
            }

            public int StepValue
            {
                get { return _siblingOrderStep; }
            }
        }

        public NodeInfo()
        {

        }

        private NodeInfo(Builder builder)
        {
            SourceIndex = builder.SourceIndex;
            Path = builder.Path;
            Content = builder.Content;
            Source = builder.Source;
            Label = builder.Label;
            Regex = builder.Regex;
            IsRoot = builder.IsRoot;
            State = builder.State;
            CareAboutSiblingOrder = builder.CareAboutSO;
            SiblingOrderStart = builder.StartIndex;
            SiblingOrderStep = builder.StepValue;
        }
        #endregion
    }
}
