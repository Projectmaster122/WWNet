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
using System.Text.Json;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Security.Policy;
using System.Runtime.Remoting.Contexts;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace WWNet_Browser
{
    internal class Website
    {
        public string Domain, GitRepo;
        public Website(string a, string b) { Domain = a;GitRepo = b; }
    }
    internal class Browser
    {
        //feel free to change this and make ur own wwnet, as long as it works and im not dumb
        const string domainsJsonUri = "https://raw.githubusercontent.com/usarana/whttp/main/websites.json";
        Panel Page;
        Form1 f1Instance;
        System.Windows.Forms.TextBox AddressBar;
        // dict[domain] = git link
        Dictionary<string, string> DomainToGit = new Dictionary<string, string>();
        List<string> history = new List<string>();
        int historyIndex = -1;
        string currentLocalDir = "";
        public Browser(Panel Page, System.Windows.Forms.TextBox AddressBar, Form1 f1Instance) 
        {
            this.Page = Page;
            this.AddressBar = AddressBar;
            this.f1Instance = f1Instance;
            string domainsJsonRaw = String.Empty;
            HttpClient h = new HttpClient();
            domainsJsonRaw = h.GetStringAsync(domainsJsonUri).Result;
            h.Dispose();

            JsonNode j = JsonNode.Parse(domainsJsonRaw);
            var w = j["websites"].AsArray();
            foreach(var jsonNode in w)
            {
                DomainToGit[
                    jsonNode["domain"]
                    .AsValue()
                    .ToString()
                ] = 
                    jsonNode["git"]
                    .AsValue()
                    .ToString();
            }
        }
        public void Refresh()
        {
            Navigate(AddressBar.Text, false);
        }
        public void Back()
        {
            if (historyIndex < 1) return;
            historyIndex--;
            Navigate(history[historyIndex], false);
        }
        public void Forward()
        {
            if (historyIndex == history.Count-1) return;
            historyIndex++;
            Navigate(history[historyIndex], false);
        }
        string currentDomain = "";
        public void Navigate(string site, bool pushHistory = true)
        {
            if (site.StartsWith("whttp://")) site = site.Substring(8);
            if(site.StartsWith("www.")) site=site.Substring(4);
            if(pushHistory) historyIndex++;
    
            string path = String.Empty;
            string file = String.Empty;
            string domain = (site.Contains("/")?site.Substring(0, site.IndexOf('/')):site);
            if(!DomainToGit.ContainsKey(domain) && !DomainToGit.ContainsKey(site))
            {
                path = site;
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
                file = path;
                path = currentLocalDir + "/" + path;
                
                if (!File.Exists(path)) { path += ".whtml"; file += ".whtml"; }
                currentLocalDir = Path.GetDirectoryName(path);
                
                if (!File.Exists(path))
                {
                    MessageBox.Show("site " + site + " not found!");
                }
                else
                {
                    domain = currentDomain;
                    string localSave = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/WWNet/" + domain + "/site";
                    localSave = Directory.GetDirectories(localSave)[0] + "/";
                    //fix different formatting for variables path and localSave
                    path = new DirectoryInfo(path).FullName;
                    localSave = new DirectoryInfo(localSave).FullName;
                    file = path.Replace(localSave, "");
                    AddressBar.Text = domain + "/" + file.Replace("\\", "/");
                }
            }
            else
            {
                currentDomain = domain;
                file = (site.Contains("/") ? site.Substring(site.IndexOf('/') + 1) : String.Empty);
                string localSave = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/WWNet";
                if(!Directory.Exists(localSave)) Directory.CreateDirectory(localSave);
                localSave += "/" + domain;
                if (!Directory.Exists(localSave)) 
                {
                    Directory.CreateDirectory(localSave);
                }
                
                else { Directory.Delete(localSave, true); Directory.CreateDirectory(localSave); }
                //download site
                HttpClient h = new HttpClient();
                var zip = h.GetByteArrayAsync(DomainToGit[domain]).Result;
                File.WriteAllBytes(localSave + "/site.zip", zip);
                var zf = ZipFile.OpenRead(localSave + "/site.zip");
                zf.ExtractToDirectory(localSave + "/site");
                zf.Dispose();
                path = localSave + "/site";
                path = Directory.GetDirectories(path)[0];
                
                currentLocalDir = Path.GetDirectoryName(path);
                try
                {
                    if (Directory.Exists(path) && file == String.Empty) path += "/main.whtml";
                    else if (Directory.Exists(path)) path += "/" + file;
                }
                catch { }
                try
                {
                    if (Directory.Exists(path)) currentLocalDir = path;
                    else 
                    {
                        if (!path.EndsWith(".whtml"))
                        {
                            path += ".whtml";
                            file += ".whtml";
                        }
                        currentLocalDir = Path.GetDirectoryName(path);
                    }
                }
                catch { }

                AddressBar.Text = domain + "/" + (file==String.Empty?"main.whtml":file);
            }
            try
            {
                if (Directory.Exists(path) && file == String.Empty) path += "/main.whtml";
                else if (Directory.Exists(path)) path += "/" + file;
            }
            catch { }
            //make this look nicer lol
            if (AddressBar.Text.EndsWith(".whtml"))
                AddressBar.Text = AddressBar.Text.Substring(0, AddressBar.Text.Length - 6);
            if (AddressBar.Text.EndsWith("/main"))
                AddressBar.Text = AddressBar.Text.Substring(0, AddressBar.Text.Length - 5);
            AddressBar.Text = "whttp://"+AddressBar.Text;
            Page.Controls.Clear();
            //fix weird
            AddressBar.Select(0, 0);
            f1Instance.ActiveControl = null;
            if(pushHistory) history.Add(AddressBar.Text);
            List<WHTMLObject> dom = new FeatherWHTML().run(File.ReadAllText(path));
            RenderDOM.Render(Page, dom, this, currentLocalDir);
        }
    }
}
