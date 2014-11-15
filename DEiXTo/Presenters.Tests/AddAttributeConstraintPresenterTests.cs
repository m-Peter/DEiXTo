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
        private Mock<IAddAttributeConstraintView> _view;
        private AddAttributeConstraintPresenter _presenter;
        private TreeNode _node;
        private TagAttributeCollection _attrs;

        [TestInitialize]
        public void SetUp()
        {
            _view = new Mock<IAddAttributeConstraintView>();
            _node = new TreeNode("A");
            _attrs = createAttributes();
        }

        private TagAttributeCollection createAttributes()
        {
            NodeInfo nInfo = new NodeInfo();
            var attrs = new TagAttributeCollection();
            attrs.Add(new TagAttribute { Name = "href", Value = "http://www.google.gr" });
            attrs.Add(new TagAttribute { Name = "id", Value = "some-link" });
            nInfo.Attributes = attrs;
            _node.Tag = nInfo;

            return attrs;
        }

        [TestMethod]
        public void TestPopulateAttributes()
        {
            // Act
            _presenter = new AddAttributeConstraintPresenter(_view.Object, _node);

            // Assert
            _view.Verify(v => v.LoadAttributes(It.Is<List<TagAttribute>>(attr => _attrs.All == attr)));
            Assert.AreEqual(2, _node.GetAttributes().Count);
        }

        [TestMethod]
        public void TestAddConstraint()
        {
            // Arrange
            string constraint = "some-link";
            _view.Setup(v => v.Constraint).Returns(constraint);

            // Act
            _presenter = new AddAttributeConstraintPresenter(_view.Object, _node);
            _presenter.AddConstraint("id");

            // Assert
            var attrConstraint = _node.GetAttrConstraint();
            Assert.AreEqual("id", attrConstraint.Attribute);
            Assert.AreEqual("some-link", attrConstraint.Value);
            _view.Verify(v => v.Exit());
        }

        [TestMethod]
        public void TestAttributeChanged()
        {
            // Arrange
            _presenter = new AddAttributeConstraintPresenter(_view.Object, _node);
            var attribute = new TagAttribute { Name = "id", Value = "some-link" };
            
            // Act
            _presenter.AttributeChanged(attribute);

            // Assert
            _view.VerifySet(v => v.Constraint = attribute.Value);
        }

        [TestMethod]
        public void TestSelectSpecifiedAttributeConstraint()
        {
            // Arrange
            var attrConstraint = new TagAttributeConstraint { Attribute = "id", Value = "some-link" };
            _node.SetAttrConstraint(attrConstraint);

            // Act
            _presenter = new AddAttributeConstraintPresenter(_view.Object, _node);

            // Assert
            _view.Verify(v => v.LoadAttributes(It.Is<List<TagAttribute>>(attr => _attrs.All == attr)));
            _view.Verify(v => v.SelectAttribute(It.Is<TagAttribute>(attr => attr.Name == "id" && attr.Value == "some-link")));
        }
    }
}
