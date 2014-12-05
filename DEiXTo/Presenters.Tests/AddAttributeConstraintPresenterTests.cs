using DEiXTo.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Windows.Forms;
using DEiXTo.Services;
using DEiXTo.Models;
using System.Collections.Generic;

namespace DEiXTo.Presenters.Tests
{
    [TestClass]
    public class AddAttributeConstraintPresenterTests
    {
        private Mock<IAddAttributeConstraintView> view;
        private AddAttributeConstraintPresenter presenter;
        private TreeNode node;
        private TagAttributeCollection attrs;

        [TestInitialize]
        public void SetUp()
        {
            view = new Mock<IAddAttributeConstraintView>();
            node = new TreeNode("A");
            attrs = createAttributes();
        }

        private TagAttributeCollection createAttributes()
        {
            NodeInfo nInfo = new NodeInfo();
            var attrs = new TagAttributeCollection();
            attrs.Add(new TagAttribute { Name = "href", Value = "http://www.google.gr" });
            attrs.Add(new TagAttribute { Name = "id", Value = "some-link" });
            nInfo.Attributes = attrs;
            node.Tag = nInfo;

            return attrs;
        }

        [TestMethod]
        public void TestPopulateAttributes()
        {
            // Act
            presenter = new AddAttributeConstraintPresenter(view.Object, node);

            // Assert
            view.Verify(v => v.LoadAttribute(It.IsAny<TagAttribute>()));
            Assert.AreEqual(2, node.GetAttributes().Count);
        }

        [TestMethod]
        public void TestAddConstraint()
        {
            // Arrange
            string constraint = "some-link";
            view.Setup(v => v.Constraint).Returns(constraint);

            // Act
            presenter = new AddAttributeConstraintPresenter(view.Object, node);
            presenter.AddConstraint("id");

            // Assert
            var attrConstraint = node.GetAttrConstraint();
            Assert.AreEqual("id", attrConstraint.Attribute);
            Assert.AreEqual("some-link", attrConstraint.Value);
            view.Verify(v => v.Exit());
        }

        [TestMethod]
        public void TestAttributeChanged()
        {
            // Arrange
            presenter = new AddAttributeConstraintPresenter(view.Object, node);
            var attribute = new TagAttribute { Name = "id", Value = "some-link" };
            
            // Act
            presenter.AttributeChanged(attribute);

            // Assert
            view.VerifySet(v => v.Constraint = attribute.Value);
        }

        [TestMethod]
        public void TestSelectSpecifiedAttributeConstraint()
        {
            // Arrange
            var attrConstraint = new TagAttributeConstraint { Attribute = "id", Value = "some-link" };
            node.SetAttrConstraint(attrConstraint);

            // Act
            presenter = new AddAttributeConstraintPresenter(view.Object, node);

            // Assert
            view.Verify(v => v.LoadAttribute(It.IsAny<TagAttribute>()));
        }
    }
}
