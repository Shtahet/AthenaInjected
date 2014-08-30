using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Athena.Core.Forms
{
    public static class RichTextBoxExtensions
    {
        static readonly Font RegularFont = new Font("Tahoma", 8, FontStyle.Regular);
        static readonly Font BoldFont = new Font("Tahoma", 8, FontStyle.Bold);

        public static void AppendText(this RichTextBox box, string text, Color color, bool bold)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;

            if (bold)
            {
                box.SelectionFont = BoldFont;
            }
            else
            {
                box.SelectionFont = RegularFont;
            }

            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
