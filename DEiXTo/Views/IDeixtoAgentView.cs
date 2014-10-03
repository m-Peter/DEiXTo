﻿using System;
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
        event Action<KeyEventArgs> KeyDownPress;
        event Action<Boolean> AutoFillChanged;
        event Action<Boolean> CrawlingChanged;
        event Action BrowserCompleted;
        event Action<HtmlElement> DocumentMouseOver;
        event Action<HtmlElement> DocumentMouseLeave;
        event Action<int> DOMNodeClick;
        event Action<HtmlElement> CreateWorkingPattern;
        event Action<HtmlElement> CreateAuxiliaryPattern;
        
        string Url { get; }

        void ShowWarningMessage();
        void NavigateTo(string url);
        void NavigateForward();
        void NavigateBack();
        void ApplyVisibilityStateInAutoFill(bool state);
        void ApplyVisibilityStateInCrawling(bool state);
        HtmlDocument GetHTMLDocument();
        void FillDomTree(TreeNode node);
        bool HighlightModeEnabled();
        void AppendTargetUrl(string url);
        void SelectDOMNode(TreeNode node);
        void FillElementInfo(TreeNode node, string outerHtml);
        void ClearElementInfo();
        bool CanAutoScroll();
        void ClearPatternTree();
        void FillPatternTree(TreeNode node);
        void FillAuxiliaryTree(TreeNode node);
        void ExpandPatternTree();
        void ClearAuxiliaryTree();
        void ExpandAuxiliaryTree();
    }
}
