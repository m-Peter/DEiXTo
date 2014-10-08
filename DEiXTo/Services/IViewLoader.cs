using DEiXTo.Views;

namespace DEiXTo.Services
{
    public interface IViewLoader
    {
        void LoadMainView();
        void LoadAgentView(string title, IMainView parent);
    }
}
