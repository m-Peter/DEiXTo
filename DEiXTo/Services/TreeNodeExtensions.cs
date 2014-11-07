using DEiXTo.Models;
using System;
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
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.Source;
            }

            return "";
        }
        
        public static bool HasAttrConstraint(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.AttrConstraint != null;
            }

            return false;
        }

        public static AttributeCollection GetAttributes(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.Attributes;
            }

            return null;
        }

        public static AttributeConstraint GetAttrConstraint(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.AttrConstraint;
            }

            return null;
        }

        public static void SetAttrConstraint(this TreeNode node, AttributeConstraint constraint)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                pInfo.AttrConstraint = constraint;
            }
        }

        public static bool IsOutputVariable(this TreeNode node)
        {
            var state = GetPointerInfo(node).State;

            if (state == NodeState.Checked || state == NodeState.CheckedSource || state == NodeState.CheckedImplied)
            {
                return true;
            }

            return false;
        }

        public static bool IsRequired(this TreeNode node)
        {
            var state = GetPointerInfo(node).State;

            if (state == NodeState.Grayed || state == NodeState.Checked || state == NodeState.CheckedSource)
            {
                return true;
            }

            return false;
        }

        public static bool IsOptional(this TreeNode node)
        {
            var state = GetPointerInfo(node).State;

            if (state == NodeState.CheckedImplied || state == NodeState.GrayedImplied)
            {
                return true;
            }

            return false;
        }

        public static bool IsSkipped(this TreeNode node)
        {
            var state = GetPointerInfo(node).State;

            if (state == NodeState.Unchecked)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="label"></param>
        public static void SetLabel(this TreeNode node, string label)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                pInfo.Label = label;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetRegex(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.Regex;
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="regex"></param>
        public static void SetRegex(this TreeNode node, string regex)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                pInfo.Regex = regex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetLabel(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.Label;
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool HasLabel(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            string label = GetLabel(node);

            if (String.IsNullOrWhiteSpace(label))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool HasRegex(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            string regex = GetRegex(node);

            if (String.IsNullOrWhiteSpace(regex))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static NodeState GetState(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

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
            NodeInfo pInfo = GetPointerInfo(node); ;

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
            NodeInfo pInfo = GetPointerInfo(node);

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
            NodeInfo pInfo = GetPointerInfo(node);

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
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.SourceIndex;
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
            NodeInfo pInfo = GetPointerInfo(node);

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
            NodeInfo pInfo = GetPointerInfo(node);

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
        /// <param name="content"></param>
        public static void SetContent(this TreeNode node, string content)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                pInfo.Content = content;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="value"></param>
        public static void SetCareAboutSiblingOrder(this TreeNode node, bool value)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                pInfo.CareAboutSiblingOrder = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool GetCareAboutSiblingOrder(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.CareAboutSiblingOrder;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="startIndex"></param>
        public static void SetStartIndex(this TreeNode node, int startIndex)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                pInfo.SiblingOrderStart = startIndex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="stepValue"></param>
        public static void SetStepValue(this TreeNode node, int stepValue)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                pInfo.SiblingOrderStep = stepValue;
            }
        }

        public static int GetStartIndex(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.SiblingOrderStart;
            }

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public static int GetStepValue(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.SiblingOrderStep;
            }

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static NodeInfo GetPointerInfo(this TreeNode node)
        {
            return node.Tag as NodeInfo;
        }
    }
}
