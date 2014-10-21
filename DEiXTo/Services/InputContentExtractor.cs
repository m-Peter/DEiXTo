using mshtml;

namespace DEiXTo.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class InputContentExtractor : TagContentExtractor
    {
        public InputContentExtractor(IHTMLElement element)
        {
            _element = element;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ExtractContent()
        {
            return _element.getAttribute("name");
        }
    }
}
