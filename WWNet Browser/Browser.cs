using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;
using WWNet_Browser.FeatherInterp;
using WWNet_Browser.FeatherRenderer;

namespace WWNet_Browser
{
    internal class Browser
    {
        Feather f;
        Panel Page;
        string currentLocalDir = "";
        public Browser(Panel Page) 
        {
            this.Page = Page;
            f = new Feather();
        }
        public void Navigate(string path)
        {

            /*
            if (!File.Exists(path))
            {
                if (path.StartsWith("./") || path.StartsWith(".\\"))
                {
                    var p = path.ToList();
                    p.RemoveAt(0);
                    path = new string(p.ToArray());
                }
                if (path.StartsWith("/") || path.StartsWith("\\"))
                {
                    var p = path.ToList();
                    p.RemoveAt(0);
                    path = new string(p.ToArray());
                }
                MessageBox.Show(Directory.GetCurrentDirectory());
                if (File.Exists(Directory.GetCurrentDirectory() + "/" + path)) path = Directory.GetCurrentDirectory() + path;
                else if (File.Exists(Directory.GetCurrentDirectory() + "\\" + path)) path = Directory.GetCurrentDirectory() + path;
                else if (File.Exists(Directory.GetCurrentDirectory() + path)) path = Directory.GetCurrentDirectory() + path;
                else throw new FileNotFoundException(path);
            }
            */
            if (path.StartsWith("./") || path.StartsWith(".\\"))
            {
                var p = path.ToList();
                p.RemoveAt(0);
                path = new string(p.ToArray());
            }
            if (path.StartsWith("/") || path.StartsWith("\\"))
            {
                var p = path.ToList();
                p.RemoveAt(0);
                path = new string(p.ToArray());
            }
            if (File.Exists(path)) { }
            else if (File.Exists(currentLocalDir + "\\" + path)) path = currentLocalDir + "\\" + path;
            else if (File.Exists(currentLocalDir + "/" + path)) path = currentLocalDir + "/" + path;
            else if (File.Exists(currentLocalDir + path)) path = currentLocalDir + path;
            else if (File.Exists(path + "\\main.whtml")) path = path + "\\main.whtml";
            else if (File.Exists(path + "/main.whtml")) path = path + "/main.whtml";
            else if (File.Exists(path + "main.whtml")) path = path + "main.whtml";
            /* why is this in my code what drugs was i using when i wrote this what
            else if (File.Exists(currentLocalDir + "\\main.whtml")) path = currentLocalDir + "\\main.whtml";
            else if (File.Exists(currentLocalDir + "/main.whtml")) path = currentLocalDir + "/main.whtml";
            else if (File.Exists(currentLocalDir + "main.whtml")) path = currentLocalDir + "main.whtml";
            */
            else throw new FileNotFoundException();
            currentLocalDir = Path.GetDirectoryName(path);
            /*
            else if (File.Exists(path + "\\index.whtml")) path = path + "\\index.whtml";
            else if (File.Exists(path + "/index.whtml")) path = path + "/index.whtml";
            else if (File.Exists(path + "index.whtml")) path = path + "index.whtml";
            */
            Page.Controls.Clear();
            Feather f = new Feather();
            List<WHTMLObject> dom = f.wh.run(File.ReadAllText(path));
            RenderDOM.Render(Page, dom, this);
        }
    }
}
