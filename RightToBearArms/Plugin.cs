using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using System;
using System.Globalization;
using System.Reflection;

namespace RightToBearArms
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("lammas123.ChatCommands", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("lammas123.CrabDevKit", BepInDependency.DependencyFlags.SoftDependency)]
    public sealed class RightToBearArms : BasePlugin
    {
        public override void Load()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            if (ChatCommandsCompatibility.Enabled) // ChatCommands depends on CrabDevKit. If we have ChatCommands, we have CrabDevKit
            {
                Assembly asm = typeof(RightToBearArms).Assembly;
                
                Type itemsCommandType = asm.GetType("RightToBearArms.ItemsCommand");
                Type giveCommandType = asm.GetType("RightToBearArms.GiveCommand");

                ChatCommandsCompatibility.RegisterCommand(itemsCommandType);
                ChatCommandsCompatibility.RegisterCommand(giveCommandType);
            }

            Harmony harmony = new(MyPluginInfo.PLUGIN_NAME);
            harmony.PatchAll(typeof(Patches));

            Log.LogInfo($"Initialized [{MyPluginInfo.PLUGIN_NAME} {MyPluginInfo.PLUGIN_VERSION}]");
        }
    }
}