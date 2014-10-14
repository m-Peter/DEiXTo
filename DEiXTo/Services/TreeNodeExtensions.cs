using DEiXTo.Models;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    /// <summary>
    /// 
    /// </summary>
    public static class TreeNodeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetSource(this TreeNode node)
        {
            PointerInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.Source;
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static NodeState GetState(this TreeNode node)
        {
            PointerInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.State;
            }

            return NodeState.Undefined;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="state"></param>
        public static void SetState(this TreeNode node, NodeState state)
        {
            PointerInfo pInfo = GetPointerInfo(node); ;

            if (pInfo != null)
            {
                pInfo.State = state;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool HasSiblings(this TreeNode node)
        {
            return (node.PrevNode != null || node.NextNode != null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool CanBeVRoot(this TreeNode node)
        {
            return !HasSiblings(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static TreeNode GetClone(this TreeNode node)
        {
            return (TreeNode)node.Clone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public static void SetAsRoot(this TreeNode node)
        {
            PointerInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                pInfo.IsRoot = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool IsRoot(this TreeNode node)
        {
            PointerInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.IsRoot;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool IsTextNode(this TreeNode node)
        {
            return node.Text == "TEXT";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="newNode"></param>
        public static void AddNode(this TreeNode node, TreeNode newNode)
        {
            node.Nodes.Add(newNode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static int SourceIndex(this TreeNode node)
        {
            PointerInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.ElementSourceIndex;
            }

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetPath(this TreeNode node)
        {
            PointerInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.Path;
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetContent(this TreeNode node)
        {
            PointerInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.Content;
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static PointerInfo GetPointerInfo(this TreeNode node)
        {
            return node.Tag as PointerInfo;
        }
    }
}
