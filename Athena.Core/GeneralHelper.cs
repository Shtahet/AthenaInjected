using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Athena.Core.Forms;
using Athena.Core.Internal.DirectX;
using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.Objects;
using Athena.Core.Internal.Scripts;
using GreyMagic;

namespace Athena.Core
{
    public static class GeneralHelper
    {
        public static Color OurColor = Color.FromArgb(255, 237, 62, 73);

        public static MainForm MainForm;
        public static InProcessMemoryReader Memory { get; private set; }
        public static ObjectManager ObjectManager { get; private set; }
        public static ScriptManager Scripts { get; private set; }
        public static SpellManager SpellManager { get; private set; }
        public static void Initialize()
        {
            Memory = new InProcessMemoryReader(GetCurrentProcess);

            Pulsator.RegisterCallbacks(
               ObjectManager = new ObjectManager(),
               Scripts = new ScriptManager(),
               SpellManager = new SpellManager()
               );
        }

        public static Process GetCurrentProcess
        {
            get { return Process.GetCurrentProcess(); }
        }

        public static uint RebaseAddress(uint addr)
        {

            return (uint) (GetCurrentProcess.MainModule.BaseAddress + (int) addr);
        }

        public static void WriteGUID(uint allocatedMemory, WoWGuidWoD guid)
        {
            Memory.Write<ulong>(allocatedMemory, guid.Low);
            Memory.Write<ulong>(allocatedMemory + 0x8, guid.High);
        }



        private static string lastLog = "";
        public static void MainLog(string msg, string process, bool bold = false)
        {
            if (lastLog == msg)
                return;

            if (MainForm.LogBox.InvokeRequired)
            {
                MainForm.LogBox.BeginInvoke(new MethodInvoker(() => MainLog(msg, process, bold)));
            }
            else
            {
                MainForm.LogBox.AppendText("[" + DateTime.Now.ToLongTimeString() + "]", Color.Black, false);
                MainForm.LogBox.AppendText(": ", Color.Black, false);
                MainForm.LogBox.AppendText("[" + process + "]", GeneralHelper.OurColor, true);
                MainForm.LogBox.AppendText(" ", Color.Black, false);

                bool textBold = bold;
                MainForm.LogBox.AppendText(msg, Color.Black, textBold);

                MainForm.LogBox.AppendText(Environment.NewLine);

                MainForm.LogBox.SelectionLength = 0;
                MainForm.LogBox.SelectionStart = MainForm.LogBox.Text.Length;

                MainForm.LogBox.ScrollToCaret();
                lastLog = msg;
            }
        }
        
    }
}
