using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillHealthItem : Item
{
    public override void Initialize ()
    {
        PlayerController.inst.Heal(PlayerController.inst.maxHp);
        DestroyItem();
    }
}