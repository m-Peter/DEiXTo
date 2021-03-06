﻿using DEiXTo.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class Executor
    {
        private ExtractionPattern _pattern;
        private TreeNodeCollection _domNodes;
        private List<Result> _results;

        public Executor(ExtractionPattern pattern, TreeNodeCollection domNodes)
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

        public void FindMatches()
        {
            _results = new List<Result>();

            if (_pattern.RootNode.IsRoot())
            {
                var counter = 0;
                _pattern.TrimUncheckedNodes();
                Match(_pattern.RootNode, _domNodes, ref counter);
                return;
            }

            var upperTree = _pattern.GetUpperTree();
            var vRoot = _pattern.FindVirtualRoot();
            MatchSplit(vRoot, _domNodes, upperTree);
        }

        private void Match(TreeNode pattern, TreeNodeCollection nodes,
            ref int counter)
        {
            var result = new Result();
            var start = pattern.GetStartIndex();
            var step = pattern.GetStepValue();

            foreach (TreeNode node in nodes)
            {
                if (Compare(pattern, node, result))
                {
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

        private void MatchSplit(TreeNode pattern, TreeNodeCollection nodes,
            TreeNode upper)
        {
            var result = new Result();

            foreach (TreeNode node in nodes)
            {
                if (Compare(pattern, node, result))
                {
                    var match = CheckUpper(upper, node, result);

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

            var content = right.GetContent();

            AddContentFromInstance(left.GetState(), content, result); ;

            for (int i = 0; i < left.Nodes.Count; i++)
            {
                var nextLeft = left.Nodes[i];
                var nextRight = right.Nodes[i];

                if (!CompareTrees(nextLeft, nextRight, result))
                {
                    return false;
                }
            }

            return true;
        }

        public int Count
        {
            get { return _results.Count; }
        }

        public IEnumerable<Result> ExtractedResults()
        {
            var count = _results.Count;

            for (int i = 0; i < count; i++)
            {
                yield return _results[i];
            }
        }

        private void AddContentFromInstance(NodeState state, string content, Result result)
        {
            if (ContainsContent(state))
            {
                result.AddContent(content);
            }

            if (ContainsSource(state))
            {
                result.AddContent(content);
            }
        }

        private bool ContainsContent(NodeState state)
        {
            if (state == NodeState.Checked || state == NodeState.CheckedImplied)
            {
                return true;
            }

            return false;
        }

        private bool ContainsSource(NodeState state)
        {
            return state == NodeState.CheckedSource;
        }

        private bool Compare(TreeNode left, TreeNode right, Result result)
        {
            if (left.Text != right.Text)
            {
                return false;
            }

            var content = right.GetContent();

            var evaluation = left.EvaluateConstraints(right);
            if (!evaluation.Match)
            {
                return false;
            }
            if (evaluation.Value != string.Empty)
            {
                content = evaluation.Value;
            }

            AddContentFromInstance(left.GetState(), content, result);
            
            var childNodes = left.Nodes.Count;

            for (int i = 0; i < childNodes; i++)
            {
                var nextLeft = left.Nodes[i];

                if (right.Nodes.Count <= i)
                {
                    return false;
                }

                if (nextLeft.IsRequired())
                {
                    var index = i;
                    var nextRight = right.Nodes[index];

                    if (!Compare(nextLeft, nextRight, result))
                    {
                        index += 1;
                        if (right.Nodes.Count > index)
                        {
                            nextRight = right.Nodes[index];
                            return Compare(nextLeft, nextRight, result);
                        }

                        return false;
                    }
                }

                if (nextLeft.IsOptional())
                {
                    var nextRight = right.Nodes[i];

                    Compare(nextLeft, nextRight, result);
                }
            }

            return true;
        }

        private bool TagMatching(TreeNode left, TreeNode right)
        {
            if (left.HasLabel())
            {
                var tag = getTag(left.Text);

                return tag == right.Text;
            }

            return left.Text == right.Text;
        }

        private string getTag(string tagValue)
        {
            var result = tagValue.Split(':');
            return result[0];
        }
    }
}
