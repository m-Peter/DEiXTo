using DEiXTo.Models;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public static class TreeNodeExtensions
    {
        public static NodeState GetState(this TreeNode node)
        {
            PointerInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.State;
            }

            return NodeState.Undefined;
        }

        public static void SetState(this TreeNode node, NodeState state)
        {
            PointerInfo pInfo = GetPointerInfo(node); ;

            if (pInfo != null)
            {
                pInfo.State = state;
            }
        }

        public static bool HasSiblings(this TreeNode node)
        {
            return (node.PrevNode != null || node.NextNode != null);
        }

        public static bool CanBeVRoot(this TreeNode node)
        {
            return !HasSiblings(node);
        }

        public static TreeNode GetClone(this TreeNode node)
        {
            return (TreeNode)node.Clone();
        }

        public static void SetAsRoot(this TreeNode node)
        {
            PointerInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                pInfo.IsRoot = true;
            }
        }

        public static bool IsRoot(this TreeNode node)
        {
            PointerInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.IsRoot;
            }

            return false;
        }

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
