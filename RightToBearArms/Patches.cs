using HarmonyLib;

namespace RightToBearArms
{
    internal static class Patches
    {
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