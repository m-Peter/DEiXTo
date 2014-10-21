using mshtml;

namespace DEiXTo.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageContentExtractor : TagContentExtractor
    {
        public ImageContentExtractor(IHTMLElement element)
        {
            _element = element;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ExtractContent()
        {
            return _element.getAttribute("src");
        }
    }
}
