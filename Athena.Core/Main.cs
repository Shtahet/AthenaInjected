using System;
using System.Windows.Forms;
using Athena.Core.Forms;
using Athena.Core.Internal;
using Athena.Core.Internal.DirectX;
using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.GameManager.IngameObjects;
using Athena.Core.Patchables;
using Athena.Core.Tests;

namespace Athena.Core
{
    public class Main
    {
        [STAThread]
        //This is the first thing called in our injected dll, Ensure to start your form and call any Initalize Functions you may need.
        public static int Run()
        {
            MessageBox.Show("Attach if you want to debug!");

            GeneralHelper.Initialize();
            Offsets.Initialize();
            WoWFunctions.Initialize(); //Some internal functions we can call :)
            ObjectManager.Initialize();
            Pulsator.Initialize();
            WoWEvents.Initialize();

            Application.EnableVisualStyles();
            Application.Run(new MainForm());

            return 0;
        }
    }
}
