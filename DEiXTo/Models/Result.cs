using System.Collections.Generic;
using System.Windows.Forms;

namespace DEiXTo.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Result
    {
        private List<string> _extractedContents;
        private TreeNode _node;

        public Result()
        {
            _extractedContents = new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        public TreeNode Node
        {
            get { return _node; }
            set { _node = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public void AddContent(string content)
        {
            _extractedContents.Add(content.Trim());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string[] ToStringArray()
        {
            return _extractedContents.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return _extractedContents.Count; }
        }
    }
}
