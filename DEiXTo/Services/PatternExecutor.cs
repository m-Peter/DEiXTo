using DEiXTo.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class PatternExecutor
    {
        private ExtractionPattern _pattern;
        private TreeNodeCollection _domNodes;
        private List<Result> _results;

        public PatternExecutor(ExtractionPattern pattern, TreeNodeCollection domNodes)
        {
            _pattern = pattern;
            _domNodes = domNodes;
            _results = new List<Result>();
        }

        public int CountOutputVariables()
        {
            return _pattern.CountOutputVariables();
        }

        public List<string> OutputVariableLabels()
        {
            return _pattern.OutputVariableLabels();
        }

        public TreeNode ScanTree(TreeNodeCollection nodes, TreeNode pattern)
        {
            foreach (TreeNode node in nodes)
            {
                if (CompareRecursiveTree(node, pattern, new Result()))
                {
                    return node.GetClone();
                }

                TreeNode candidate = ScanTree(node.Nodes, pattern);

                if (candidate != null)
                {
                    return candidate;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void FindMatches()
        {
            _results = new List<Result>();

            if (_pattern.RootNode.IsRoot())
            {
                int counter = 0;
                _pattern.TrimUncheckedNodes();
                Match(_pattern.RootNode, _domNodes, ref counter);
                return;
            }

            // Extract the tree above the virtual root node.
            var upperTree = _pattern.GetUpperTree();
            TreeNode vRoot = _pattern.FindVirtualRoot();
            MatchSplit(vRoot, _domNodes, upperTree);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="nodes"></param>
        private void Match(TreeNode pattern, TreeNodeCollection nodes, ref int counter)
        {
            var result = new Result();
            int start = pattern.GetStartIndex();
            int step = pattern.GetStepValue();

            foreach (TreeNode node in nodes)
            {
                if (CompareRecursiveTree(pattern, node, result))
                {
                    // this is where the matching has succeeded and node
                    // is a instance that matched.

                    if (counter < start)
                    {
                        counter++;
                        result = new Result();
                        continue;
                    }

                    if ((step != 0) && (counter % step != 0))
                    {
                        counter++;
                        result = new Result();
                        continue;
                    }

                    counter++;
                    
                    result.Node = node;
                    _results.Add(result);
                    result = new Result();
                }

                Match(pattern, node.Nodes, ref counter);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="nodes"></param>
        /// <param name="upper"></param>
        private void MatchSplit(TreeNode pattern, TreeNodeCollection nodes, TreeNode upper)
        {
            var result = new Result();

            foreach (TreeNode node in nodes)
            {
                if (CompareRecursiveTree(pattern, node, result))
                {
                    bool match = CheckUpper(upper, node, result);

                    if (!match)
                    {
                        return;
                    }
                    
                    result.Node = node;
                    _results.Add(result);
                    result = new Result();
                }

                MatchSplit(pattern, node.Nodes, upper);
            }
        }

        private bool CheckUpper(TreeNode pattern, TreeNode instance, Result result)
        {
            if (pattern.Text != instance.Text)
            {
                return false;
            }

            var leftParent = pattern.Parent;
            var rightParent = instance.Parent;

            if (leftParent == null)
            {
                return true;
            }

            if (rightParent == null)
            {
                return false;
            }

            if (!CompareTrees(leftParent, rightParent, result))
            {
                return false;
            }

            CheckUpper(leftParent, rightParent, result);

            return true;
        }

        private bool CompareTrees(TreeNode left, TreeNode right, Result result)
        {
            if (left.Text != right.Text)
            {
                return false;
            }

            AddContentFromInstance(left.GetState(), right, result);

            for (int i = 0; i < left.Nodes.Count; i++)
            {
                var nextLeft = left.Nodes[i];
                bool hasNode = HasNextNode(right, i);

                if (hasNode)
                {
                    return false;
                }

                var nextRight = right.Nodes[i];

                if (!CompareTrees(nextLeft, nextRight, result))
                {
                    return false;
                }
            }

            return true;
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
        /// <param name="state"></param>
        /// <param name="node"></param>
        /// <param name="result"></param>
        private void AddContentFromInstance(NodeState state, TreeNode node, Result result)
        {
            if (ContainsContent(state))
            {
                result.AddContent(node.GetContent());
            }

            if (ContainsSource(state))
            {
                result.AddContent(node.GetSource());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private bool ContainsContent(NodeState state)
        {
            if (state == NodeState.Checked || state == NodeState.CheckedImplied)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private bool ContainsSource(NodeState state)
        {
            return state == NodeState.CheckedSource;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private bool CompareRecursiveTree(TreeNode left, TreeNode right, Result result)
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

            if (!TagMatching(left, right))
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
                right.SetContent(match.Value);
            }

            if (left.HasAttrConstraint())
            {
                var constraint = left.GetAttrConstraint();
                string attr = constraint.Attribute;
                string value = constraint.Value;

                var attributes = right.GetAttributes();
                var klass = attributes.GetByName(attr).Value;

                if (value != klass)
                {
                    return false;
                }
            }

            AddContentFromInstance(left.GetState(), right, result);

            int childNodes = left.Nodes.Count;

            for (int i = 0; i < childNodes; i++)
            {
                var nextLeft = left.Nodes[i];
                bool hasNode = HasNextNode(right, i);

                if (nextLeft.IsRequired())
                {
                    if (hasNode)
                    {
                        return false;
                    }

                    var nextRight = right.Nodes[i];

                    if (!CompareRecursiveTree(nextLeft, nextRight, result))
                    {
                        return false;
                    }
                }
                
                if (nextLeft.IsOptional())
                {
                    if (hasNode)
                    {
                        return true;
                    }

                    var nextRight = right.Nodes[i];

                    CompareRecursiveTree(nextLeft, nextRight, result);
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool HasNextNode(TreeNode node, int index)
        {
            return node.Nodes.Count <= index;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private bool TagMatching(TreeNode left, TreeNode right)
        {
            if (left.HasLabel())
            {
                string tag = getTag(left.Text);

                return tag == right.Text;
            }

            return left.Text == right.Text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagValue"></param>
        /// <returns></returns>
        private string getTag(string tagValue)
        {
            var result = tagValue.Split(':');
            return result[0];
        }
    }
}
