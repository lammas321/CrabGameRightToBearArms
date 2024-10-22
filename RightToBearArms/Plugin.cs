using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using System.Globalization;

namespace RightToBearArms
{
    [BepInPlugin($"lammas123.{MyPluginInfo.PLUGIN_NAME}", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("lammas123.ChatCommands")]
    public class RightToBearArms : BasePlugin
    {
        public override void Load()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            ChatCommands.Api.RegisterCommand(new ItemsCommand());
            ChatCommands.Api.RegisterCommand(new GiveCommand());
            
            Harmony.CreateAndPatchAll(typeof(Patches));
            Log.LogInfo($"Loaded [{MyPluginInfo.PLUGIN_NAME} {MyPluginInfo.PLUGIN_VERSION}]");
        }
    }
}