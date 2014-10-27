using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public class RssDialogBuilder : IDialogBuilder
    {
        public void Build(ISaveFileDialog dialog)
        {
            dialog.Filter = "RSS Files (*.rss)|";
            dialog.Extension = "rss";
        }
    }
}
