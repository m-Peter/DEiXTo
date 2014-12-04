using DEiXTo.Models;
using DEiXTo.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class SimplifiedDOMBuilderTests
    {
        private IDOMBuilder _builder;
        private HtmlElement _htmlTag;

        [TestInitialize]
        public void SetUp()
        {
            var document = TestUtils.CreateSimplifiedDocument();
            _htmlTag = document.GetElementsByTagName("HTML")[0];
        }

        [TestMethod]
        public void TestBuildDomTree()
        {
            // Arrange
            string[] ignoredTags = new string[] { "P", "EM" };
            _builder = new SimplifiedDOMBuilder(_htmlTag, ignoredTags);

            // Act
            var domTree = _builder.Build();

            // Assert
            Assert.IsInstanceOfType(domTree, typeof(DOMTree));
        }

        [TestMethod]
        public void TestContainsHtmlNode()
        {
            // Arrange
            string[] ignoredTags = new string[] { "P", "EM" };
            _builder = new SimplifiedDOMBuilder(_htmlTag, ignoredTags);
            
            // Act
            var domTree = _builder.Build();
            
            // Arrange
            var html = domTree.RootNode;
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
            string[] ignoredTags = new string[] {"<B>"};
            /*var html = new TreeNode("HTML");
            var head = new TreeNode("HEAD");
            html.AddNode(head);
            var title = new TreeNode("TITLE");
            head.AddNode(title);
            var text = new TreeNode("TEXT");
            var body = new TreeNode("BODY");
            html.AddNode(body);
            var p = new TreeNode("P");
            body.AddNode(p);
            var br = new TreeNode("BR");
            p.AddNode(br);*/

            // Act
            _builder = new SimplifiedDOMBuilder(_htmlTag, ignoredTags);
            var domTree = _builder.Build();
            //var result = CompareTrees(html, domTree.RootNode);

            // Assert
            //Assert.IsTrue(result);*/
            var root = domTree.RootNode;
            Assert.AreEqual("HTML", root.Text);
            var head = root.Nodes[0];
            Assert.AreEqual("HEAD", head.Text);
            var title = head.Nodes[0];
            Assert.AreEqual("TITLE", title.Text);
            var body = root.Nodes[1];
            Assert.AreEqual("BODY", body.Text);
            var p = body.Nodes[0];
            Assert.AreEqual("P", p.Text);
            var text = p.Nodes[0];
            Assert.AreEqual("TEXT", text.Text);
            var br = p.Nodes[1];
            Assert.AreEqual("BR", br.Text);
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
    }
}
