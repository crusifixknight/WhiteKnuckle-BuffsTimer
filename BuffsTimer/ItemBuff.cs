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

    public List<BuffStat> GetBuffs()
    {
        var result = new List<BuffStat>();
        
        foreach (var hiddenBuff in Buffs.SelectMany(buff => buff.buffs))
        {
            var matchBuff = result.FirstOrDefault(b => b.BuffId == hiddenBuff.id);
            if (matchBuff != null)
            {
                matchBuff.Amount += hiddenBuff.amount;
                matchBuff.MaxAmount += hiddenBuff.maxAmount;
            }
            else
            {
                result.Add(new BuffStat(hiddenBuff.id, hiddenBuff.amount, hiddenBuff.maxAmount));
            }
        }

        return result;
    }
}

public class BuffStat(string buffId, float amount, float maxAmount)
{
    public string BuffId { get; } = buffId;
    public float Amount { get; set; } = amount;
    public float MaxAmount { get; set; }= maxAmount;
}