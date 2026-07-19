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
                
                var buffs = itemBuff.GetBuffs();
                foreach (var buff in buffs.Where(b => Math.Round(b.Amount, 2) != 0))
                {
                    if (!BuffNames.Dict.TryGetValue(buff.BuffId, out var buffName)) continue;
                    stringBuilder.AppendLine(
                        $"\t<color=\"lightblue\">{buffName} {buff.Amount:F2} ({buff.MaxAmount:F2})</color>");
                }
            }
        }
        _targetText.text = stringBuilder.ToString();
    }
}