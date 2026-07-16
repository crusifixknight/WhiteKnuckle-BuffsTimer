using System.Linq;
using System.Text;
using BepInEx.Logging;
using HarmonyLib;

namespace BuffsTimer;

[HarmonyPatch(typeof(HandItem))]
public class HandItemP
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(HandItem.Start))]
    public static void Postfix(HandItem __instance)
    {
        void Action()
        {
            string prefabName = __instance.item.prefabName;

            /*For reasons unknown to me, hot cocoa conflicts with milk overwriting its time duration and vice versa.
             Therefore, if a player already has buffs from one of these items, 
             then when attempting to obtain a new buff from the conflicting item, it won't be added to the list.*/
            Plugin.Logger.LogInfo(prefabName);
            if (Plugin.Buffs.Any(b => (b.ItemPrefabName.Equals("Item_Milk") && prefabName.Equals("Item_Cocoa_Full")) 
                                      || (b.ItemPrefabName.Equals("Item_Cocoa_Full") && prefabName.Equals("Item_Milk")) ))
            {
                return;
            }

            BuffContainer buffContainer;
            switch (__instance)
            {
                case HandItem_Buff buff:
                    buffContainer = buff.buff;
                    break;
                case HandItem_Food food when food.useBuff:
                    buffContainer = food.buff;
                    break;
                default:
                    return;
            }
            
            if (Plugin.Buffs.Any(b => b.ItemPrefabName.Equals(prefabName)))
            {
                Plugin.Buffs.FirstOrDefault(b => b.ItemPrefabName.Equals(prefabName)).Buffs.Add(buffContainer);
                return;
            }

            ItemBuff itemBuff = new ItemBuff(prefabName, buffContainer);
            Plugin.Logger.LogInfo(itemBuff.ToString());
            Plugin.Buffs.Add(itemBuff);
        }

        __instance.itemUseEvent.AddListener(Action);
    }
}