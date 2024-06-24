using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinichPassiveItem : PassiveItems
{
    protected override void ApplyModifier()
    {
        player.CurrentStrength *= 1 + passiveItemData.Multiplier / 100f;
    }
}
