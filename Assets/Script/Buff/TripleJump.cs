using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleJump : Buff
{
    protected int additionalJumps = 1; // Double jump + 1 additional
    
    private int baseDuration = 10;

    public TripleJump() 
    {
        Type = BuffType.TripleJump;
        Duration = baseDuration;
    }

    public override void OnBuffEnd(PlayerController player)
    {
        player.SubstractAdditionalJumps(additionalJumps);
    }

    public override void OnBuffStart(PlayerController player)
    {
        player.AddAdditionalJumps(additionalJumps);
    }

}
