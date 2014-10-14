﻿using System.Collections.Generic;

namespace DEiXTo.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Result
    {
        private List<string> _extractedContents;

        public Result()
        {
            _extractedContents = new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public void AddContent(string content)
        {
            _extractedContents.Add(content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetContents()
        {
            return _extractedContents;
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
