using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Thi_CTDL_WF
{
    public class TextBoxWriter : TextWriter
    {
        // The control where we will write text.
        private readonly Control MyControl;
        public TextBoxWriter(Control control)
        {
            MyControl = control;
        }

        public override void Write(char value)//ERROR: This cause stack overflow, DO NOT USE Console.WriteLine(); !!!
        {
            MyControl.Text += value;
        }

        public override void Write(string value)
        {
            MyControl.Text += value;
        }

        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }
    }
}