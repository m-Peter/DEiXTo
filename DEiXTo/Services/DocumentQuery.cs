using System.Drawing;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    /// <summary>
    /// This class wraps a HtmlDocument and provides some useful query methods
    /// that operate on it.
    /// </summary>
    public class DocumentQuery
    {
        private HtmlDocument _htmlDocument;

        public DocumentQuery(HtmlDocument document)
        {
            _htmlDocument = document;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public HtmlElement GetElementByIndex(int index)
        {
            return _htmlDocument.All[index];
        }

        /// <summary>
        /// Retrieve the HTML HtmlElement of the underlying HtmlDocument.
        /// </summary>
        /// <returns></returns>
        public HtmlElement GetHtmlElement()
        {
            return _htmlDocument.GetElementsByTagName("HTML")[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public HtmlElement GetElementFromPoint(Point point)
        {
            return _htmlDocument.GetElementFromPoint(point);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int CountElements()
        {
            return _htmlDocument.All.Count;
        }
    }
}
