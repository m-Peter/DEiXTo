using DEiXTo.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class PatternExtraction
    {
        private TreeNode _pattern;
        private TreeNodeCollection _domNodes;
        private List<Result> _results;

        public PatternExtraction(TreeNode pattern, TreeNodeCollection domNodes)
        {
            _pattern = pattern;
            _domNodes = domNodes;
            _results = new List<Result>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int CountOutputVariables()
        {
            int outputVariables = 0;

            if (IsOutputVariable(_pattern))
            {
                outputVariables += 1;
            }

            countVariables(_pattern.Nodes, ref outputVariables);

            return outputVariables;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="counter"></param>
        private void countVariables(TreeNodeCollection nodes, ref int counter)
        {
            foreach (TreeNode node in nodes)
            {
                if (IsOutputVariable(node))
                {
                    counter += 1;
                }
                countVariables(node.Nodes, ref counter);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsOutputVariable(TreeNode node)
        {
            var state = node.GetState();
            
            if (state == NodeState.Checked || state == NodeState.CheckedSource || state == NodeState.CheckedImplied)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="filtered"></param>
        public void FilterUncheckedNodes(TreeNodeCollection nodes, TreeNode filtered)
        {
            foreach (TreeNode node in nodes)
            {
                var state = node.GetState();

                if (state != NodeState.Unchecked)
                {
                    var newNode = new TreeNode(node.Text);
                    newNode.Tag = node.Tag;
                    filtered.Nodes.Add(newNode);
                    FilterUncheckedNodes(node.Nodes, newNode);
                }
                else
                {
                    FilterUncheckedNodes(node.Nodes, filtered);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void FindMatches()
        {
            _results = new List<Result>();

            if (_pattern.IsRoot())
            {
                Match(_pattern, _domNodes);
            }
            else
            {
                // Extract the tree above the virtual root node.
                var ancestors = new TreeNode(_pattern.Text);
                BuiltAncestorsTree(_pattern.Nodes, ancestors);

                TreeNode vRoot = null;
                FindRoot(_pattern.Nodes, ref vRoot);

                MatchSplit(vRoot, _domNodes, ancestors);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="nodes"></param>
        public void Match(TreeNode pattern, TreeNodeCollection nodes)
        {
            var result = new Result();
            foreach (TreeNode node in nodes)
            {
                if (CompareRecursiveTree(pattern, node, result))
                {
                    // this is where the matching has succeeded and node
                    // is a instance that matched.
                    result.Node = node;
                    _results.Add(result);
                    result = new Result();
                }
                Match(pattern, node.Nodes);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="nodes"></param>
        /// <param name="upper"></param>
        public void MatchSplit(TreeNode pattern, TreeNodeCollection nodes, TreeNode upper)
        {
            var result = new Result();
            foreach (TreeNode node in nodes)
            {
                if (CompareRecursiveTree(pattern, node, result))
                {
                    string fw = "";
                    int count = 0;
                    traverse(upper, ref fw, ref count);
                    string bw = "";
                    int counter = 0;
                    backward(node.Parent, ref bw, count, ref counter);
                    if (fw != bw)
                    {
                        return;
                    }
                    _results.Add(result);
                    result = new Result();
                }
                MatchSplit(pattern, node.Nodes, upper);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="format"></param>
        /// <param name="count"></param>
        public void traverse(TreeNode t, ref string format, ref int count)
        {
            for (int i = t.Nodes.Count - 1; i >= 0; i--)
            {
                traverse(t.Nodes[i], ref format, ref count);
            }
            count += 1;
            format += (t.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="format"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        public void backward(TreeNode t, ref string format, int limit, ref int count)
        {
            if (limit == count)
            {
                return;
            }
            format += t.Text;
            if (t.Parent != null)
            {
                count += 1;
                backward(t.Parent, ref format, limit, ref count);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return _results.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Result> ExtractedResults()
        {
            int count = _results.Count;

            for (int i = 0; i < count; i++)
            {
                yield return _results[i];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="root"></param>
        public void FindRoot(TreeNodeCollection nodes, ref TreeNode root)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.IsRoot())
                {
                    root = node.GetClone();
                    return;
                }
                FindRoot(node.Nodes, ref root);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="root"></param>
        public void BuiltAncestorsTree(TreeNodeCollection nodes, TreeNode root)
        {
            foreach (TreeNode node in nodes)
            {
                if (!node.IsRoot())
                {
                    var newNode = new TreeNode(node.Text);
                    root.Nodes.Add(newNode);
                    BuiltAncestorsTree(node.Nodes, newNode);
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool isRequired(TreeNode node)
        {
            var state = node.GetState();

            if (state == NodeState.Grayed || state == NodeState.Checked || state == NodeState.CheckedSource)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool isOptional(TreeNode node)
        {
            var state = node.GetState();

            if (state == NodeState.CheckedImplied || state == NodeState.GrayedImplied)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool isSkipped(TreeNode node)
        {
            var state = node.GetState();

            if (state == NodeState.Unchecked)
            {
                return true;
            }

            return false;
        }

        public void AddContentFromInstance(NodeState state, TreeNode node, Result result)
        {
            if (ContainsContent(state))
            {
                result.AddContent(node.GetContent());
            }
            else if (ContainsSource(state))
            {
                result.AddContent(node.GetSource());
            }
        }

        public bool ContainsContent(NodeState state)
        {
            if (state == NodeState.Checked || state == NodeState.CheckedImplied)
            {
                return true;
            }

            return false;
        }

        public bool ContainsSource(NodeState state)
        {
            return state == NodeState.CheckedSource;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public bool CompareRecursiveTree(TreeNode left, TreeNode right, Result result)
        {
            //  LEFT                      RIGHT
            //
            // -section                  -section
            //    -a                        -a
            //        -img                      -img
            //    -h2                       -h2
            //        -a                        -a
            //            -text                     -text
            //    -p                        -p


            // (1) if left.Tag != right.Tag
            //      return false;

            // (2) extractContentFrom(right)
            
            // (3) inspect the state of the left node
            // it can be in Grayed || CheckedSource || Checked || CheckedImplied || GrayedImplied || Unchecked

            // Case left.State == Grayed
            // this node is required so the right node has to be present, if it's not return false.
            
            // Case left.state == CheckedSource
            // this node is required so the right node has to be present, if it's not return false.
            // We also want to extract the source of this node.

            // Case left.state == Checked
            // this node is required so the right node has to be present, if it's not return false.
            // We also want to extract the content of this node. (innerText, href, src, name).

            // Case left.state == CheckedImplied
            // this node is optional, so we examine the right node. If it's present and it matches
            // the left.Tag then we extract its content (innerText, href, src, name). If it's not
            // present or does not match in terms of Tag, we return true.

            // Case left.state == GrayedImplied
            // this node is optional, so we examine the right node. If it's present and it matches
            // the left.Tag then we return true. If it's not present or does not match in terms of
            // Tag, we return true.

            // Case left.state == Unchecked
            // this node is not important, so we don't examine the right node. We just return true.

            if (left.Text != right.Text)
            {
                return false;
            }

            if (left.HasRegex())
            {
                string content = right.GetContent();
                string regex = left.GetRegex();

                Match match = Regex.Match(content, regex);

                if (!match.Success)
                {
                    return false;
                }
            }

            AddContentFromInstance(left.GetState(), right, result);

            int childNodes = left.Nodes.Count;

            for (int i = 0; i < childNodes; i++)
            {
                var nextLeft = left.Nodes[i];

                if (isRequired(nextLeft))
                {
                    if (right.Nodes.Count <= i)
                    {
                        return false;
                    }
                    var nextRight = right.Nodes[i];

                    if (!CompareRecursiveTree(nextLeft, nextRight, result))
                    {
                        return false;
                    }
                }
                else if (isOptional(nextLeft))
                {
                    if (right.Nodes.Count <= i)
                    {
                        return true;
                    }
                    var nextRight = right.Nodes[i];

                    CompareRecursiveTree(nextLeft, nextRight, result);
                }
            }

            return true;
        }
    }
}
