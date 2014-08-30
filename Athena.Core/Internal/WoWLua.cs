using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Athena.Core.Internal.GameManager;

namespace Athena.Core.Internal
{
    public static class WoWLua
    {
        public static void ExecuteBuffer(string command)
        {
            WoWFunctions._doString(command, command, 0);
        }

        public static string GetLocalizedText(string returnValue)
        {
            uint returnVal = WoWFunctions._GetLocalizedText(ObjectManager.LocalPlayer.Pointer, returnValue, -1);

            return GeneralHelper.Memory.ReadString(returnVal, new UTF8Encoding());
        }

        public static string[] GetReturnValues(string command, string Argument)
        {
            return GetReturnValues(command, new[] {Argument});
        }

        public static string[] GetReturnValues(string command, string[] Arguments)
        {
            string[] returnStrings = new string[Arguments.Length];

            ExecuteBuffer(command);
            HideLuaErrors();

            for (int index = 0; index < Arguments.Length; ++index)
            {
                string argResult = GetLocalizedText(Arguments[index]);
                returnStrings[index] = argResult;
            }

            return returnStrings;
        }
        
        private static void HideLuaErrors()
        {
            string str = "if ScriptErrors ~= nil then if ScriptErrors:IsVisible() then ScriptErrors:Hide(); end end";
            ExecuteBuffer(str);
        }
    }
}
