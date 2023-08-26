using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Bridge : Buff
{

    public Bridge(Sprite icon) : base(BuffType.Bridge, icon)
    {
        Level = GameData.GetBuffLevel(Type);
        Duration = BaseDuration;
    }

    public override Buff Clone()
    {
        return new Bridge(Icon);
    }
}
