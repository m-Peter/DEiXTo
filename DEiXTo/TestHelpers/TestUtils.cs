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

        public static HtmlElement CreateLinkElement()
        {
            var doc = CreateHtmlDocument();
            doc.Write("<a href='http://www.google.gr/?page=1'>Google</a>");
            var element = doc.GetElementsByTagName("a")[0];

            return element;
        }

        public static HtmlElement CreateUlElement()
        {
            //WebBrowser browser = new WebBrowser();
            //browser.DocumentText = "some text";
            //browser.Show();
            var doc = CreateHtmlDocument();
            doc.Write("<ul><li>hey</li></ul>");
            var elements = doc.GetElementsByTagName("ul");
            var elem = elements[0];

            return elem;
        }
    }
}
