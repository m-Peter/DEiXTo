using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class OpenFileDialogWrapper : IOpenFileDialog
    {
        private readonly OpenFileDialog _dialog;

        public OpenFileDialogWrapper()
        {
            _dialog = new OpenFileDialog();
        }

        public string Filter
        {
            get
            {
                return _dialog.Filter;
            }
            set
            {
                _dialog.Filter = value;
            }
        }

        public string Filename
        {
            get
            {
                return _dialog.FileName;
            }
            set
            {
                _dialog.FileName = value;
            }
        }

        public DialogResult ShowDialog()
        {
            return _dialog.ShowDialog();
        }
    }
}
