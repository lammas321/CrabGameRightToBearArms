using HarmonyLib;

namespace RightToBearArms
{
    internal static class Patches
    {
        //   Anti Bepinex detection (Thanks o7Moon: https://github.com/o7Moon/CrabGame.AntiAntiBepinex)
        [HarmonyPatch(typeof(EffectManager), nameof(EffectManager.Method_Private_Void_GameObject_Boolean_Vector3_Quaternion_0))] // Ensures effectSeed is never set to 4200069 (if it is, modding has been detected)
        [HarmonyPatch(typeof(LobbyManager), nameof(LobbyManager.Method_Private_Void_0))] // Ensures connectedToSteam stays false (true means modding has been detected)
        // [HarmonyPatch(typeof(Deobf_MenuSnowSpeedModdingDetector), nameof(Deobf_MenuSnowSpeedModdingDetector.Method_Private_Void_0))] // Would ensure snowSpeed is never set to Vector3.zero (though it is immediately set back to Vector3.one due to an accident on Dani's part lol)
        [HarmonyPrefix]
        internal static bool PreBepinexDetection()
            => false;


        // Renames the Rifle and Double Barrel to AK and Dual Shotgun to make them useable, and makes the Milk and Pizza Steve items equipable
        [HarmonyPatch(typeof(ItemManager), nameof(ItemManager.Awake))]
        [HarmonyPostfix]
        internal static void PostItemManagerAwake()
        {
            ItemManager.idToItem[0].itemName = "AK";
            ItemManager.idToItem[3].itemName = "Dual Shotgun";
            ItemManager.idToItem[11].type = ItemData_ItemType.Throwable;
            ItemManager.idToItem[12].itemName = "Pizza Steve";
            ItemManager.idToItem[12].type = ItemData_ItemType.Throwable;
        }
    }
}