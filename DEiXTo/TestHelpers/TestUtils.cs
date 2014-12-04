using mshtml;
using System.Windows.Forms;

namespace DEiXTo.TestHelpers
{
    public static class TestUtils
    {
        private static WebBrowser browser = new WebBrowser();

        public static HtmlDocument CreateHtmlDocument()
        {
            browser.DocumentText = "some text";
            browser.Show();

            return browser.Document;
        }

        public static HtmlElement CreateHtmlElement()
        {
            var doc = CreateHtmlDocument();
            doc.Write("<li></li>");
            var element = doc.GetElementsByTagName("li")[0];

            return element;
        }

        public static HtmlElement CreateHtmlLinkElement()
        {
            var doc = CreateHtmlDocument();
            doc.Write("<a href='http://www.google.gr/?page=1'>Google</a>");
            var element = doc.GetElementsByTagName("a")[0];

            return element;
        }

        public static IHTMLElement CreateGenericElement()
        {
            var doc = CreateHtmlDocument();
            doc.Write("<p>Some text in here</p>");
            var element = doc.GetElementsByTagName("p")[0];
            var domElement = (IHTMLElement)element.DomElement;

            return domElement;
        }

        public static IHTMLElement CreateInputElement()
        {
            var doc = CreateHtmlDocument();
            doc.Write("<input name='s' type='text' />");
            var element = doc.GetElementsByTagName("input")[0];
            var domElement = (IHTMLElement)element.DomElement;

            return domElement;
        }

        public static IHTMLElement CreateFormElement()
        {
            var doc = CreateHtmlDocument();
            doc.Write("<form method='get' action='http://www.sitepoint.com'></form>");
            var element = doc.Forms[0];
            var domElement = (IHTMLElement)element.DomElement;

            return domElement;
        }

        public static IHTMLElement CreateImageElement()
        {
            var doc = CreateHtmlDocument();
            doc.Write("<img src='/img/main/thumb-small.png' alt='Image' />");
            var element = doc.GetElementsByTagName("img")[0];
            var domElement = (IHTMLElement)element.DomElement;

            return domElement;
        }

        public static IHTMLElement CreateLinkElement()
        {
            var doc = CreateHtmlDocument();
            doc.Write("<a href='http://www.google.gr/'>Google</a>");
            var element = doc.GetElementsByTagName("a")[0];
            var domElement = (IHTMLElement)element.DomElement;

            return domElement;
        }

        public static HtmlElement CreateUlElement()
        {
            var doc = CreateHtmlDocument();
            doc.Write("<ul><li>hey</li></ul>");
            var elements = doc.GetElementsByTagName("ul");
            var elem = elements[0];

            return elem;
        }
    }
}
