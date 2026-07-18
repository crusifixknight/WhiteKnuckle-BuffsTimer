using System.Linq;
using HarmonyLib;

namespace BuffsTimer;

[HarmonyPatch(typeof(HandItem_Buff_Grub))]
public class HandItem_Buff_GrubP
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(HandItem_Buff_Grub.Activate))]
    public static void Postfix(HandItem_Buff_Grub __instance)
    {
        if (!__instance.CanPet()) return;
        
        var prefabName = __instance.item.prefabName;
        var buffContainer = __instance.grubPetBuff;
    
        /*
         * Grub pet buffs doesn't stack. That's why we are rewriting previous buff
         */ 
        foreach(var buff in Plugin.Buffs.Where(b => b.Buffs.Any(bc => bc == buffContainer)))
        {
            buff.Buffs[0] = buffContainer;
            return;
        }
        
        Plugin.AddBuff(prefabName, buffContainer);
    }
}