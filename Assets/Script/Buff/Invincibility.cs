

public class Invincibility : Buff
{
    private int baseDuration = 5;

    public Invincibility()
    {
        Type = BuffType.Invincibility;
        Duration = baseDuration;
    }

    public override Buff Clone()
    {
        return new Invincibility();
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
