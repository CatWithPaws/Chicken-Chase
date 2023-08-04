

using UnityEngine;

public class Invincibility : Buff
{

    public Invincibility() 
    {
        Type = BuffType.Invincibility;
        baseDuration = 10;
        Duration = baseDuration;
    }

    public override Buff Clone()
    {
        var clone = new Invincibility();
        clone.Icon = Icon;
        return clone;
    }

    public override void OnBuffEnd(PlayerController player)
    {
        player.IsInvincible = false;
    }

    public override void OnBuffStart(PlayerController player)
    {
        player.IsInvincible = true;
    }
}
