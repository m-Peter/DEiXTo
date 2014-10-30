using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public interface IOpenFileDialog
    {
        string Filter { get; set; }
        string Filename { get; set; }
        DialogResult ShowDialog();
        void Reset();
    }
}
