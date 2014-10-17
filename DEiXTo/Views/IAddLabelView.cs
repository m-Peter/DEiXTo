using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
