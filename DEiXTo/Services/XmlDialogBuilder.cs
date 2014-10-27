using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public class XmlDialogBuilder : IDialogBuilder
    {
        public void Build(ISaveFileDialog dialog)
        {
            dialog.Filter = "XML Files (*.xml)|";
            dialog.Extension = "xml";
        }
    }
}
