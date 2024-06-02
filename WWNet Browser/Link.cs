using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using WWNet_Browser.FeatherInterp;

namespace WWNet_Browser
{
    internal class Link : LinkLabel
    {
        public Link()
        {
            this.LinkColor = Color.FromArgb(0x1A, 0x0D, 0xBE);
            this.VisitedLinkColor = Color.FromArgb(0x68, 0x1D, 0xA8);
            this.ActiveLinkColor = Color.FromArgb(0x1A, 0x0D, 0xBE);
            this.LinkBehavior = LinkBehavior.HoverUnderline;
        }

        const int IDC_HAND = 32649;
        const int WM_SETCURSOR = 0x0020;
        const int HTCLIENT = 1;
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SetCursor(HandleRef hcursor);

        static readonly Cursor SystemHandCursor =
            new Cursor(LoadCursor(IntPtr.Zero, IDC_HAND));
        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == WM_SETCURSOR)
                WmSetCursor(ref msg);
            else
                base.WndProc(ref msg);
        }
        void WmSetCursor(ref Message m)
        {
            if (m.WParam == (IsHandleCreated ? Handle : IntPtr.Zero) &&
               (unchecked((int)(long)m.LParam) & 0xffff) == HTCLIENT)
            {
                if (OverrideCursor != null)
                {
                    if (OverrideCursor == Cursors.Hand)
                        SetCursor(new HandleRef(SystemHandCursor, SystemHandCursor.Handle));
                    else
                        SetCursor(new HandleRef(OverrideCursor, OverrideCursor.Handle));
                }
                else
                {
                    SetCursor(new HandleRef(Cursor, Cursor.Handle));
                }
            }
            else
            {
                DefWndProc(ref m);
            }
        }
    }
}
