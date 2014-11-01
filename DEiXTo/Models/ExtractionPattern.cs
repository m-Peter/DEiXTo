using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void CollectVariableLabels(TreeNodeCollection nodes, List<string> labels)
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
    }
}
