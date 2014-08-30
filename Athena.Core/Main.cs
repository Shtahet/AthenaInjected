using System;
using System.Windows.Forms;
using Athena.Core.Forms;
using Athena.Core.Internal.DirectX;
using Athena.Core.Internal.GameManager;
using Athena.Core.Internal.GameManager.IngameObjects;
using Athena.Core.Patchables;

namespace Athena.Core
{
    public class Main
    {
        [STAThread]
        //This is the first thing called in our injected dll, Ensure to start your form and call any Initalize Functions you may need.
        public static int Run()
        {
            MessageBox.Show("");
            //OlusConsole.Show();
            //Logging.Write("Current WildStar version: " + OpenAPI.OpenApi.GetProcessFileVersion);
            //if (!OpenAPI.OpenApi.IsSupportedVersion)
            //{
            //    Logging.Write("Wildstar has been updated! Please wait for NXSBot to be updated! NXSBot only supports WildStar " + OpenAPI.OpenApi.GetSupportedVersion);
            //    return 0; //Don't load the bot, it will crash!
            //}

            //GeneralHelper.Initalize();
            //Pulsator.Initialize();
            //ClickToMove.Init();

            GeneralHelper.Initialize();
            Offsets.Initialize();
            WoWFunctions.Initialize(); //Some internal functions we can call :)
            ObjectManager.Initialize();
            Pulsator.Initialize();

            Application.EnableVisualStyles();
            Application.Run(new MainForm());

            return 0;
        }
    }
}
