

using UnityEngine;

public class Invincibility : Buff
{

    public Invincibility(Sprite icon) : base(BuffType.Invincibility,icon)
    {
        OnBuffStart = BuffStart;
        OnBuffEnd = BuffEnd;
        Level = GameData.GetBuffLevel(Type);
        Duration = BaseDuration;
    }

    public override Buff Clone()
    {
        return new Invincibility(Icon);
    }

    private void BuffEnd(PlayerController player)
    {
        player.IsInvincible = false;
    }

    private void BuffStart(PlayerController player)
    {
        player.IsInvincible = true;
    }

    
}
