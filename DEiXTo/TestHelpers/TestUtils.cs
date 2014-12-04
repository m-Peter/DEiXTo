using mshtml;
using System.Windows.Forms;

namespace DEiXTo.TestHelpers
{
    public static class TestUtils
    {
        private static WebBrowser browser = new WebBrowser();

        public static HtmlDocument CreateSimplifiedDocument()
        {
            browser.DocumentText = "some text";
            browser.Show();

            var doc = browser.Document;
            string source = @"
            <!DOCTYPE HTML>
            <html>
                <head>
                    <title>My Web Page</title>
                </head>
                <body>
                    <p>
                        This <b>is</b> some <b>text</b> that <b>is</b> made <b>bold</b> for <b>testing</b> <b>purposes</b><br/>
                    </p>
                </body>
            </html>";
            doc.Write(source);

            return browser.Document;
        }

        public static HtmlDocument CreateFilledHtmlDocument()
        {
            browser.DocumentText = "some text";
            browser.Show();

            var doc = browser.Document;
            string source = @"
            <html>
                <head>
                    <title>My Web Page</title>
                </head>
                <body>
                    <div id='links'>
                        <nav id='nav'>
						    <a href='/projects/'>Projects</a>
						    <a href='/blog/'>Blog</a>
						    <a href='/notes/'>Drafts &amp; Notes</a>
						</nav>
                    </div>
                    <div id='main'>
                        <a href='/next_page/'>Next</a>
                    </div>
                    <div id='search'>
                        <form method='get' action='http://www.search.com' name='search-form'>
                            <input name='s' type='text' />
                        </form
                    </div>
                </body>
            </html>";
            doc.Write(source);

            return browser.Document;
        }

        public static HtmlDocument CreateDomHtmlDocument()
        {
            browser = new WebBrowser();
            browser.DocumentText = "some text";
            browser.Show();

            var doc = browser.Document;
            string source = @"
            <!DOCTYPE HTML>
            <html>
                <head>
                    <title>My Web Page</title>
                </head>
                <body>
                    <div id='links'>
						<a href='/projects/'>Projects</a>
						<a href='/blog/'>Blog</a>
						<a href='/notes/'>Drafts &amp; Notes</a>
                    </div>
                    <div id='main'>
                        <a href='/next_page/'>Next</a>
                    </div>
                    <div id='search'>
                        <form method='get' action='http://www.search.com' name='search-form'>
                            <input name='s' type='text' />
                        </form>
                    </div>
                </body>
            </html>";
            doc.Write(source);

            return browser.Document;
        }

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

        public static HtmlElement CreateParagraphElement()
        {
            browser = new WebBrowser();
            browser.DocumentText = "some text";
            browser.Show();
            var doc = CreateHtmlDocument();
            doc.Write("<p>Some text in here</p>");
            var element = doc.GetElementsByTagName("p")[0];

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
