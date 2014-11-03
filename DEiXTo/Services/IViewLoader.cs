using DEiXTo.Views;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IViewLoader
    {
        void LoadMainView();
        void LoadAgentView(string title, IMainView parent);
        void LoadAddLabelView(TreeNode node);
        void LoadRegexBuilderView(TreeNode node);
        void LoadAddSiblingOrderView(TreeNode node);
        void LoadAddAttributeConstraintView(TreeNode node);
    }
}
