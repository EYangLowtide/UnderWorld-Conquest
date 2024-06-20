using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinichPassiveItem : PassiveItems
{
    protected override void ApplyModifier()
    {
        player.currentStrength *= 1 + passiveItemData.Multiplier / 100f;
    }
}
