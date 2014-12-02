using DEiXTo.Models;
using DEiXTo.Presenters;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public interface IDeixtoAgentView
    {
        DeixtoAgentPresenter Presenter { get; set; }

        void ShowHighlightContextMenu();
        void DisableHighlighting();
        void EnableHighlighting();
        void ClearAttributes();
        void LoadNodeAttributes(List<TagAttribute> attributes);
        string[] IgnoredTags { get; set; }
        string[] TargetUrls { get; set; }
        OutputMode OutputMode { get; set; }
        Format OutputFormat { get; set; }
        string OutputFileName { get; set; }
        int NumberOfHits { get; set; }
        bool MultiPageCrawling { get; set; }
        int MaxCrawlingDepth { get; set; }
        string InputFile { get; set; }
        string FormTerm { get; set; }
        string FormInputName { get; set; }
        string FormName { get; set; }
        bool ExtractNativeUrl { get; set; }
        TreeNode ExtractionPattern { get; set; }
        int Delay { get; set; }
        bool AutoFill { get; set; }
        string HtmlNextLink { get; set; }

        string Url { get; }
        HtmlElement CurrentElement { get; set; }
        bool HighlightModeEnabled { get; set; }
        bool CrawlingEnabled { get; set; }
        bool CanAutoScroll { get; set; }
        bool BrowserContextMenuEnabled { get; }
        string FirstTargetURL { get; }
        string TargetURLsFile { get; set; }
        bool ExtractionPatternSpecified { get; }
        bool TargetURLSpecified { get; }

        string GetDocumentUrl();
        string TargetURLToAdd();
        void ShowEnterURLToAddMessage();
        void ShowNavigationErrorMessage(string message);
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
        void ShowSpecifyInputSourceMessage();
        void ShowSelectOneInputSourceMessage();
        void ShowSpecifyTargetURLMessage();
        void AppendTargetUrl(string url);
        void AppendTargetUrls(string[] urls);
        void SelectDOMNode(TreeNode node);
        void FillElementInfo(TreeNode node, string outerHtml);
        void ClearElementInfo();
        void ClearPatternTree();
        void ClearExtractionPattern();
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
        TreeNodeCollection GetExtractionPatternNodes();
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
        Format OutputFileFormat { get; }
    }
}
