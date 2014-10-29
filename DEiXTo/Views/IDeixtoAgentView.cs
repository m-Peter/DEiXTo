using DEiXTo.Models;
using DEiXTo.Presenters;
using System;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDeixtoAgentView
    {
        DeixtoAgentPresenter Presenter { get; set; }

        DeixtoWrapper Wrapper { get; set; } 
        string Url { get; }
        HtmlElement CurrentElement { get; set; }
        bool HighlightModeEnabled { get; }
        bool CrawlingEnabled { get; }
        bool CanAutoScroll { get; }
        bool BrowserContextMenuEnabled { get; }
        string FirstTargetURL { get; }
        string TargetURLsFile { get; set; }
        bool ExtractionPatternSpecified { get; }
        bool TargetURLSpecified { get; }

        string GetDocumentUrl();
        string TargetURLToAdd();
        void ShowEnterURLToAddMessage();
        void SetURLInput(string url);
        void ClearAddURLInput();
        void ShowSelectURLMessage();
        void ClearTargetURLs();
        void ClearExtractedOutputs();
        void FillTextNodeElementInfo(TreeNode node);
        void ShowSpecifyURLMessage();
        void ShowRequestNotFoundMessage();
        void ShowEmptyLinkMessage();
        void ShowInvalidDepthMessage();
        void ShowSpecifyPatternMessage();
        void NavigateTo(string url);
        void NavigateForward();
        void NavigateBack();
        void ApplyVisibilityStateInAutoFill(bool state);
        void ApplyVisibilityStateInCrawling(bool state);
        void FocusAuxiliaryTabPage();
        void FocusOutputTabPage();
        void RemoveTargetURL(string url);
        HtmlDocument GetHtmlDocument();
        void FillDomTree(TreeNode node);
        void FillSnapshotTree(TreeNode node);
        void FillExtractionPattern(TreeNode node);
        void DeletePatternNode(TreeNode node);
        int CrawlingDepth();
        string HtmlLink();
        void ShowSpecifyExtractionPatternMessage();
        void ShowSpecifyTargetURLMessage();
        void AppendTargetUrl(string url);
        void AppendTargetUrls(string[] urls);
        void SelectDOMNode(TreeNode node);
        void FillElementInfo(TreeNode node, string outerHtml);
        void ClearElementInfo();
        void ClearPatternTree();
        void ClearDOMTree();
        void ClearSnapshotTree();
        void FillPatternTree(TreeNode node);
        void FillAuxiliaryTree(TreeNode node);
        void ExpandPatternTree();
        void ExpandExtractionTree();
        void ClearAuxiliaryTree();
        void ExpandAuxiliaryTree();
        void SetContextMenuFor(TreeNode node);
        void SetAdjustContextMenuFor(TreeNode node);
        void ShowBrowserMenu();
        string[] IgnoredTags();
        void AttachDocumentEvents();
        void DeleteSnapshotInstance(TreeNode node);
        bool AskUserToClearTreeViews();
        bool AskUserToRemoveURL();
        void ShowNoTagSelectedMessage();
        TreeNode GetWorkingPattern();
        TreeNode GetAuxiliaryInstance();
        TreeNode GetExtractionPattern();
        TreeNodeCollection GetDOMTreeNodes();
        TreeNodeCollection GetBodyTreeNodes();
        TreeNodeCollection GetPatternTreeNodes();
        void AddOutputColumn(string columnHeader);
        void AddOutputItem(string[] contents, TreeNode node);
        void WritePageResults(string message);
        void UpdateDocumentUrl();
        void AddWorkingPatternImages(ImageList imageList);
        void AddExtractionTreeImages(ImageList imageList);
        void SetNodeFont(TreeNode node);
        void ShowCannotDeleteRootMessage();
        void ApplyStateToNode(TreeNode node, int imageIndex);
        bool OutputFileSpecified { get; }
        string OutputFileName { get; set; }
        Format OutputFileFormat { get; }
    }
}
