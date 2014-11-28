using DEiXTo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mshtml;
using System.Windows.Forms;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class DOMBuilderTests
    {
        private WebBrowser browser = new WebBrowser();
        private IDOMBuilder _builder;
        private HtmlElement _htmlTag;
        private DOMTree _domTree;

        [TestInitialize]
        public void SetUp()
        {
            var document = CreateDocument();
            _htmlTag = document.GetElementsByTagName("HTML")[0];
            _builder = new DOMBuilder(_htmlTag);
            _domTree = _builder.Build();
        }

        [TestMethod]
        public void TestBuildDomTree()
        {
            Assert.IsInstanceOfType(_domTree, typeof(DOMTree));
        }

        [TestMethod]
        public void TestContainsHtmlNode()
        {
            var html = _domTree.RootNode;
            Assert.AreEqual("HTML", html.Text);
            Assert.AreEqual(3, html.ImageIndex);
            Assert.AreEqual(3, html.SelectedImageIndex);
            Assert.IsNotNull(html.SourceIndex());
            Assert.AreEqual("HTML[1]", html.GetPath());
            Assert.IsNotNull(html.GetContent());
            Assert.AreEqual(NodeState.Grayed, html.GetState());
            Assert.IsNotNull(html.GetSource());
        }

        [TestMethod]
        public void TestDOMTreeStructure()
        {
            // Arrange
            var html = new TreeNode("HTML");
            var head = new TreeNode("HEAD");
            html.AddNode(head);
            var title = new TreeNode("TITLE");
            head.AddNode(title);
            var text = new TreeNode("TEXT");
            var body = new TreeNode("BODY");
            html.AddNode(body);
            var div1 = new TreeNode("DIV");
            body.AddNode(div1);
            var a1 = new TreeNode("A");
            div1.AddNode(a1);
            a1.AddNode(text);
            var a2 = new TreeNode("A");
            div1.AddNode(a2);
            a2.AddNode(text);
            var a3 = new TreeNode("A");
            div1.AddNode(a3);
            a3.AddNode(text);
            var div2 = new TreeNode("DIV");
            body.AddNode(div2);
            var a4 = new TreeNode("A");
            a4.AddNode(text);
            div2.AddNode(a4);
            var div3 = new TreeNode("DIV");
            body.AddNode(div3);
            var form = new TreeNode("FORM");
            div3.AddNode(form);
            var input = new TreeNode("INPUT");
            form.AddNode(input);

            // Act
            var result = CompareTrees(html, _domTree.RootNode);
            
            // Assert
            Assert.IsTrue(result);
        }

        private bool CompareTrees(TreeNode left, TreeNode right)
        {
            if (left.Text != right.Text)
            {
                return false;
            }

            int childNodes = left.Nodes.Count;

            for (int i = 0; i < childNodes; i++)
            {
                var nextleft = left.Nodes[i];
                var nextRight = right.Nodes[i];

                if (!CompareTrees(nextleft, nextRight))
                {
                    return false;
                }
            }

            return true;
        }

        private HtmlDocument CreateDocument()
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
    }
}
