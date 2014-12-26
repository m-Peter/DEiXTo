using DEiXTo.Models;
using DEiXTo.Presenters;
using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DEiXTo.Views
{
    public partial class RegexBuilderWindow : Form, IRegexBuilderView
    {
        public RegexBuilderWindow()
        {
            InitializeComponent();
            var item1 = new ListViewItem(new string[] { "[abc]", "A single character of: a, b, or c" });
            var item2 = new ListViewItem(new string[] { "[^abc]", "Any single character except: a, b, or c" });
            var item3 = new ListViewItem(new string[] { "[a-z]", "Any single character in the range a-z" });
            var item4 = new ListViewItem(new string[] { "[a-zA-Z]", "Any single character in the range a-z or A-Z" });
            var item5 = new ListViewItem(new string[] { "^", "Start of line" });
            var item6 = new ListViewItem(new string[] { "$", "End of line" });
            var item7 = new ListViewItem(new string[] { "\\A", "Start of string" });
            var item8 = new ListViewItem(new string[] { "\\z", "End of string" });
            var item9 = new ListViewItem(new string[] { ".", "Any single character" });
            var item10 = new ListViewItem(new string[] { "\\s", "Any whitespace character" });
            var item11 = new ListViewItem(new string[] { "\\S", "Any non-whitespace character" });
            var item12 = new ListViewItem(new string[] { "\\d", "Any digit" });
            var item13 = new ListViewItem(new string[] { "\\D", "Any non-digit" });
            var item14 = new ListViewItem(new string[] { "\\w", "Any word character (letter, number, underscore)" });
            var item15 = new ListViewItem(new string[] { "\\W", "Any non-word character" });
            var item16 = new ListViewItem(new string[] { "\\b", "Any word boundary" });
            var item17 = new ListViewItem(new string[] { "(...)", "Capture everything enclosed" });
            var item18 = new ListViewItem(new string[] { "(a|b)", "a or b" });
            var item19 = new ListViewItem(new string[] { "a?", "Zero or one of a" });
            var item20 = new ListViewItem(new string[] { "a*", "Zero or more of a" });
            var item21 = new ListViewItem(new string[] { "a+", "One or more of a" });
            var item22 = new ListViewItem(new string[] { "a{3}", "Exactly 3 of a" });
            var item23 = new ListViewItem(new string[] { "a{3,}", "3 or more of a" });
            var item24 = new ListViewItem(new string[] { "a{3,6}", "Between 3 and 6 of a" });

            PatternsListView.Items.Add(item1);
            PatternsListView.Items.Add(item2);
            PatternsListView.Items.Add(item3);
            PatternsListView.Items.Add(item4);
            PatternsListView.Items.Add(item5);
            PatternsListView.Items.Add(item6);
            PatternsListView.Items.Add(item7);
            PatternsListView.Items.Add(item8);
            PatternsListView.Items.Add(item9);
            PatternsListView.Items.Add(item10);
            PatternsListView.Items.Add(item11);
            PatternsListView.Items.Add(item12);
            PatternsListView.Items.Add(item13);
            PatternsListView.Items.Add(item14);
            PatternsListView.Items.Add(item15);
            PatternsListView.Items.Add(item16);
            PatternsListView.Items.Add(item17);
            PatternsListView.Items.Add(item18);
            PatternsListView.Items.Add(item19);
            PatternsListView.Items.Add(item20);
            PatternsListView.Items.Add(item21);
            PatternsListView.Items.Add(item22);
            PatternsListView.Items.Add(item23);
            PatternsListView.Items.Add(item24);

            this.KeyPreview = true;
            this.KeyDown += RegexBuilderWindow_KeyDown;
        }

        public string RegexText
        {
            get { return AddRegexTextBox.Text; }
            set
            {
                AddRegexTextBox.Text = value;
                AddRegexTextBox.SelectAll();
                AddRegexTextBox.Focus();
            }
        }

        public bool InverseRegex
        {
            get { return InverseEvaluationCheckBox.Checked; }
            set { InverseEvaluationCheckBox.Checked = value; }
        }

        public RegexBuilderPresenter Presenter { get; set; }

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
            Presenter.AddRegex();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void RegexBuilderWindow_KeyDown(object sender, KeyEventArgs e)
        {
            Presenter.KeyDownPress(e.KeyCode);
        }

        private void PatternsListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var item = e.Item;

            if (e.IsSelected)
            {
                AddRegexTextBox.Text = item.SubItems[0].Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Gather Pattern, Input string
            var pattern = RegexTb.Text;
            var input = InputTb.Text;
            var regex = new Regex(pattern);
            var match = regex.Match(input);

            ShowMatches(regex, match);

            MatchRtb.AppendText(input);
            int pos = 0;
            int index = -1;

            while (match.Success)
            {
                // Get the value that matched (regex.value)
                index = input.IndexOf(match.Value, pos);
                // Advance the position, by the length of matched value
                pos = index;
                // Highlight the matched value
                MatchRtb.SelectionStart = index;
                MatchRtb.SelectionLength = match.Value.Length;
                MatchRtb.SelectionColor = Color.BlueViolet;
                // Move to the next regex
                match = match.NextMatch();
            }
        }

        private void ShowMatches(Regex regex, Match match)
        {
            string[] names = regex.GetGroupNames();
            for (int i = 1; i < names.Length; i++)
            {
                Group grp = match.Groups[names[i]];
                string format = string.Format("{0} {1}", names[i], grp.Value);
                GroupsRtb.AppendText(format + "\n");
            }
        }
    }
}
