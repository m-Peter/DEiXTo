using DEiXTo.Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace DEiXTo.Services
{
    public class ReadWrapperSettings
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public DeixtoWrapper read(string filename)
        {
            DeixtoWrapper wrapper = new DeixtoWrapper();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;

            using (XmlReader reader = XmlReader.Create(filename, settings))
            {
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
                        createPattern1(doc.ChildNodes, rootNode);
                        wrapper.ExtractionPattern = rootNode;
                    }
                }
            }

            return wrapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="tNode"></param>
        private void createPattern1(XmlNodeList nodes, TreeNode tNode)
        {
            foreach (XmlNode node in nodes)
            {
                TreeNode temp;
                if (node.NodeType == XmlNodeType.Element && node.Name == "Node")
                {
                    if (tNode.Text == String.Empty)
                    {
                        NodeInfo pInfo = new NodeInfo();
                        tNode.Text = node.Attributes["tag"].Value;
                        var isRoot = node.Attributes["IsRoot"];

                        if (isRoot != null && isRoot.Value == "true")
                        {
                            pInfo.IsRoot = true;
                            var font = new Font(FontFamily.GenericSansSerif, 8.25f);
                            tNode.NodeFont = new Font(font, FontStyle.Bold);
                        }

                        var state = node.Attributes["stateIndex"].Value;
                        pInfo.State = getState(state);
                        tNode.Tag = pInfo;

                        tNode.SelectedImageIndex = getStateIndex(state);
                        tNode.ImageIndex = getStateIndex(state);
                        createPattern1(node.ChildNodes, tNode);
                    }
                    else
                    {
                        NodeInfo pInfo = new NodeInfo();
                        temp = tNode.Nodes.Add(node.Attributes["tag"].Value);
                        var isRoot = node.Attributes["IsRoot"];

                        if (isRoot != null && isRoot.Value == "true")
                        {
                            pInfo.IsRoot = true;
                            var font = new Font(FontFamily.GenericSansSerif, 8.25f);
                            temp.NodeFont = new Font(font, FontStyle.Bold);
                        }

                        var state = node.Attributes["stateIndex"].Value;
                        pInfo.State = getState(state);
                        temp.Tag = pInfo;

                        temp.SelectedImageIndex = getStateIndex(state);
                        temp.ImageIndex = getStateIndex(state);
                        createPattern1(node.ChildNodes, temp);
                    }
                }
            }
        }

        private NodeState getState(string state)
        {
            NodeState nState = NodeState.Undefined;

            switch (state)
            {
                case "checked":
                    nState = NodeState.Checked;
                    break;
                case "checked_implied":
                    nState = NodeState.CheckedImplied;
                    break;
                case "checked_source":
                    nState = NodeState.CheckedSource;
                    break;
                case "grayed":
                    nState = NodeState.Grayed;
                    break;
                case "grayed_implied":
                    nState = NodeState.GrayedImplied;
                    break;
                case "dont_care":
                    nState = NodeState.Unchecked;
                    break;
            }

            return nState;
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
