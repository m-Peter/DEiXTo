using mshtml;

namespace DEiXTo.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class GenericContentExtractor : TagContentExtractor
    {
        public GenericContentExtractor(IHTMLElement element)
        {
            _element = element;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ExtractContent()
        {
            return _element.innerText;
        }
    }
}
