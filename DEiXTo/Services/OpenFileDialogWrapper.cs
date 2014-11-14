using System;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class OpenFileDialogWrapper : IOpenFileDialog, IDisposable
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
