using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public interface IDeixtoAgentScreen
    {
        HtmlElement GetElementFromNode(TreeNode node);
        IOpenFileDialog GetTextFileDialog();
        ISaveFileDialog GetTextSaveFileDialog();
        string[] LoadUrlsFromFile(string filename);
        void WriteExtractedRecords(string filename);
    }
}
