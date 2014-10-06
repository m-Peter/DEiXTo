using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public interface IDeixtoAgentView
    {
        event Action BrowseToUrl;
        event Action SimplifyDOMTree;
        event Action<KeyEventArgs> KeyDownPress;
        event Action<Boolean> AutoFillChanged;
        event Action<Boolean> CrawlingChanged;
        event Action BrowserCompleted;
        event Action<HtmlElement> DocumentMouseOver;
        event Action<HtmlElement> DocumentMouseLeave;
        event Action<TreeNode, MouseButtons> DOMNodeClick;
        event Action<TreeNode> CreateWorkingPattern;
        event Action<TreeNode> CreateAuxiliaryPattern;
        event Action<TreeNode> CreateSnapshot;
        event Action<HtmlElement> CreateWorkingPatternFromDocument;
        event Action<TreeNode, MouseButtons> WorkingPatternNodeClick;
        event Action<TreeNode> AuxiliaryPatternNodeClick;
        event HtmlElementEventHandler ShowBrowserContextMenu;
        event Action<HtmlElement> CreateAuxiliaryPatternFromDocument;
        
        string Url { get; }
        HtmlElement CurrentElement { get; set; }

        void ShowWarningMessage();
        void ShowRequestNotFoundMessage();
        void NavigateTo(string url);
        void NavigateForward();
        void NavigateBack();
        void ApplyVisibilityStateInAutoFill(bool state);
        void ApplyVisibilityStateInCrawling(bool state);
        HtmlDocument GetHtmlDocument();
        void FillDomTree(TreeNode node);
        void FillSnapshotTree(TreeNode node);
        bool HighlightModeEnabled();
        void AppendTargetUrl(string url);
        void SelectDOMNode(TreeNode node);
        void FillElementInfo(TreeNode node, string outerHtml);
        void ClearElementInfo();
        bool CanAutoScroll();
        void ClearPatternTree();
        void ClearDOMTree();
        void ClearSnapshotTree();
        void FillPatternTree(TreeNode node);
        void FillAuxiliaryTree(TreeNode node);
        void ExpandPatternTree();
        void ClearAuxiliaryTree();
        void ExpandAuxiliaryTree();
        void SetContextMenuFor(TreeNode node);
        void ShowBrowserMenu();
        bool BrowserContextMenuEnabled();
        string[] IgnoredTags();
    }
}
