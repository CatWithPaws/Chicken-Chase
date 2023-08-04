
using UnityEngine;

public class TripleJump : Buff
{
    protected int additionalJumps = 1; // Double jump + 1 additional
    

    public TripleJump()
    {
        Type = BuffType.TripleJump;
        baseDuration = 10;
        Duration = baseDuration;
    }

    public override Buff Clone()
    {
        var clone = new TripleJump();
        clone.Icon = Icon;
        return clone;
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
