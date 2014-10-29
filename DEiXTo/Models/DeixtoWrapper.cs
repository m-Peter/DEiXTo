using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Models
{
    public class DeixtoWrapper
    {
        public string InputFile { get; set; }
        public string[] TargetUrls { get; set; }
        public string OutputFileName { get; set; }
        public Format OutputFormat { get; set; }
        public bool MultiPageCrawling { get; set; }
        public int MaxCrawlingDepth { get; set; }
        public string HtmlNextLink { get; set; }
        public string[] IgnoredTags { get; set; }
        public bool AutoFill { get; set; }
        public string FormName { get; set; }
        public string FormInputName { get; set; }
        public string FormTerm { get; set; }
        public OutputMode OutputMode { get; set; }
        public TreeNode ExtractionPattern { get; set; }
        public int NumberOfHits { get; set; }
        public int Delay { get; set; }
        public bool ExtractNativeUrl { get; set; }
    }

    public enum OutputMode
    {
        Overwrite,
        Append
    }
}
