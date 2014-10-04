using DEiXTo.Models;
using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public static class TreeNodeExtensions
    {
        public static int SourceIndex(this TreeNode node)
        {
            PointerInfo pInfo = node.Tag as PointerInfo;

            if (pInfo != null)
            {
                return pInfo.ElementSourceIndex;
            }

            return -1;
        }

        public static string GetPath(this TreeNode node)
        {
            PointerInfo pInfo = node.Tag as PointerInfo;

            if (pInfo != null)
            {
                return pInfo.Path;
            }

            return "";
        }

        public static string GetContent(this TreeNode node)
        {
            PointerInfo pInfo = node.Tag as PointerInfo;

            if (pInfo != null)
            {
                return pInfo.Content;
            }

            return "";
        }
    }
}
