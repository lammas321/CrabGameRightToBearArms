using BepInEx.IL2CPP.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace RightToBearArms
{
    internal class Utility
    {
        public static void GiveItem(ulong clientId, int itemId, int ammo)
        {
            if (!GameManager.Instance.activePlayers.ContainsKey(clientId) || GameManager.Instance.activePlayers[clientId].dead || !ItemManager.idToItem.ContainsKey(itemId))
                return;

            if (ItemManager.idToItem[itemId].type == ItemData_ItemType.Other || ItemManager.idToItem[itemId].type == ItemData_ItemType.Ammo)
            {
                ServerSend.DropItem(clientId, itemId, SharedObjectManager.Instance.GetNextId(), ammo);
                return;
            }

            if (ItemManager.idToItem[itemId].type == ItemData_ItemType.Melee || ItemManager.idToItem[itemId].type == ItemData_ItemType.Throwable || ammo == ItemManager.idToItem[itemId].currentAmmo)
            {
                GameServer.ForceGiveWeapon(clientId, itemId, SharedObjectManager.Instance.GetNextId());
                return;
            }

            LobbyManager.Instance.StartCoroutine(GiveItemCoroutine(clientId, itemId, ammo == -1 ? ItemManager.idToItem[itemId].currentAmmo : ammo));
        }
        public static IEnumerator GiveItemCoroutine(ulong clientId, int itemId, int ammo)
        {
            int uniqueObjectId = SharedObjectManager.Instance.GetNextId();
            ServerSend.DropItem(clientId, itemId, uniqueObjectId, ammo);
            if (ItemManager.idToItem[itemId].type == ItemData_ItemType.Other || ItemManager.idToItem[itemId].type == ItemData_ItemType.Ammo)
                yield break;

            while (!SharedObjectManager.Instance.field_Private_Dictionary_2_Int32_MonoBehaviourPublicInidBoskUnique_0.ContainsKey(uniqueObjectId))
                yield return new WaitForEndOfFrame();
            if (!LobbyManager.steamIdToUID.ContainsKey(clientId))
                yield break;

            yield return new WaitForSeconds(Mathf.Min((LobbyManager.Instance.field_Private_ArrayOf_ObjectPublicBoInBoCSItBoInSiBySiUnique_0[LobbyManager.steamIdToUID[clientId]].field_Public_Int32_0 + 50) / 1000f, 1));
            if (!LobbyManager.steamIdToUID.ContainsKey(clientId) || !SharedObjectManager.Instance.field_Private_Dictionary_2_Int32_MonoBehaviourPublicInidBoskUnique_0.ContainsKey(uniqueObjectId))
                yield break;

            Packet packet = new(uniqueObjectId);
            packet.field_Private_ArrayOf_Byte_0 = BitConverter.GetBytes(uniqueObjectId);
            ServerHandle.TryInteract(clientId, packet);
        }
    }
}
