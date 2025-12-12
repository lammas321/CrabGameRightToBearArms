using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using System;
using System.Globalization;
using System.Reflection;

namespace RightToBearArms
{
    [BepInPlugin($"lammas123.{MyPluginInfo.PLUGIN_NAME}", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("lammas123.ChatCommands", BepInDependency.DependencyFlags.SoftDependency)]
    public class RightToBearArms : BasePlugin
    {
        public override void Load()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            if (ChatCommandsCompatibility.Enabled)
            {
                Assembly asm = typeof(RightToBearArms).Assembly;
                
                Type itemsCommandType = asm.GetType("RightToBearArms.ItemsCommand");
                Type giveCommandType = asm.GetType("RightToBearArms.GiveCommand");

                ChatCommandsCompatibility.RegisterCommand(itemsCommandType);
                ChatCommandsCompatibility.RegisterCommand(giveCommandType);
            }
            
            Harmony.CreateAndPatchAll(typeof(Patches));
            Log.LogInfo($"Loaded [{MyPluginInfo.PLUGIN_NAME} {MyPluginInfo.PLUGIN_VERSION}]");
        }
    }
}