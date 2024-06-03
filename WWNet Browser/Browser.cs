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
        Feather f;
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
            f = new Feather();
            string domainsJsonRaw = String.Empty;
            HttpClient h = new HttpClient();
            domainsJsonRaw = h.GetStringAsync(domainsJsonUri).Result;
            h.Dispose();
            /*
            Task.Run(async () => {
                HttpClient h = new HttpClient();
                domainsJsonRaw = await h.GetStringAsync(domainsJsonUri);
                h.Dispose();
            }).Wait();
            */
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
                /*
                domains.Add(
                    new Website(
                        jsonNode["domain"]
                        .AsValue()
                        .ToString(),

                        jsonNode["git"]
                        .AsValue()
                        .ToString()
                    )
                );
                */
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
            if(pushHistory) historyIndex++;
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
                /*
                while(path.Contains("../"))
                {
                    path = Regex.Replace(path, "[A-Za-z0-9]+/\\.\\./", "", RegexOptions.IgnoreCase);
                }
                */
                
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
                /*
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
                //else if (File.Exists(currentLocalDir + "\\" + path)) path = currentLocalDir + "\\" + path;
                else if (File.Exists(currentLocalDir + "/" + path)) path = currentLocalDir + "/" + path;
                //else if (File.Exists(currentLocalDir + path)) path = currentLocalDir + path;
                //else if (File.Exists(path + "\\main.whtml")) path = path + "\\main.whtml";
                else if (File.Exists(path + "/main.whtml")) path = path + "/main.whtml";
                //else if (File.Exists(path + "main.whtml")) path = path + "main.whtml";
                */ 

                /* why is this in my code what drugs was i using when i wrote this what
                else if (File.Exists(currentLocalDir + "\\main.whtml")) path = currentLocalDir + "\\main.whtml";
                else if (File.Exists(currentLocalDir + "/main.whtml")) path = currentLocalDir + "/main.whtml";
                else if (File.Exists(currentLocalDir + "main.whtml")) path = currentLocalDir + "main.whtml";
                */
                // else throw new FileNotFoundException();
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
                /*
                else if (File.Exists(path + "\\index.whtml")) path = path + "\\index.whtml";
                else if (File.Exists(path + "/index.whtml")) path = path + "/index.whtml";
                else if (File.Exists(path + "index.whtml")) path = path + "index.whtml";
                */
                AddressBar.Text = domain + "/" + (file==String.Empty?"main.whtml":file);
            }
            try
            {
                if (Directory.Exists(path) && file == String.Empty) path += "/main.whtml";
                else if (Directory.Exists(path)) path += "/" + file;
            }
            catch { }
            //temporary
            //path += "/main.whtml";
            Page.Controls.Clear();
            //fix weird
            AddressBar.Select(0, 0);
            f1Instance.ActiveControl = null;
            if(pushHistory) history.Add(AddressBar.Text);
            Feather f = new Feather();
            List<WHTMLObject> dom = f.wh.run(File.ReadAllText(path));
            RenderDOM.Render(Page, dom, this);
        }
    }
}
