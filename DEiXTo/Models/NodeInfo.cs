namespace DEiXTo.Models
{
    /// <summary>
    /// Maintains various necessary properties for a TreeNode of DOM Tree.
    /// </summary>
    public class NodeInfo
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public int ElementSourceIndex { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Source { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Label { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Regex { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsRoot { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public NodeState State { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool CareAboutSiblingOrder { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int SiblingOrderStart { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int SiblingOrderStep { get; set; }
        #endregion
    }
}
