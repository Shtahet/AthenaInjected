using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Athena.Core.Internal.DirectX.Drawing;
using Athena.Core.Internal.DirectX.Drawing.Drawables;
using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.GameManager.IngameObjects;
using Athena.Core.Internal.Objects;
using Athena.Core.Internal.Scripts;
using Athena.Core.Patchables;
using Athena.Core.Tests;

namespace Athena.Core.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GeneralHelper.MainForm = this;
            SetupScripts();

            GeneralHelper.MainLog("Ready", "Athena", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawableString test = new DrawableString(textBox1.Text, int.Parse(textBox2.Text), int.Parse(textBox3.Text));
            Rendering.AddDrawable(test);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ObjectManager.Objects.Count.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DrawableCircleAroundObject testAroundObject = new DrawableCircleAroundObject(ObjectManager.LocalPlayer, 5, Color.Red, Color.Black);
            Rendering.AddDrawable(testAroundObject);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //DrawableLine line = new DrawableLine(ObjectManager.LocalPlayer.Location, new Location(0,0,0), Color.Red );
            DrawableLineFromObject line = new DrawableLineFromObject(ObjectManager.LocalPlayer, new Location(0, 0, 0), Color.Red);
            Rendering.AddDrawable(line);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DrawableImageInWorld img = new DrawableImageInWorld(@"C:\Users\Ashley\Desktop\DownArrow.png",
                ObjectManager.LocalPlayer.Location);
            Rendering.AddDrawable(img);
        }

        private void button6_Click(object sender, EventArgs e)
        {

            foreach (WoWObject obj in ObjectManager.Objects)
            {
                DrawableBox box = new DrawableBox(obj, 0, 1, 1, Color.Gold, 155);
                Rendering.AddDrawable(box);

                //DrawableCircleAroundObject cir = new DrawableCircleAroundObject(obj, 1, Color.Gold, Color.Yellow);
                //Rendering.AddDrawable(cir);
            }
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ObjectManager.LocalPlayer.Name + "    " + ObjectManager.LocalPlayer.Location.ToString());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ObjectManager.LocalPlayer.Target.Name + "    " + ObjectManager.LocalPlayer.Location.ToString());
        }

        private void button9_Click(object sender, EventArgs e)
        {
        }


        #region Scripts tab

        private void SetupScripts()
        {
            GeneralHelper.Scripts.ScriptRegistered += OnScriptRegisteredEvent;

            foreach (var s in GeneralHelper.Scripts.Scripts)
            {
                s.OnStartedEvent += OnScriptStartedEvent;
                s.OnStoppedEvent += OnScriptStoppedEvent;
            }

            lstScripts.DataSource = GeneralHelper.Scripts.Scripts.OrderBy(x => x.Category).ToList();
            //Log.WriteLine(LogType.Information, "Loaded {0} scripts.", GeneralHelper.Scripts.Scripts.Count);
        }

        private Script SelectedScript;

        private void btnScriptStart_Click(object sender, EventArgs e)
        {
            if (SelectedScript == null)
                return;

            SelectedScript.Start();
        }

        private void btnScriptStop_Click(object sender, EventArgs e)
        {
            if (SelectedScript == null)
                return;

            SelectedScript.Stop();
        }

        private void lstScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            var script = lstScripts.SelectedItem as Script;
            if (script == null)
                return;

            SelectedScript = script;
        }

        private void lstScripts_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var script = lstScripts.Items[e.Index] as Script;
            if (e.NewValue == CheckState.Checked)
                script.Start();
            else if (e.NewValue == CheckState.Unchecked)
                script.Stop();
        }

        private void OnScriptRegisteredEvent(object sender, EventArgs e)
        {
            var script = sender as Script;
            script.OnStartedEvent += OnScriptStartedEvent;
            script.OnStoppedEvent += OnScriptStoppedEvent;
            lstScripts.Invoke((Action)(() =>
            {
                lstScripts.DataSource = GeneralHelper.Scripts.Scripts.OrderBy(x => x.Category).ToList();
                lstScripts.Invalidate();
            }));
        }

        private void OnScriptStartedEvent(object sender, EventArgs e)
        {
            var idx = lstScripts.Items.IndexOf(sender);
            lstScripts.Invoke((Action)(() => lstScripts.SetItemCheckState(idx, CheckState.Checked)));
        }

        private void OnScriptStoppedEvent(object sender, EventArgs e)
        {
            var idx = lstScripts.Items.IndexOf(sender);
            lstScripts.Invoke((Action)(() => lstScripts.SetItemCheckState(idx, CheckState.Unchecked)));
        }

        #endregion

        private ulong OldGUID = 0;
        private void button12_Click(object sender, EventArgs e)
        {
            GeneralHelper.MainLog(ObjectManager.LocalPlayer.Pointer.ToString("X"), "wow");
            OldGUID = ObjectManager.LocalPlayer.Guid;
            /*WoWPacket moverPacket = new WoWPacket(0x002708E1);
            moverPacket.Set<ulong>(0x10, 0);
            moverPacket.Send();*/
            WoWFunctions._SetActiveMover(0, true);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            WoWFunctions._SetActiveMover(OldGUID, true);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            WoWPacket telePacket = new WoWPacket(0x00270CA6);
            uint ClientConnection = WoWFunctions._ClientConnection();
            int tick = GeneralHelper.Memory.Read<int>(Offsets.Packet.OsTick);
            uint oldFlags = GeneralHelper.Memory.Read<uint>(
                GeneralHelper.Memory.Read<uint>(ObjectManager.LocalPlayer + 0xEC)
                + 0x38);

            GeneralHelper.Memory.Write<uint>(
                GeneralHelper.Memory.Read<uint>(ObjectManager.LocalPlayer + 0xEC)
                + 0x38, 1);

            //telePacket.fillInMovementStatusWithFlags(moveTiming, 1);
            telePacket.Set<int>(0xB0, 0); // Unknown
            telePacket.Set<float>(0x24, ObjectManager.LocalPlayer.Location.X);
            telePacket.Set<float>(0x28, ObjectManager.LocalPlayer.Location.Y);
            telePacket.Set<float>(0x2C, ObjectManager.LocalPlayer.Location.Z);

            //telePacket.Set<float>(0x54, tick);

            telePacket.Set<bool>(0x78 + 0x10, true);           // send speed data
            telePacket.Set<float>(0x74 + 0x10, 0);             // speed value or something
            telePacket.Set<float>(0x68 + 0x10, -7.955547333f); // velocity
            

            /*telePacket.Set<bool>(0x78, true);           // send speed data
            telePacket.Set<float>(0x74, 0);             // speed value or something
            telePacket.Set<float>(0x68, -7.955547333f); // velocity*/


            telePacket.Send();


            GeneralHelper.Memory.Write<uint>(
                GeneralHelper.Memory.Read<uint>(ObjectManager.LocalPlayer + 0xEC)
                + 0x38, oldFlags);
        }

    }
}
