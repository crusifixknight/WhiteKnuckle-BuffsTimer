using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace BuffsTimer;

public class BuffsMonitorComponent : MonoBehaviour
{
    private Text _targetText;

    public void Setup(Text targetText)
    {
        _targetText = targetText;
    }
    
    private void Update()
    {
        StringBuilder stringBuilder = new StringBuilder();
        if (Plugin.Buffs.Count > 0)
        {
            foreach (ItemBuff itemBuff in Plugin.Buffs)
            {
                BuffContainer buffContainer = itemBuff.Buffs.Last();
                if (buffContainer.buffTime/buffContainer.loseRate <= 0.1f || !buffContainer.loseOverTime) Plugin.Buffs.Remove(itemBuff);
                else
                {
                    stringBuilder.AppendLine(itemBuff.ToString());
                    var buffsAmount = itemBuff.GetBuffsAmount();
                    var buffsMaxAmount = itemBuff.GetBuffsMaxAmount();
                    foreach (var buffAmount in buffsAmount)
                    {
                        if (Math.Round(buffAmount.Value, 2) != 0)
                            stringBuilder.AppendLine($"\t<color=\"lightblue\">{buffAmount.Key} {buffAmount.Value:F2} ({buffsMaxAmount[buffAmount.Key]:F2})</color>");
                    }
                }
            }
        }
        _targetText.text = stringBuilder.ToString();
    }
}