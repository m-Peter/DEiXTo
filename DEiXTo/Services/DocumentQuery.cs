using mshtml;
using System.Drawing;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class DocumentQuery
    {
        private HtmlDocument _htmlDocument;
        private HtmlElementCollection _elements;

        public DocumentQuery(HtmlDocument document)
        {
            _htmlDocument = document;
            _elements = _htmlDocument.All;
        }

        public HtmlElement GetElementByIndex(int index)
        {
            return _elements[index];
        }

        public void FillInput(HtmlElement input, string term)
        {
            var domInput = input.DomElement as IHTMLInputElement;
            domInput.value = term;
        }

        public void SubmitForm(HtmlElement form)
        {
            form.InvokeMember("submit");
        }

        public HtmlElement GetForm(string formName)
        {
            var forms = _htmlDocument.Forms;

            foreach (HtmlElement form in forms)
            {
                if (form.Name == formName)
                {
                    return form;
                }
            }

            return forms[0];
        }

        public HtmlElement GetInputFor(HtmlElement form, string inputName)
        {
            foreach (HtmlElement input in form.All)
            {
                if (input.Name == inputName)
                {
                    return input;
                }
            }

            return form.All[0];
        }

        public HtmlElement GetHtmlElement()
        {
            return _htmlDocument.GetElementsByTagName("HTML")[0];
        }

        public HtmlElement GetElementFromPoint(Point point)
        {
            return _htmlDocument.GetElementFromPoint(point);
        }

        public HtmlElement GetLinkToFollow(string mylink)
        {
            var links = _htmlDocument.Links;
            HtmlElement link = null;

            foreach (HtmlElement element in links)
            {
                if (Matches(element, mylink))
                {
                    link = element;
                }
            }

            return link;
        }

        private bool Matches(HtmlElement element, string link)
        {
            if (element.InnerText != null && element.InnerText.Equals(link))
            {
                return true;
            }

            return false;
        }

        public int CountElements()
        {
            return _htmlDocument.All.Count;
        }
    }
}
