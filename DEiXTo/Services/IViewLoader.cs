using DEiXTo.Views;

namespace DEiXTo.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IViewLoader
    {
        void LoadMainView();
        void LoadAgentView(string title, IMainView parent);
    }
}
