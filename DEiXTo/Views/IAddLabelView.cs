using System;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public interface IAddLabelView
    {
        event Action AddLabel;
        event Action<KeyEventArgs> KeyDownPress;

        string GetLabelText();
        void ShowInvalidLabelMessage();
        void Exit();
    }
}
