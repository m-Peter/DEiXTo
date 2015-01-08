using System;
using System.Windows.Forms;

namespace DEiXTo.Models
{
    public static class TreeNodeExtensions
    {
        public static bool HasNextNode(this TreeNode node, int index)
        {
            return node.Nodes.Count <= index;
        }

        public static void SetRegexConstraint(this TreeNode node, RegexConstraint constraint)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                pInfo.RegexConstraint = constraint;
            }
        }

        public static RegexConstraint GetRegexConstraint(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.RegexConstraint;
            }

            return null;
        }

        public static bool HasRegexConstraint(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.RegexConstraint != null;
            }

            return false;
        }

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

        public static TagAttributeCollection GetAttributes(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.Attributes;
            }

            return null;
        }

        public static void SetAttributes(this TreeNode node, TagAttributeCollection attributes)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                pInfo.Attributes = attributes;
            }
        }

        public static TagAttributeConstraint GetAttrConstraint(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.AttrConstraint;
            }

            return null;
        }

        public static void SetAttrConstraint(this TreeNode node, TagAttributeConstraint constraint)
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

        public static void SetLabel(this TreeNode node, string label)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                pInfo.Label = label;
            }
        }

        public static string GetRegex(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.Regex;
            }

            return "";
        }

        public static void SetRegex(this TreeNode node, string regex)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                pInfo.Regex = regex;
            }
        }

        public static string GetLabel(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.Label;
            }

            return "";
        }

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

        public static NodeState GetState(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.State;
            }

            return NodeState.Undefined;
        }

        public static void SetState(this TreeNode node, NodeState state)
        {
            NodeInfo pInfo = GetPointerInfo(node); ;

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
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                pInfo.IsRoot = true;
            }
        }

        public static bool IsRoot(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.IsRoot;
            }

            return false;
        }

        public static bool IsTextNode(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.IsTextNode;
            }

            return false;
        }

        public static void AddNode(this TreeNode node, TreeNode newNode)
        {
            node.Nodes.Add(newNode);
        }

        public static int SourceIndex(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.SourceIndex;
            }

            return -1;
        }

        public static string GetPath(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.Path;
            }

            return "";
        }

        public static string GetContent(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.Content;
            }

            return "";
        }

        public static void SetContent(this TreeNode node, string content)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                pInfo.Content = content;
            }
        }

        public static void SetCareAboutSiblingOrder(this TreeNode node, bool value)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                pInfo.CareAboutSiblingOrder = value;
            }
        }

        public static bool GetCareAboutSiblingOrder(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.CareAboutSiblingOrder;
            }

            return false;
        }

        public static void SetStartIndex(this TreeNode node, int startIndex)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                pInfo.SiblingOrderStart = startIndex;
            }
        }

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

        public static int GetStepValue(this TreeNode node)
        {
            NodeInfo pInfo = GetPointerInfo(node);

            if (pInfo != null)
            {
                return pInfo.SiblingOrderStep;
            }

            return -1;
        }

        private static NodeInfo GetPointerInfo(this TreeNode node)
        {
            return node.Tag as NodeInfo;
        }
    }
}
