using System.Linq;
using HarmonyLib;

namespace BuffsTimer;

[HarmonyPatch(typeof(HandItem_Buff))]
public class HandItem_BuffP
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(HandItem_Buff.Activate))]
    public static void Postfix(HandItem_Buff __instance)
    {
        var prefabName = __instance.item.prefabName;
        Plugin.Logger.LogInfo(prefabName);
            
        var buffContainer = __instance.buff;
            
        if (Plugin.Buffs.Any(b => b.ItemPrefabName.Equals(prefabName)))
        {
            Plugin.Buffs.FirstOrDefault(b => b.ItemPrefabName.Equals(prefabName))?.Buffs.Add(buffContainer);
            return;
        }
        
        Plugin.AddBuff(prefabName, buffContainer);
    }
}