using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public interface IDeixtoAgentScreen
    {
        HtmlElement GetElementFromNode(TreeNode node);
        HtmlElement GetElementFromPosition(Point position);
        TreeNode GetDomNode(TreeNode node);
        TreeNode GetNodeFromElement(HtmlElement element);
        TreeNode LoadExtractionPattern(string filename);
        IOpenFileDialog GetOpenFileDialog(string filter);
        ISaveFileDialog GetSaveFileDialog(string filter, string extension);
        string[] LoadUrlsFromFile(string filename);
        void WriteExtractedRecords(string filename);
        void SaveExtractionPattern(string filename, TreeNodeCollection nodes);
        void HighlightElement(HtmlElement element);
        void RemoveHighlighting(HtmlElement element);
        void ClearStyling();
        void CreateDocument(HtmlDocument document);
        TreeNode BuildSimplifiedDOM(string[] ignoredTags);
        TreeNode BuildDom();
    }
}
