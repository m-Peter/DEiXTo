using System.Collections.Generic;
using System.Windows.Forms;

namespace DEiXTo.Models
{
    public class Result
    {
        private List<string> _extractedContents;
        private TreeNode _node;

        public Result()
        {
            _extractedContents = new List<string>();
        }

        public TreeNode Node
        {
            get { return _node; }
            set { _node = value; }
        }

        public int Count
        {
            get { return _extractedContents.Count; }
        }

        public void AddContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return;
            }

            _extractedContents.Add(content.Trim());
        }

        public string[] ToStringArray()
        {
            return _extractedContents.ToArray();
        }

        public IEnumerable<string> Contents()
        {
            var count = Count;

            for (int i = 0; i < count; i++)
            {
                yield return _extractedContents[i];
            }
        }
    }
}
