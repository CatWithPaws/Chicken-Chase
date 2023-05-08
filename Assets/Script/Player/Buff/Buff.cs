using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff
{
    public readonly float BuffBaseDuration;
}

public enum BuffEffects
{
    Invincible = 0,
    Magnet = 1
}