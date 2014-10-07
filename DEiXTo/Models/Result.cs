using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Models
{
    public class Result
    {
        private List<string> _extractedContents;

        public Result()
        {
            _extractedContents = new List<string>();
        }

        public void AddContent(string content)
        {
            _extractedContents.Add(content);
        }

        public List<string> GetContents()
        {
            return _extractedContents;
        }

        public string[] ToStringArray()
        {
            return _extractedContents.ToArray();
        }

        public int Count
        {
            get { return _extractedContents.Count; }
        }
    }
}
