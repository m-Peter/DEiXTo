using DEiXTo.Presenters;

namespace DEiXTo.Views
{
    public interface IAddSiblingOrderView
    {
        AddSiblingOrderPresenter Presenter { get; set; }

        void ApplyVisibilityStateInOrdering(bool checkedState);
        int StartIndex { get; set; }
        int StepValue { get; set; }
        bool CareAboutSiblingOrder { get; set; }
        void Exit();
    }
}
