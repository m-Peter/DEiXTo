using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class SaveFileDialogWrapper : ISaveFileDialog, IDisposable
    {
        private readonly SaveFileDialog _dialog;

        public SaveFileDialogWrapper()
        {
            _dialog = new SaveFileDialog();
            _dialog.AddExtension = true;
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

        public string Extension
        {
            get
            {
                return _dialog.DefaultExt;
            }
            set
            {
                _dialog.DefaultExt = value;
            }
        }

        public DialogResult ShowDialog()
        {
            return _dialog.ShowDialog();
        }

        public void Reset()
        {
            _dialog.Reset();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dialog.Dispose();
            }
        }
    }
}
