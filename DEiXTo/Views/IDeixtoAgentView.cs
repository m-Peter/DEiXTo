﻿using System;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    /// <summary>
    /// 
    /// </summary>
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
        event Action<TreeNode> MakeWorkingPatternFromSnapshot;
        event Action<TreeNode> DeleteSnapshot;
        event Action<int> ClearTreeViews;
        event Action RebuildDOM;
        event Action ExecuteRule;
        event Action<TreeNode> LevelUpWorkingPattern;
        event Action<TreeNode> LevelDownWorkingPattern;
        event Action<TreeNode, NodeState> NodeStateChanged;
        event Action<bool, TreeNode> OutputResultSelected;
        event Action<TreeNode> AddNewLabel;
        event Action<TreeNode> AddRegex;
        event Action<TreeNode> RemoveLabel;
        event Action<TreeNode> RemoveRegex;
        event Action<FormClosingEventArgs> WindowClosing;
        event Action AddURLToTargetURLs;
        event Action RemoveURLFromTargetURLs;
        event Action<String> TargetURLSelected;
        
        string Url { get; }
        HtmlElement CurrentElement { get; set; }

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
        bool HighlightModeEnabled();
        bool CrawlingEnabled();
        int CrawlingDepth();
        string HtmlLink();
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
        void SetAdjustContextMenuFor(TreeNode node);
        void ShowBrowserMenu();
        bool BrowserContextMenuEnabled();
        string[] IgnoredTags();
        void AttachDocumentEvents();
        void DeleteSnapshotInstance(TreeNode node);
        bool AskUserToClearTreeViews();
        bool AskUserToRemoveURL();
        void ShowNoTagSelectedMessage();
        TreeNode GetWorkingPattern();
        TreeNode GetAuxiliaryInstance();
        TreeNodeCollection GetDOMTreeNodes();
        TreeNodeCollection GetBodyTreeNodes();
        void AddOutputColumn(string columnHeader);
        void AddOutputItem(string[] contents, TreeNode node);
        void SetExtractedResults(int count);
        void WritePageResults(string message);
        void UpdateDocumentUrl();
        void AddWorkingPatternImages(ImageList imageList);
        void SetNodeFont(TreeNode node);
        void ShowCannotDeleteRootMessage();
        void ApplyStateToNode(TreeNode node, int imageIndex);
        void AddLabelToNode(string label, TreeNode node);
        void UnderlineNode(TreeNode node);
    }
}
