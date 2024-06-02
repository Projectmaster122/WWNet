using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WWNet_Browser.FeatherInterp;
using WWNet_Browser.FeatherRenderer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WWNet_Browser
{
    public partial class Form1 : Form
    {
        Browser b;
        const int AddressBarPadding = 5;
        string print(List<WHTMLObject> dom, int level = 0, string display = "")
        {
            foreach (var i in dom)
            {
                for (int j = 0; j < level; j++) display += '>';
                display += '>';
                display += (i.Type.ToString() + " Id=" + i.Id.ToString() + (i.Attributes.ContainsKey("text") ? (" Text=" + i.Attributes["text"]) : "") + '\n');
                display += print(i.Children, level + 1);
            }
            return display;
        }
        public Form1()
        {
            InitializeComponent();
            AddressBar.Width = this.ClientSize.Width - AddressBar.Left - AddressBarPadding; // keep a right margin on address bar when resizing window
            b = new Browser(Page, AddressBar, this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            AddressBar.Width = this.ClientSize.Width - AddressBar.Left - AddressBarPadding; // keep a right margin on address bar when resizing window
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string localSave = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/" + "WWNet";
            if (Directory.Exists(localSave)) Directory.Delete(localSave, true);
        }

        private void AddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                b.Navigate(AddressBar.Text);
            }
        }

        private void Back_Click(object sender, EventArgs e)
        {
            b.Back();
        }

        private void Forward_Click(object sender, EventArgs e)
        {
            b.Forward();
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            b.Refresh();
        }
    }
}
