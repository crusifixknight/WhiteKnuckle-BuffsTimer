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
    
    private void FixedUpdate()
    {
        StringBuilder stringBuilder = new StringBuilder();
        if (Plugin.Buffs.Count > 0)
        {
            foreach (ItemBuff itemBuff in Plugin.Buffs)
            {
                if (itemBuff.IsBuffEnded()) return;

                var buffsAmount = itemBuff.GetBuffsAmount();
                var buffsMaxAmount = itemBuff.GetBuffsMaxAmount();
                stringBuilder.AppendLine(itemBuff.ToString());
                foreach (var buffAmount in buffsAmount.Where(buffAmount => Math.Round(buffAmount.Value, 2) != 0))
                {
                    stringBuilder.AppendLine(
                        $"\t<color=\"lightblue\">{buffAmount.Key} {buffAmount.Value:F2} ({buffsMaxAmount[buffAmount.Key]:F2})</color>");
                }
            }
        }
        _targetText.text = stringBuilder.ToString();
    }
}