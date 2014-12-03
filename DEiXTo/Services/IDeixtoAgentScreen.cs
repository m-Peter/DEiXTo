using DEiXTo.Models;
using System.Drawing;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public interface IDeixtoAgentScreen
    {
        HtmlElement GetElementFromNode(TreeNode node);
        HtmlElement GetElementFromPosition(Point position);
        TreeNode GetDomNode(TreeNode node);
        TreeNode ScanDomTree(TreeNode pattern);
        TreeNode GetNodeFromElement(HtmlElement element);
        TreeNode LoadExtractionPattern(string filename);
        IOpenFileDialog GetOpenFileDialog(string filter);
        ISaveFileDialog GetSaveFileDialog(string filter, string extension);
        ISaveFileDialog GetSaveFileDialog(Format format);
        string[] LoadUrlsFromFile(string filename);
        void WriteExtractedRecords(string filename);
        void SaveExtractionPattern(string filename, TreeNode node);
        void HighlightElement(HtmlElement element);
        void RemoveHighlighting(HtmlElement element);
        void ClearStyling();
        void CreateDocument(HtmlDocument document);
        TreeNode BuildSimplifiedDOM(string[] ignoredTags);
        TreeNode BuildDom();
        ImageList LoadStateImages();
        IExtraction Execute(TreeNode pattern, TreeNodeCollection domNodes);
        void SaveWrapper(DeixtoWrapper wrapper, TreeNodeCollection nodes, string filename);
        DeixtoWrapper LoadWrapper(string filename);
        void SubmitForm(string formName, string inputName, string term);
        HtmlElement GetLinkToFollow(string link);
    }
}
