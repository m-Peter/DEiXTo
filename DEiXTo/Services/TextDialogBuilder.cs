using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public class TextDialogBuilder : IDialogBuilder
    {
        public void Build(ISaveFileDialog dialog)
        {
            dialog.Filter = "Text Files (*.txt)|";
            dialog.Extension = "txt";
        }
    }
}
