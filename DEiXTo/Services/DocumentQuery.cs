using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public HtmlElement GetElementByIndex(int index)
        {
            return _htmlDocument.All[index];
        }

        public HtmlElement GetHtmlElement()
        {
            return _htmlDocument.GetElementsByTagName("HTML")[0];
        }

        public HtmlElement GetElementFromPoint(Point point)
        {
            return _htmlDocument.GetElementFromPoint(point);
        }

        public int CountElements()
        {
            return _htmlDocument.All.Count;
        }
    }
}
