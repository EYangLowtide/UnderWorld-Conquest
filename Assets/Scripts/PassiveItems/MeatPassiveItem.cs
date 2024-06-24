using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatPassiveItem : PassiveItems
{
    protected override void ApplyModifier()
    {
        player.CurrentGuts *= 1 + passiveItemData.Multiplier / 100f;
    }
}
