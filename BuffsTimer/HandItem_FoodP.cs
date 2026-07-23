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
        if (Plugin.Buffs.Any(b => (b.ItemPrefabName.Contains("Item_Milk") && prefabName.Contains("Item_Cocoa")) || (b.ItemPrefabName.Contains("Item_Cocoa") && prefabName.Contains("Item_Milk")))) 
            return;
        
        var buffContainer = __instance.buff;

        /*
         * Same thing as with mother instinct perk
         */
        var buff = Plugin.Buffs.FirstOrDefault(b => b.ItemPrefabName.Equals("Item_Beans_Periphery"));
        if(buff != null)
        {
            buff.Buffs[0] = buffContainer;
            return;
        }
        
        if (Plugin.Buffs.Any(b => b.ItemPrefabName.Equals(prefabName)))
        {
            Plugin.Buffs.FirstOrDefault(b => b.ItemPrefabName.Equals(prefabName))?.Buffs.Add(buffContainer);
            return;
        }

        Plugin.AddBuff(prefabName, buffContainer);
    }
}