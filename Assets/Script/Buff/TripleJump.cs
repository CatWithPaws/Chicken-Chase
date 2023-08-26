
using UnityEngine;

public class TripleJump : Buff
{
    protected int additionalJumps = 1; // Double jump + 1 additional


    public TripleJump(Sprite icon) : base(BuffType.TripleJump, icon)
    {
        OnBuffStart = BuffStart; 
        OnBuffEnd = BuffEnd;

        Level = GameData.GetBuffLevel(Type);

        Duration = BaseDuration;
    }

    public  void BuffEnd(PlayerController player)
    {
        player.SubstractAdditionalJumps(additionalJumps);
    }

    public  void BuffStart(PlayerController player)
    {
        player.AddAdditionalJumps(additionalJumps);
    }

    public override Buff Clone()
    {
        return new TripleJump(Icon);
    }
}
