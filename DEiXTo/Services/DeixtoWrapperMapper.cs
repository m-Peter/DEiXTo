using DEiXTo.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace DEiXTo.Services
{
    public class DeixtoWrapperMapper : IDeixtoWrapperMapper
    {
        public DeixtoWrapper Map(XmlReader reader)
        {
            DeixtoWrapper wrapper = new DeixtoWrapper();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "InputFile")
                {
                    reader.MoveToAttribute("Filename");
                    wrapper.InputFile = reader.Value;
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "TargetUrls")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(reader.ReadSubtree());

                    int i = 0;
                    var nodes = doc.ChildNodes[0];
                    string[] urls = new string[nodes.ChildNodes.Count];

                    foreach (XmlNode node in nodes.ChildNodes)
                    {
                        if (node.NodeType == XmlNodeType.Element && node.LocalName == "URL")
                        {
                            urls[i] = node.Attributes["Address"].Value;
                            i++;
                        }
                    }

                    wrapper.TargetUrls = urls;
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "IgnoredTagsList")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(reader.ReadSubtree());

                    int i = 0;
                    var nodes = doc.ChildNodes[0];
                    string[] tags = new string[nodes.ChildNodes.Count];

                    foreach (XmlNode node in nodes.ChildNodes)
                    {
                        if (node.NodeType == XmlNodeType.Element && node.LocalName == "IgnoredTag")
                        {
                            tags[i] = node.Attributes["Label"].Value;
                            i++;
                        }
                    }

                    wrapper.IgnoredTags = tags;
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "MultiplePage")
                {
                    reader.MoveToAttribute("Enabled");
                    bool enabled = reader.ReadContentAsBoolean();
                    reader.MoveToAttribute("ContainsText");
                    string containsText = reader.Value;
                    reader.MoveToAttribute("MaxCrawlDepth");
                    int crawlDepth = reader.ReadContentAsInt();
                    wrapper.MultiPageCrawling = enabled;
                    wrapper.HtmlNextLink = containsText;
                    wrapper.MaxCrawlingDepth = crawlDepth;
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "OutputFile")
                {
                    reader.MoveToAttribute("Filename");
                    string output_filename = reader.Value;
                    reader.MoveToAttribute("Format");
                    var format_value = reader.Value;

                    OutputFormat format = new OutputFormat();
                    if (format_value == "XML")
                    {
                        format = new OutputFormat(Format.XML, format_value);
                    }
                    else if (format_value == "TabDelimited")
                    {
                        format = new OutputFormat(Format.Text, format_value);
                    }
                    else if (format_value == "RSS")
                    {
                        format = new OutputFormat(Format.RSS, format_value);
                    }

                    reader.MoveToAttribute("FileMode");
                    var mode = reader.Value;

                    wrapper.OutputFileName = output_filename;
                    wrapper.OutputFormat = format.Format;
                    wrapper.OutputMode = mode == "Append" ? OutputMode.Append : OutputMode.Overwrite;
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "MaxHits")
                {
                    reader.MoveToAttribute("Value");
                    int maxHits = reader.ReadContentAsInt();
                    wrapper.NumberOfHits = maxHits;
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "ExtractPageURL")
                {
                    reader.MoveToAttribute("Enabled");
                    bool enabled = reader.ReadContentAsBoolean();
                    wrapper.ExtractNativeUrl = enabled;
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "SubmitForm")
                {
                    reader.MoveToAttribute("Enabled");
                    bool enabled = reader.ReadContentAsBoolean();
                    reader.MoveToAttribute("FormName");
                    string formName = reader.ReadContentAsString();
                    reader.MoveToAttribute("InputName");
                    string inputName = reader.ReadContentAsString();
                    reader.MoveToAttribute("Term");
                    string term = reader.ReadContentAsString();
                    wrapper.AutoFill = enabled;
                    wrapper.FormName = formName;
                    wrapper.FormInputName = inputName;
                    wrapper.FormTerm = term;
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "ExtractionPattern")
                {
                    var rootNode = new TreeNode();
                    reader.ReadToFollowing("Node");
                    XmlDocument doc = new XmlDocument();
                    doc.Load(reader.ReadSubtree());
                    createPattern(doc.ChildNodes, rootNode);
                    wrapper.ExtractionPattern = new ExtractionPattern(rootNode);
                }
            }

            return wrapper;
        }

        private bool NonNodeElement(XmlNode node)
        {
            return (node.NodeType != XmlNodeType.Element) || (node.Name != "Node");
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

        private bool hasLabel(string tagValue)
        {
            return tagValue.Contains(":");
        }

        private string getLabel(string tagValue)
        {
            var result = tagValue.Split(':');
            return result[1];
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

            nInfo.State = NodeStateTranslator.StringToState(state);
            int imageIndex = NodeStateTranslator.StringToImageIndex(state);
            treeNode.SelectedImageIndex = imageIndex;
            treeNode.ImageIndex = imageIndex;
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

        private void createPattern(XmlNodeList nodes, TreeNode treeNode)
        {
            foreach (XmlNode node in nodes)
            {
                if (NonNodeElement(node))
                {
                    continue;
                }

                NodeInfo pInfo = new NodeInfo();
                string tag = ReadTagAttribute(node, pInfo, treeNode);

                if (treeNode.Text == String.Empty)
                {
                    treeNode.Text = tag;

                    ReadIsRootAttribute(node, pInfo, treeNode);
                    ReadRegexAttribute(node, pInfo, treeNode);
                    ReadStateAttribute(node, pInfo, treeNode);
                    ReadCareAboutSOAttribute(node, pInfo, treeNode);

                    treeNode.Tag = pInfo;

                    createPattern(node.ChildNodes, treeNode);
                    return;
                }

                TreeNode tempNode;
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
