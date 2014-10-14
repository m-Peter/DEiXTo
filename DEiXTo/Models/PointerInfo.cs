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
        private string _source;
        private bool _isRoot;
        private NodeState _state;

        /// <summary>
        /// 
        /// </summary>
        public int ElementSourceIndex
        {
            get { return _elementSourceIndex; }
            set { _elementSourceIndex = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRoot
        {
            get { return _isRoot; }
            set { _isRoot = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public NodeState State
        {
            get { return _state; }
            set { _state = value; }
        }
    }
}
