using System.Collections.Generic;
using System.Windows.Forms;

namespace DEiXTo.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Result
    {
        #region Instance Variables
        private List<string> _extractedContents;
        private TreeNode _node;
        #endregion

        #region Constructors
        public Result()
        {
            _extractedContents = new List<string>();
        }
        #endregion

        #region Properties
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
        public int Count
        {
            get { return _extractedContents.Count; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public void AddContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return;
            }

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
        /// <returns></returns>
        public IEnumerable<string> Contents()
        {
            int count = Count;

            for (int i = 0; i < count; i++)
            {
                yield return _extractedContents[i];
            }
        }
        #endregion
    }
}
