using System.Linq;
using HarmonyLib;

namespace BuffsTimer;

[HarmonyPatch(typeof(HandItem_Food))]
public class HandItem_FoodP
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(HandItem_Food.Eat))]
    public static void Postfix(HandItem_Food __instance)
    {
        if (!__instance.useBuff) return;
        
        var prefabName = __instance.item.prefabName;
        
        /*For reasons unknown to me, hot cocoa conflicts with milk overwriting its time duration and vice versa.
         *Therefore, if a player already has buffs from one of these items,
         *then when attempting to obtain a new buff from the conflicting item, it won't be added to the list.
        */
        if (Plugin.Buffs.Any(b => (b.ItemPrefabName.Equals("Item_Milk") && prefabName.Equals("Item_Cocoa_Full")) || (b.ItemPrefabName.Equals("Item_Cocoa_Full") && prefabName.Equals("Item_Milk")))) 
            return;
        
        var buffContainer = __instance.buff;
            
        if (Plugin.Buffs.Any(b => b.ItemPrefabName.Equals(prefabName)))
        {
            Plugin.Buffs.FirstOrDefault(b => b.ItemPrefabName.Equals(prefabName))?.Buffs.Add(buffContainer);
            return;
        }

        Plugin.AddBuff(prefabName, buffContainer);
    }
}