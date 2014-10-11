namespace DEiXTo.Models
{
    /// <summary>
    /// Maintains various necessary properties for a TreeNode of DOM Tree.
    /// </summary>
    public class PointerInfo
    {
        private int _elementSourceIndex;
        private string _path;
        private string _content;
        private bool _isRoot;
        private NodeState _state;

        public int ElementSourceIndex
        {
            get { return _elementSourceIndex; }
            set { _elementSourceIndex = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public bool IsRoot
        {
            get { return _isRoot; }
            set { _isRoot = value; }
        }

        public NodeState State
        {
            get { return _state; }
            set { _state = value; }
        }
    }
}
