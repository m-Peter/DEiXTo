using DEiXTo.Models;
using System.Collections.Generic;
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
        private int _counter;
        private List<Result> _results;

        public PatternExtraction(TreeNode pattern, TreeNodeCollection domNodes)
        {
            _pattern = pattern;
            _domNodes = domNodes;
            _counter = 0;
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
            _counter = 0;
            _results = new List<Result>();
            // Remove the nodes in Unchecked state from the Pattern.
            /*var copiedPattern = new TreeNode(_pattern.Text);
            //copiedPattern.Tag = _pattern.Tag;
            FilterUncheckedNodes(_pattern.Nodes, copiedPattern);*/

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
                    _counter += 1;
                    // this is where the matching has succeeded and node
                    // is a instance that matched.
                    _results.Add(result);
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
                    _counter += 1;
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
        public int Results
        {
            get { return _counter; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Result> ExtractedResults()
        {
            return _results;
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
        public bool hasContent(TreeNode node)
        {
            var state = node.GetState();
            bool result;

            switch (state)
            {
                case NodeState.Checked:
                    result = true;
                    break;
                case NodeState.CheckedImplied:
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool hasSource(TreeNode node)
        {
            var state = node.GetState();

            if (state == NodeState.CheckedSource)
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
        public bool isRequired(TreeNode node)
        {
            if (node.GetState() == NodeState.Grayed || node.GetState() == NodeState.Checked || node.GetState() == NodeState.CheckedSource)
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

        public void AddContentFromInstance(TreeNode node, Result result)
        {
            if (hasContent(node))
            {
                result.AddContent(node.GetContent());
            }
            else if (hasSource(node))
            {
                result.AddContent(node.GetSource());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public bool CompareRecursiveTree(TreeNode left, TreeNode right, Result result)
        {
            // if the two nodes don't match, then cancel the current instance
            if (left.Text != right.Text)
            {
                return false;
            }

            right.SetState(left.GetState());
            AddContentFromInstance(right, result);

            // if left.state == (Grayed || CheckedSource || Checked)
            //   move both trees by selecting the next childs from each
            //   tree node.
            // if left.state == (CheckedImplied || GrayedImplied || Unchecked)
            //   if left.Text == right.Text
            //     move both trees by selecting the next childs from each
            //     tree node.
            //   else
            //     move only the node of the left tree by picking its first
            //     child node.
            if (isRequired(left))
            {
                for (int i = 0; i < left.Nodes.Count; i++)
                {
                    var nextLeft = left.Nodes[i];

                    if (isOptional(nextLeft))
                    {
                        return true;
                    }
                    else if (isSkipped(nextLeft))
                    {
                        return true;
                    }
                    else
                    {
                        if (left.Nodes.Count != right.Nodes.Count)
                        {
                            return false;
                        }
                        var nextRight = right.Nodes[i];

                        if (!CompareRecursiveTree(nextLeft, nextRight, result))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
