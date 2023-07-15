
public class TripleJump : Buff
{
    protected int additionalJumps = 1; // Double jump + 1 additional
    
    private int baseDuration = 10;

    public TripleJump() 
    {
        Type = BuffType.TripleJump;
        Duration = baseDuration;
    }

    public override Buff Clone()
    {
        return new TripleJump();
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
