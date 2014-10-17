using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public partial class RegexBuilderWindow : Form , IRegexBuilderView
    {
        public event Action AddRegex;

        public RegexBuilderWindow()
        {
            InitializeComponent();
            ListViewItem item1 = new ListViewItem(new string[] { "^b", "Begin with 'b'", "some example use case" });
            ListViewItem item2 = new ListViewItem(new string[] { "e$", "End with 'e'", "some example use case" });
            ListViewItem item3 = new ListViewItem(new string[] { "sth", "Contains 'sth'", "some example use case" });
            ListViewItem item4 = new ListViewItem(new string[] { ".*sth.*", "Contains 'sth' but gets all the text", "some example use case" });
            ListViewItem item5 = new ListViewItem(new string[] { "^w$", "Exact match with 'w'", "some example use case" });
            ListViewItem item6 = new ListViewItem(new string[] { "\\$", "Price in dollars", "some example use case" });
            ListViewItem item7 = new ListViewItem(new string[] { "€", "Price in euro", "some example use case" });
            ListViewItem item8 = new ListViewItem(new string[] { "\\$(\\d*,?\\d*\\.?\\d*)", "Extract just the price", "some example use case" });
            ListViewItem item9 = new ListViewItem(new string[] { ".*", "Matches everything", "some example use case" });
            PatternsListView.Items.Add(item1);
            PatternsListView.Items.Add(item2);
            PatternsListView.Items.Add(item3);
            PatternsListView.Items.Add(item4);
            PatternsListView.Items.Add(item5);
            PatternsListView.Items.Add(item6);
            PatternsListView.Items.Add(item7);
            PatternsListView.Items.Add(item8);
            PatternsListView.Items.Add(item9);
        }

        public string GetRegexText()
        {
            return AddRegexTextBox.Text;
        }

        public void SetRegexText(string regex)
        {
            AddRegexTextBox.Text = regex;
        }

        public void ShowInvalidRegexMessage()
        {
            MessageBox.Show("Invalid EMPTY regular expression!", "DEiXTo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public void Exit()
        {
            this.Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (AddRegex != null)
            {
                AddRegex();
            }
        }
    }
}
