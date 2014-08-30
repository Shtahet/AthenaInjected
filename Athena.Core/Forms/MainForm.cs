using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Athena.Core.Internal.Drawing;
using Athena.Core.Internal.Drawing.Drawables;
using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.GameManager.IngameObjects;
using Athena.Core.Internal.Objects;
using Athena.Core.Internal.Scripts;

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
    }
}
