using System.Windows.Forms;

namespace DEiXTo.Models
{
    public class DeixtoWrapper
    {
        // Target URLs
        public string[] TargetUrls { get; set; }
        public string UrlsInputFile { get; set; }
        
        // Output File
        public string OutputFileName { get; set; }
        public Format OutputFileFormat { get; set; }
        public OutputMode OutputFileMode { get; set; }
        
        // Multi/Chained Page Crawling
        public bool MultiPageCrawling { get; set; }
        public int MaxCrawlingDepth { get; set; }
        public string HtmlNextLink { get; set; }
        
        // Ignore HTML Tags
        public string[] IgnoredHtmlTags { get; set; }
        
        // Submit Form
        public bool AutoSubmitForm { get; set; }
        public string FormName { get; set; }
        public string InputName { get; set; }
        public string SearchQuery { get; set; }
        
        // Extraction Pattern
        public ExtractionPattern ExtractionPattern { get; set; }
        
        // Options
        public int NumberOfHits { get; set; }
        public int Delay { get; set; }
        public bool ExtractNativeUrl { get; set; }
    }
}
