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
            findRoot(_rootNode, ref vRoot);

            return vRoot;
        }

        public TreeNode GetUpperTree()
        {
            var upperNode = new TreeNode(_rootNode.Text);
            upperNode.Tag = _rootNode.Tag;
            var leafNode = new TreeNode();

            buildUpperTree(_rootNode, upperNode, ref leafNode);

            return leafNode;
        }

        public int CountOutputVariables()
        {
            var outputVariables = 0;

            countVariables(_rootNode, ref outputVariables);

            return outputVariables;
        }

        public List<string> OutputVariableLabels()
        {
            var labels = new List<string>();
            var counter = 1;

            collectVariableLabels(_rootNode, labels, ref counter);

            return labels;
        }

        public void TrimUncheckedNodes()
        {
            filterUncheckedNodes(_rootNode);
        }

        private void findRoot(TreeNode node, ref TreeNode vRoot)
        {
            if (node.IsRoot())
            {
                vRoot = node.GetClone();
                return;
            }

            foreach (TreeNode n in node.Nodes)
            {
                findRoot(n, ref vRoot);
            }
        }

        private void buildUpperTree(TreeNode node, TreeNode root, ref TreeNode leafNode)
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

            foreach (TreeNode n in node.Nodes)
            {
                buildUpperTree(n, newNode, ref leafNode);
            }
        }

        private void countVariables(TreeNode node, ref int counter)
        {
            if (node.IsOutputVariable())
            {
                counter += 1;
            }

            foreach (TreeNode n in node.Nodes)
            {
                countVariables(n, ref counter);
            }
        }

        private void collectVariableLabels(TreeNode node, List<string> labels, ref int counter)
        {
            if (node.IsOutputVariable())
            {
                AddNodeLabel(node, labels, counter);
                counter++;
            }

            foreach (TreeNode n in node.Nodes)
            {
                collectVariableLabels(n, labels, ref counter);
            }
        }

        private void AddNodeLabel(TreeNode node, List<string> labels, int counter)
        {
            if (node.HasLabel())
            {
                labels.Add(node.GetLabel());
                return;
            }

            labels.Add("VAR" + counter);
        }

        private void filterUncheckedNodes(TreeNode node)
        {
            var parent = node.Parent;

            if (node.IsSkipped())
            {
                parent.Nodes.Remove(node);
            }

            foreach (TreeNode n in node.Nodes)
            {
                filterUncheckedNodes(n);
            }
        }
    }
}
