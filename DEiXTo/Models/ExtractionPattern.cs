using System.Collections.Generic;
using System.Windows.Forms;
using DEiXTo.Services;

namespace DEiXTo.Models
{
    public class ExtractionPattern
    {
        private TreeNode _rootNode;

        public ExtractionPattern(TreeNode rootNode)
        {
            _rootNode = rootNode;
        }

        public TreeNode RootNode
        {
            get { return _rootNode; }
        }

        public TreeNode FindVirtualRoot()
        {
            var vRoot = new TreeNode();
            FindRoot(_rootNode.Nodes, ref vRoot);
            
            return vRoot;
        }

        private void FindRoot(TreeNodeCollection nodes, ref TreeNode root)
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

        public TreeNode GetUpperTree()
        {
            var upperNode = new TreeNode(_rootNode.Text);
            upperNode.Tag = _rootNode.Tag;
            var leafNode = new TreeNode();

            BuiltUpperTree(_rootNode.Nodes, upperNode, ref leafNode);

            return leafNode;
        }

        private void BuiltUpperTree(TreeNodeCollection nodes, TreeNode root,
            ref TreeNode leafNode)
        {
            foreach (TreeNode node in nodes)
            {
                var newNode = new TreeNode(node.Text);
                newNode.Tag = node.Tag;
                root.Nodes.Add(newNode);

                if (node.IsRoot())
                {
                    leafNode.Text = newNode.Text;
                    leafNode = newNode;
                    return;
                }

                BuiltUpperTree(node.Nodes, newNode, ref leafNode);
            }
        }

        public int CountOutputVariables()
        {
            int outputVariables = 0;

            if (_rootNode.IsOutputVariable())
            {
                outputVariables += 1;
            }

            countVariables(_rootNode.Nodes, ref outputVariables);

            return outputVariables;
        }

        private void countVariables(TreeNodeCollection nodes, ref int counter)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.IsOutputVariable())
                {
                    counter += 1;
                }

                countVariables(node.Nodes, ref counter);
            }
        }

        public List<string> OutputVariableLabels()
        {
            List<string> labels = new List<string>();

            if (_rootNode.HasLabel())
            {
                labels.Add(_rootNode.GetLabel());
            }

            CollectVariableLabels(_rootNode.Nodes, labels);

            return labels;
        }

        private void CollectVariableLabels(TreeNodeCollection nodes,
            List<string> labels)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.HasLabel())
                {
                    labels.Add(node.GetLabel());
                }

                CollectVariableLabels(node.Nodes, labels);
            }
        }

        public void TrimUncheckedNodes()
        {
            FilterUncheckedNodes(_rootNode.Nodes);
        }

        private void FilterUncheckedNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                TreeNode parent = node.Parent;

                if (node.IsSkipped())
                {
                    parent.Nodes.Remove(node);
                }

                FilterUncheckedNodes(node.Nodes);
            }
        }
    }
}
