using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class DocumentQuery
    {
        private HtmlDocument _document;

        public DocumentQuery(HtmlDocument document)
        {
            _document = document;
        }

        public HtmlElement GetElementByIndex(int index)
        {
            return _document.All[index];
        }

        public HtmlElement GetHTMLElement()
        {
            return _document.GetElementsByTagName("HTML")[0];
        }

        public HtmlElement GetElementFromPoint(Point point)
        {
            return _document.GetElementFromPoint(point);
        }

        public int CountElements()
        {
            return _document.All.Count;
        }
    }
}
