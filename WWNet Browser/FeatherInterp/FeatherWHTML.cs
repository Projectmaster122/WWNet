using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WWNet_Browser.FeatherInterp
{
    public enum TagType
    {
        h1, h2, div, text, h3, h4, h5, h6, p, b, s, i, u, button, input, a
    }
    public class Tag
    {
        public TagType Type;
        public string Text;
        public bool Closed = false;
        public string[] Arguments;
        public bool IsVoid = false;
        TagType[] VoidTags =
        {
            TagType.input
        };
        public void Init(string t)
        {
            string z = t.Trim();
            if (z[1]=='/') Closed = true;
            List<char> t1;
            t1 = t.ToList();
            t1.RemoveAt(0);
            t1.RemoveAt(t1.Count - 1);
            if (t1[0] == '/') t1.RemoveAt(0);
            t = new string(t1.ToArray());
            string[] args = t.Split(' ');
            Arguments = args;
            IsVoid = VoidTags.Where(x => x.ToString()==args[0]).Count()>0;
            try
            {
                Type = (TagType)Enum.Parse(typeof(TagType), args[0]);
            }
            catch
            {
                MessageBox.Show("unknown tag: " + t);
            }
        }
        public void InitText(string t)
        {
            Type = TagType.text;
            Text = t;
        }
    }
    internal class FeatherWHTML
    {
        List<WHTMLObject> dom;
        void AddChildToObjDom(WHTMLObject find, WHTMLObject set)
        {
            foreach (var obj in dom)
            {
                var q = obj;
                if (AddChildToObjDomRec(ref q, find, set)) return;
            }
        }
        bool AddChildToObjDomRec(ref WHTMLObject obj, WHTMLObject find, WHTMLObject set)
        {
            if (obj.Id == find.Id)
            {
                if (set.Type == TagType.text) obj.Attributes["text"] = set.OriginalTag.Text;
                else obj.Children.Add(set);
                return true;
            }
            foreach (var c in obj.Children)
            {
                var q = c;
                if (AddChildToObjDomRec(ref q, find, set)) return true;
            }
            return false;
        }

        public List<WHTMLObject> run(string html)
        {
            dom = new List<WHTMLObject>();
            Stack<WHTMLObject> objects = new Stack<WHTMLObject>();
            Stack<char> brackets = new Stack<char>();
            string currToken = "";
            foreach (char c in html)
            {
                if (c == '<')
                {
                    if (currToken.Trim().Length > 1)
                    {
                        List<char> chars = currToken.ToList();
                        chars.RemoveAt(0);
                        string txt = new string(chars.ToArray());
                        Tag t = new Tag();
                        t.InitText(txt);
                        //MessageBox.Show(txt);
                        WHTMLObject curr = WHTMLObject.Create(t);
                        if (objects.Count == 0)
                        {
                            dom.Add(curr);
                        }
                        else
                        {
                            AddChildToObjDom(objects.Peek(), curr);
                        }
                    }
                    currToken = "";
                    brackets.Push(c);
                }
                else if (c == '>' && brackets.Count > 0)
                {
                    brackets.Pop();
                    currToken += c;

                    Tag t = new Tag();
                    t.Init(currToken);

                    if (t.Closed)
                    {
                        objects.Pop();
                        currToken = "";
                        continue;
                    }
                    WHTMLObject curr = WHTMLObject.Create(t);
                    if (objects.Count == 0)
                    {
                        dom.Add(curr);
                        //objects.Push(curr);
                    }
                    else
                    {

                        AddChildToObjDom(objects.Peek(), curr);
                    }
                    if(!t.IsVoid) objects.Push(curr);
                    /*
                    if (t.opening) tags.Push(t);
                    else if (tags.Count > 0)
                    {
                        //handle top tag
                        Tag top = tags.Peek();
                        tags.Pop();
                    }
                    */
                    currToken = "";
                }
                currToken += c;
            }
            return dom;
        }
    }
}
