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
    public partial class RegexBuilderWindow : Form
    {
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
            listView1.Items.Add(item1);
            listView1.Items.Add(item2);
            listView1.Items.Add(item3);
            listView1.Items.Add(item4);
            listView1.Items.Add(item5);
            listView1.Items.Add(item6);
            listView1.Items.Add(item7);
            listView1.Items.Add(item8);
            listView1.Items.Add(item9);
        }
    }
}
