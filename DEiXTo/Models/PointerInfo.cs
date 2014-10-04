﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
