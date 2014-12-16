using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using DEiXTo.Models;

namespace DEiXTo.Services.Tests
{
    [TestClass]
    public class NodeBuilderTests
    {
        [TestMethod]
        public void TestCreateNode()
        {
            var builder = new NodeInfo.Builder();
            
            var info = builder.SetSourceIndex(12).Build();
            Assert.AreEqual(12, info.SourceIndex);

            info = builder.SetPath("/html/head/body/div").Build();
            Assert.AreEqual("/html/head/body/div", info.Path);

            info = builder.SetContent("about web").Build();
            Assert.AreEqual("about web", info.Content);

            info = builder.SetSource("<div>about</div>").Build();
            Assert.AreEqual("<div>about</div>", info.Source);

            info = builder.SetLabel("SYNOPSIS").Build();
            Assert.AreEqual("SYNOPSIS", info.Label);

            info = builder.SetRoot(true).Build();
            Assert.AreEqual(true, info.IsRoot);

            info = builder.SetState(NodeState.Grayed).Build();
            Assert.AreEqual(NodeState.Grayed, info.State);

            info = builder.SetCareAboutSO(true).Build();
            Assert.AreEqual(true, info.CareAboutSiblingOrder);

            info = builder.SetStartIndex(0).Build();
            Assert.AreEqual(0, info.SiblingOrderStart);

            info = builder.SetStepValue(2).Build();
            Assert.AreEqual(2, info.SiblingOrderStep);
        }
    }
}
