﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DEiXTo.Views;
using System.Windows.Forms;
using DEiXTo.Services;
using DEiXTo.Models;
using System.Drawing;

namespace DEiXTo.Presenters.Tests
{
    [TestClass]
    public class RegexBuilderPresenterTests
    {
        private Mock<IRegexBuilderView> view;
        private TreeNode node;
        private RegexBuilderPresenter presenter;
        private Mock<IEventHub> eventHub;

        [TestInitialize]
        public void SetUp()
        {
            view = new Mock<IRegexBuilderView>();
            node = new TreeNode("H1");
            node.Tag = new NodeInfo();
            node.SetContent("This is the header");
            eventHub = new Mock<IEventHub>();
            presenter = new RegexBuilderPresenter(view.Object, node, eventHub.Object);
        }

        [TestMethod]
        public void TestRegexTextIsPopulatedWithNodeContent()
        {
            // Assert
            view.VerifySet(v => v.RegexText = "This is the header");
        }

        [TestMethod]
        public void TestAddRegex()
        {
            // Arrange
            view.Setup(v => v.RegexText).Returns("some regex");
            view.Setup(v => v.Action).Returns(ConstraintAction.MatchAndExtract);

            // Act
            presenter.AddRegex();

            // Assert
            var constraint = node.GetRegexConstraint();
            Assert.AreEqual("some regex", constraint.Pattern);
            Assert.AreEqual(ConstraintAction.MatchAndExtract, constraint.Action);
            eventHub.Verify(e => e.Publish(It.Is<RegexAdded>(ra => ra.Node == node)));
            view.Verify(v => v.Exit());
        }

        [TestMethod]
        public void TestAddInvalidRegex()
        {
            // Arrange
            view.Setup(v => v.RegexText).Returns("");

            // Act
            presenter.AddRegex();

            // Assert
            view.Verify(v => v.ShowInvalidRegexMessage());
            view.Verify(v => v.Exit(), Times.Never);
        }

        [TestMethod]
        public void TestPressingEnterAddsRegex()
        {
            // Arrange
            view.Setup(v => v.RegexText).Returns("some regex");
            view.Setup(v => v.Action).Returns(ConstraintAction.Match);

            // Act
            presenter.KeyDownPress(Keys.Enter);

            // Assert
            var constraint = node.GetRegexConstraint();
            Assert.AreEqual("some regex", constraint.Pattern);
            Assert.AreEqual(ConstraintAction.Match, constraint.Action);
            eventHub.Verify(e => e.Publish(It.Is<RegexAdded>(ra => ra.Node == node)));
            view.Verify(v => v.Exit());
        }

        [TestMethod]
        public void TestLoadExistingRegex()
        {
            // Arrange
            var constraint = new RegexConstraint("[0-9]{2}", ConstraintAction.Match);
            node.SetRegexConstraint(constraint);

            // Act
            presenter = new RegexBuilderPresenter(view.Object, node, eventHub.Object);

            // Assert
            view.VerifySet(v => v.RegexText = "[0-9]{2}");
            view.VerifySet(v => v.Action = ConstraintAction.Match);
        }

        [TestMethod]
        public void TestChangeExistingRegex()
        {
            // Arrange
            var constraint = new RegexConstraint("[a-z]{4}", ConstraintAction.Match);
            node.SetRegexConstraint(constraint);

            // Act
            presenter = new RegexBuilderPresenter(view.Object, node, eventHub.Object);
            view.Setup(v => v.RegexText).Returns("[0-9]{2}");
            view.Setup(v => v.Action).Returns(ConstraintAction.MatchAndExtract);
            presenter.AddRegex();

            // Assert
            var result = node.GetRegexConstraint();
            Assert.AreEqual("[0-9]{2}", result.Pattern);
            Assert.AreEqual(ConstraintAction.MatchAndExtract, result.Action);
            eventHub.Verify(e => e.Publish(It.Is<RegexAdded>(ra => ra.Node == node)));
            view.Verify(v => v.Exit());
        }
    }
}
