using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BuffsTimer;

public class ItemBuff
{
    public string ItemPrefabName { get; }
    public List<BuffContainer> Buffs { get; }
    // public Sprite Icon { get; }

    public ItemBuff(string itemName, BuffContainer buff)
    {
        ItemPrefabName = itemName;
        Buffs = [ buff ];
        // TODO: Change Item name with item icons
        //Icon = 
    }

    public override string ToString()
    {
        float timer = GetTimer();
        var itemNameFormated = ItemPrefabName.Replace('_', ' ').Replace("Item", "").Replace("Denizen", ""); // Item_Food_Bar -> Food Bar, Denizen_SlugGrub -> SlugGrub
        
        return $"{itemNameFormated}: {timer:F2}s";
    }

    public bool IsBuffEnded()
    {
        if (!(GetTimer() <= 0.01)) return false;
        
        Plugin.RemoveBuff(this);
        return true;
    }

    private float GetTimer()
    {
        float multiplier = ENT_Player.playerObject.curBuffs.GetBuff("buffTimeMult");
        float timeMultiplier = 1f / Mathf.Max(1f + multiplier, 0.1f);
        float timer = Buffs.Last().buffTime / (Buffs.Last().loseRate * timeMultiplier);

        return timer;
    }

    public Dictionary<string, float> GetBuffsAmount()
    {
        var result = new Dictionary<string, float>();
        foreach (var hiddenBuff in Buffs.SelectMany(buff => buff.buffs))
        {
            if (result.ContainsKey(hiddenBuff.id)) result[hiddenBuff.id] += hiddenBuff.amount;
            else result.Add(hiddenBuff.id, hiddenBuff.amount);
        }
        return result;
    }

    public Dictionary<string, float> GetBuffsMaxAmount()
    {
        var result = new Dictionary<string, float>();
        foreach (var hiddenBuff in Buffs.SelectMany(buff => buff.buffs))
        {
            if (result.ContainsKey(hiddenBuff.id)) result[hiddenBuff.id] += hiddenBuff.maxAmount;
            else result.Add(hiddenBuff.id, hiddenBuff.maxAmount);
        }
        return result;
    }
}