using System.Windows.Forms;

namespace DEiXTo.Services
{
    public interface ISaveFileDialog
    {
        string Filter { get; set; }
        string Filename { get; set; }
        string Extension { get; set; }
        DialogResult ShowDialog();
        void Reset();
    }
}
