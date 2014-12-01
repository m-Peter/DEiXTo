using DEiXTo.Models;
using DEiXTo.Presenters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;

namespace DEiXTo.Views.Tests
{
    [TestClass]
    public class AddAttributeConstraintWindowTests
    {
        private AddAttributeConstraintWindow window;
        private AddAttributeConstraintPresenter presenter;
        private TreeNode node;

        [TestMethod]
        public void TestStartingState()
        {
            node = new TreeNode("DIV");
            NodeInfo nInfo = new NodeInfo();
            node.Tag = nInfo;
            window = new AddAttributeConstraintWindow();
            presenter = new AddAttributeConstraintPresenter(window, node);

            window.Show();

            Assert.AreEqual("", window.AddConstraintTextBox.Text);
            Assert.AreEqual(0, window.AttributesComboBox.Items.Count);
        }

        [TestMethod]
        public void TestLoadAttributes()
        {
            // Arrange
            node = new TreeNode("DIV");
            NodeInfo nInfo = new NodeInfo();
            node.Tag = nInfo;
            var attributes = new TagAttributeCollection();
            var id = new TagAttribute { Name = "Id", Value = "container" };
            var klass = new TagAttribute { Name = "class", Value = "column" };
            attributes.Add(id);
            attributes.Add(klass);
            node.SetAttributes(attributes);
            window = new AddAttributeConstraintWindow();
            presenter = new AddAttributeConstraintPresenter(window, node);
            
            // Act
            window.Show();

            // Assert
            Assert.AreEqual(2, window.AttributesComboBox.Items.Count);
        }

        [TestMethod]
        public void TestLoadAttributesAndConstraint()
        {
            // Arrange
            node = new TreeNode("DIV");
            NodeInfo nInfo = new NodeInfo();
            node.Tag = nInfo;
            var attributes = new TagAttributeCollection();
            var id = new TagAttribute { Name = "Id", Value = "container" };
            var klass = new TagAttribute { Name = "class", Value = "column" };
            attributes.Add(id);
            attributes.Add(klass);
            node.SetAttributes(attributes);
            var attrConstraint = new TagAttributeConstraint { Attribute="class", Value="column" };
            node.SetAttrConstraint(attrConstraint);
            window = new AddAttributeConstraintWindow();
            presenter = new AddAttributeConstraintPresenter(window, node);

            // Act
            window.Show();
            var selectedItem = window.AttributesComboBox.SelectedItem as TagAttribute;

            // Assert
            Assert.AreEqual("class", selectedItem.Name);
            Assert.AreEqual("column", selectedItem.Value);
        }

        [TestMethod]
        public void TestChangeSelectedAttribute()
        {
            // Arrange
            node = new TreeNode("DIV");
            NodeInfo nInfo = new NodeInfo();
            node.Tag = nInfo;
            var attributes = new TagAttributeCollection();
            var id = new TagAttribute { Name = "Id", Value = "container" };
            var klass = new TagAttribute { Name = "class", Value = "column" };
            attributes.Add(id);
            attributes.Add(klass);
            node.SetAttributes(attributes);
            window = new AddAttributeConstraintWindow();
            presenter = new AddAttributeConstraintPresenter(window, node);

            // Act
            window.Show();
            window.AttributesComboBox.SelectedItem = klass;

            // Assert
            var selectedItem = window.AttributesComboBox.SelectedItem as TagAttribute;
            Assert.AreEqual("class", selectedItem.Name);
            Assert.AreEqual("column", selectedItem.Value);
        }
    }
}
