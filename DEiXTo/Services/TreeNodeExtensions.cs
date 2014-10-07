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
        public static bool IsTextNode(this TreeNode node)
        {
            return node.Text == "TEXT";
        }

        public static void AddNode(this TreeNode node, TreeNode newNode)
        {
            node.Nodes.Add(newNode);
        }

        public static int SourceIndex(this TreeNode node)
        {
            PointerInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.ElementSourceIndex;
            }

            return -1;
        }

        public static string GetPath(this TreeNode node)
        {
            PointerInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.Path;
            }

            return "";
        }

        public static string GetContent(this TreeNode node)
        {
            PointerInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.Content;
            }

            return "";
        }

        private static PointerInfo GetPointerInfo(this TreeNode node)
        {
            return node.Tag as PointerInfo;
        }
    }
}
