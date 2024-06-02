using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WWNet_Browser.FeatherInterp
{
    internal static class IdCounter
    {
        internal static int c = 0;
    }
    public class WHTMLObject
    {
        public List<WHTMLObject> Children = new List<WHTMLObject>();
        public Dictionary<string, string> Attributes = new Dictionary<string, string>();
        public TagType Type;
        public int Id;
        public Tag OriginalTag;
        void ParseArgString(string arg)
        {
            string attrib = arg.Substring(0, arg.IndexOf('='));
            string val = arg.Substring(arg.IndexOf('=') + 2);
            val = val.Substring(0, val.Length - 1);
            Attributes[attrib] = val;
        }
        void ParseArgs(string[] args)
        {
            if (args == null || args.Length == 1) return;
            string argFull = args[1];
            int j = 2;
            start:
            while (argFull.Where(x => x == '\"').Count() % 2 == 1 && j < args.Length)
            {
                argFull += " " + args[j];
                j++;
            }
            ParseArgString(argFull);
            argFull = "";
            if (j >= args.Length) return;
            else goto start;
        }
        public static WHTMLObject Create(Tag t)
        {
            WHTMLObject w = new WHTMLObject();
            w.OriginalTag = t;
            w.Type = t.Type;
            w.Id = IdCounter.c;
            w.ParseArgs(t.Arguments);
            IdCounter.c++;
            return w;
        }
    }
}
