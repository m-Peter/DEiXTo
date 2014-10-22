using DEiXTo.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DEiXTo.Services
{
    public class ReadExtractionPattern
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public TreeNode read(string filename)
        {
            var rootNode = new TreeNode();

            using (XmlReader reader = XmlReader.Create(filename))
            {
                while (reader.Read())
                {
                    ReadPatternElement(reader, rootNode);
                }
            }

            return rootNode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="rootNode"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private bool IsPatternElement(XmlReader reader)
        {
            return reader.NodeType == XmlNodeType.Element && reader.LocalName == "Pattern";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool IsNodeElement(XmlNode node)
        {
            return node.NodeType == XmlNodeType.Element && node.Name == "Node";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="pInfo"></param>
        private string ReadTagAttribute(XmlNode node, NodeInfo pInfo, TreeNode treeNode)
        {
            string tagValue = node.Attributes["tag"].Value;

            if (hasLabel(tagValue))
            {
                pInfo.Label = getLabel(tagValue);
            }

            return tagValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="pInfo"></param>
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

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="treeNode"></param>
        private void ReadStateAttribute(XmlNode node, TreeNode treeNode)
        {
            var state = node.Attributes["stateIndex"].Value;

            treeNode.SelectedImageIndex = getStateIndex(state);
            treeNode.ImageIndex = getStateIndex(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="tNode"></param>
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

                        treeNode.Tag = pInfo;

                        ReadStateAttribute(node, treeNode);

                        createPattern(node.ChildNodes, treeNode);
                    }
                    else
                    {
                        NodeInfo pInfo = new NodeInfo();

                        string tag = ReadTagAttribute(node, pInfo, treeNode);
                        tempNode = treeNode.Nodes.Add(tag);

                        ReadIsRootAttribute(node, pInfo, tempNode);

                        ReadRegexAttribute(node, pInfo, tempNode);

                        tempNode.Tag = pInfo;

                        ReadStateAttribute(node, tempNode);

                        createPattern(node.ChildNodes, tempNode);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagValue"></param>
        /// <returns></returns>
        private bool hasLabel(string tagValue)
        {
            return tagValue.Contains(":");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagValue"></param>
        /// <returns></returns>
        private string getLabel(string tagValue)
        {
            var result = tagValue.Split(':');
            return result[1];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
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
