using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WWNet_Browser.FeatherInterp;

namespace WWNet_Browser.FeatherRenderer
{
    internal class RenderDOM
    {
        static Dictionary<Control, WHTMLObject> conv = new Dictionary<Control, WHTMLObject>(); // initiates the connection between controls and WHTMLObjects
        static Panel Page;
        static Browser BrowserInstance;
        static Dictionary<string, string> hrefValue = new Dictionary<string, string>(); // HREF value for every link or button, lookup by control name
        static Dictionary<string, bool> visited = new Dictionary<string, bool>(); // link visited in current session?
        static Control GenerateText(WHTMLObject t)
        {
            float textSize = t.Type switch
            {
                TagType.h1 => Constants.SizeH1,
                TagType.h2 => Constants.SizeH2,
                TagType.h3 => Constants.SizeH3,
                TagType.h4 => Constants.SizeH4,
                TagType.h5 => Constants.SizeH5,
                TagType.h6 => Constants.SizeH6,
                _ => Constants.SizeDef
            };
            Label txt = new Label();
            if(t.Attributes.ContainsKey("text")) txt.Text = t.Attributes["text"];
            txt.Font = new System.Drawing.Font(txt.Font.FontFamily, textSize, FontStyle.Regular, GraphicsUnit.Pixel);
            if (t.Type == TagType.b) txt.Font = new System.Drawing.Font(txt.Font, System.Drawing.FontStyle.Bold);
            if (t.Type == TagType.s) txt.Font = new System.Drawing.Font(txt.Font, System.Drawing.FontStyle.Strikeout);
            if (t.Type == TagType.i) txt.Font = new System.Drawing.Font(txt.Font, System.Drawing.FontStyle.Italic);
            if (t.Type == TagType.u || t.Type==TagType.a) txt.Font = new System.Drawing.Font(txt.Font, System.Drawing.FontStyle.Underline);
            txt.AutoSize = true;
            if(t.Type==TagType.a)
            {
                txt.ForeColor = SystemColors.HotTrack;
                txt.Cursor = Cursors.Hand;
                txt.MouseClick += Txt_MouseClick;
                hrefValue[txt.Name] = t.Attributes["href"];
            }
            //if (t.Attributes.ContainsKey("style")) StyleWCSS.InlineStyle(t, t.Attributes["style"]);
            conv[txt] = t;
            return txt;
        }
        static Control GenerateLink(WHTMLObject t)
        {
            Link b = new Link();
            b.Tag = Guid.NewGuid().ToString();
            b.Font = new System.Drawing.Font(b.Font.FontFamily, Constants.SizeDef, FontStyle.Regular, GraphicsUnit.Pixel);
            b.AutoSize = true;
            if (t.Attributes.ContainsKey("text")) b.Text = t.Attributes["text"];
            hrefValue[b.Tag.ToString()] = t.Attributes["href"];
            b.MouseClick += Txt_MouseClick;
            if (visited.ContainsKey(t.Attributes["href"])) b.LinkVisited = visited[t.Attributes["href"]];
            conv[b] = t;
            return b;
        }
        private static void Txt_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender.GetType() == typeof(Link))
                {
                    (sender as Link).LinkVisited = true;
                    visited[hrefValue[(sender as Link).Tag.ToString()]] = true;
                }
            }
            catch { }
            BrowserInstance.Navigate(hrefValue[(sender as Control).Tag.ToString()]);
        }

        static Control GenerateButton(WHTMLObject t)
        {
            System.Windows.Forms.Button b = new System.Windows.Forms.Button();
            b.Tag = Guid.NewGuid().ToString();
            if (t.Attributes.ContainsKey("text")) b.Text = t.Attributes["text"];
            hrefValue[b.Tag.ToString()] = t.Attributes["href"];
            b.MouseClick += Txt_MouseClick;
            conv[b] = t;
            return b;
        }
        static Control GenerateInput(WHTMLObject t)
        {
            System.Windows.Forms.TextBox b = new System.Windows.Forms.TextBox();
            if (t.Attributes.ContainsKey("text")) b.Text = t.Attributes["text"];
            if (t.Attributes.ContainsKey("hidden")) b.UseSystemPasswordChar = t.Attributes["hidden"].ToLower()=="true";
            b.TextChanged += B_TextChanged;
            conv[b] = t;
            return b;
        }

        private static void B_TextChanged(object sender, EventArgs e)
        {
            conv[sender as TextBox].Attributes["text"] = (sender as TextBox).Text;
        }

        static Control GenerateDiv(WHTMLObject t)
        {
            FlowLayoutPanel f = new FlowLayoutPanel();
            f.FlowDirection = FlowDirection.TopDown;
            f.AutoSize = true;
            conv[f] = t;
            return f;
        }
        static void AddToUI(Control u, Panel parent)
        {
            //page.Controls.Remove(page.Controls[0]);
            try { parent.Controls.Add(u); } catch { Page.Controls.Add(u); }
        }
        static void ApplyAttributes(Control u, WHTMLObject obj) // Universal attributes only
        {
            if (u == null) return;
            foreach(string attrib in obj.Attributes.Keys)
            {
                if (attrib == "text") continue;
                if (attrib == "height") u.MaximumSize = new Size(u.MaximumSize.Width, int.Parse(obj.Attributes[attrib]));
                if (attrib == "width") u.MaximumSize = new Size(int.Parse(obj.Attributes[attrib]), u.MaximumSize.Height);
            }
        }
        public static void Render(Panel page, List<WHTMLObject> dom, Browser b, Panel parent = null)
        {
            List<string> wcssFiles = new List<string>(), wcsharpFiles = new List<string>();
            BrowserInstance = b;
            Page = page;
            if (parent == null) parent = page;
            foreach(WHTMLObject node in dom)
            {
                Control c = null;
                switch(node.Type)
                {
                    case TagType.p:
                    case TagType.s:
                    case TagType.i:
                    case TagType.u:
                    case TagType.b:
                    case TagType.h6:
                    case TagType.h5:
                    case TagType.h4:
                    case TagType.h3:
                    case TagType.h2:
                    case TagType.h1:
                        c = GenerateText(node);
                        break;
                    case TagType.a:
                        c = GenerateLink(node);
                        break;
                    case TagType.div:
                        c = GenerateDiv(node);
                        break;

                    case TagType.button:
                        c = GenerateButton(node);
                        break;
                    case TagType.input:
                        c = GenerateInput(node);
                        break;
                    case TagType.include:
                        if (node.Attributes["type"].ToLower().Trim() == "wcss") wcssFiles.Add(node.Attributes["src"]);
                        else if (node.Attributes["type"].ToLower().Trim() == "wcsharp") wcsharpFiles.Add(node.Attributes["src"]);
                        break;
                    default:
                        MessageBox.Show("cannot render type " + node.Type.ToString());
                        break;
                }
                ApplyAttributes(c, node);
                AddToUI(c, parent);
                Render(page, node.Children, b, c as Panel);
            }
        }
    }
}
