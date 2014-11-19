using DEiXTo.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace DEiXTo.Services
{
    public class ExtractionPatternMapper : IExtractionPatternMapper
    {
        public ExtractionPattern Map(XmlReader reader)
        {
            var rootNode = new TreeNode();
            
            while (reader.Read())
            {
                ReadPatternElement(reader, rootNode);
            }

            return new ExtractionPattern(rootNode);
        }

        private void ReadPatternElement(XmlReader reader, TreeNode rootNode)
        {
            if (IsPatternElement(reader))
            {
                reader.ReadToFollowing("Node");
                XmlDocument doc = new XmlDocument();
                doc.Load(reader.ReadSubtree());
                createPattern(doc.ChildNodes, rootNode);
            }
        }

        private bool IsPatternElement(XmlReader reader)
        {
            return reader.NodeType == XmlNodeType.Element && reader.LocalName == "Pattern";
        }

        private bool IsNodeElement(XmlNode node)
        {
            return node.NodeType == XmlNodeType.Element && node.Name == "Node";
        }

        private string ReadTagAttribute(XmlNode node, NodeInfo pInfo, TreeNode treeNode)
        {
            string tagValue = node.Attributes["tag"].Value;

            if (hasLabel(tagValue))
            {
                pInfo.Label = getLabel(tagValue);
            }

            return tagValue;
        }

        private void ReadIsRootAttribute(XmlNode node, NodeInfo pInfo, TreeNode treeNode)
        {
            var isRoot = node.Attributes["IsRoot"];

            if (isRoot != null && isRoot.Value == "true")
            {
                pInfo.IsRoot = true;
                var font = new Font(FontFamily.GenericSansSerif, 8.25f);
                treeNode.NodeFont = new Font(font, FontStyle.Bold);
            }
        }

        private void ReadRegexAttribute(XmlNode node, NodeInfo pInfo, TreeNode treeNode)
        {
            var regexpr = node.Attributes["regexpr"];

            if (regexpr != null)
            {
                pInfo.Regex = regexpr.Value;
                var font = new Font(FontFamily.GenericSansSerif, 8.25f);
                treeNode.NodeFont = new Font(font, FontStyle.Underline);
            }
        }

        private void ReadStateAttribute(XmlNode node, NodeInfo nInfo, TreeNode treeNode)
        {
            var state = node.Attributes["stateIndex"].Value;

            var st = getState(state);
            if (st == NodeState.Undefined)
            {
                throw new ArgumentException("The value: " + state + " returned: " + st );
            }

            nInfo.State = getState(state);
            treeNode.SelectedImageIndex = getStateIndex(state);
            treeNode.ImageIndex = getStateIndex(state);
        }

        private void ReadCareAboutSOAttribute(XmlNode node, NodeInfo nInfo, TreeNode treeNode)
        {
            var careAboutSO = node.Attributes["CareAboutSO"];
            if (careAboutSO == null)
            {
                return;
            }

            if (careAboutSO.Value == "1")
            {
                nInfo.CareAboutSiblingOrder = true;
                nInfo.SiblingOrderStart = Int32.Parse(node.Attributes["so_start"].Value);
                nInfo.SiblingOrderStep = Int32.Parse(node.Attributes["so_step"].Value);
            }
        }

        private void createPattern(XmlNodeList nodes, TreeNode treeNode)
        {
            foreach (XmlNode node in nodes)
            {
                TreeNode tempNode;

                if (IsNodeElement(node))
                {
                    if (treeNode.Text == String.Empty)
                    {
                        NodeInfo pInfo = new NodeInfo();
                        string tag = ReadTagAttribute(node, pInfo, treeNode);
                        treeNode.Text = tag;

                        ReadIsRootAttribute(node, pInfo, treeNode);
                        ReadRegexAttribute(node, pInfo, treeNode);
                        ReadStateAttribute(node, pInfo, treeNode);
                        ReadCareAboutSOAttribute(node, pInfo, treeNode);
                        treeNode.Tag = pInfo;

                        createPattern(node.ChildNodes, treeNode);
                    }
                    else
                    {
                        NodeInfo pInfo = new NodeInfo();
                        string tag = ReadTagAttribute(node, pInfo, treeNode);
                        tempNode = treeNode.Nodes.Add(tag);
                        
                        ReadIsRootAttribute(node, pInfo, tempNode);
                        ReadRegexAttribute(node, pInfo, tempNode);
                        ReadStateAttribute(node, pInfo, tempNode);
                        ReadCareAboutSOAttribute(node, pInfo, tempNode);

                        tempNode.Tag = pInfo;
                        
                        createPattern(node.ChildNodes, tempNode);
                    }
                }
            }
        }

        private bool hasLabel(string tagValue)
        {
            return tagValue.Contains(":");
        }

        private string getLabel(string tagValue)
        {
            var result = tagValue.Split(':');
            return result[1];
        }

        private NodeState getState(string state)
        {
            switch (state)
            {
                case "checked":
                    return NodeState.Checked;
                case "checked_implied":
                    return NodeState.CheckedImplied;
                case "checked_source":
                    return NodeState.CheckedSource;
                case "grayed":
                    return NodeState.Grayed;
                case "grayed_implied":
                    return NodeState.GrayedImplied;
                case "dont_care":
                    return NodeState.Unchecked;
            }

            return NodeState.Undefined;
        }

        private int getStateIndex(string state)
        {
            int index = -1;

            switch (state)
            {
                case "checked":
                    index = 0;
                    break;
                case "checked_implied":
                    index = 1;
                    break;
                case "checked_source":
                    index = 2;
                    break;
                case "grayed":
                    index = 3;
                    break;
                case "grayed_implied":
                    index = 4;
                    break;
                case "dont_care":
                    index = 5;
                    break;
            }

            return index;
        }
    }
}
