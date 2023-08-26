using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Buff
{
    public delegate void OnBuffDelegate(PlayerController player);
    public float Duration;
    protected float baseDuration = 10;
    public float BaseDuration => baseDuration + AdditionalDurationPerLevel * Level;
    public BuffType Type = BuffType.Count;
    public Sprite Icon = null;
    public int Level = 1;
    public float AdditionalDurationPerLevel = 2;
    public OnBuffDelegate OnBuffStart;
    public OnBuffDelegate OnBuffEnd;

    public Buff(BuffType type, Sprite icon, int level = 1,OnBuffDelegate OnBuffStart = default, OnBuffDelegate OnBuffEnd = default,float additionalDurationPerLevel = 2)
    {
        Type = type;
        Icon = icon;
        Level = level;
        this.OnBuffStart = OnBuffStart == default ? (x) => { } : OnBuffStart;
        this.OnBuffEnd = OnBuffEnd == default ? (x) => { } : OnBuffEnd;
        AdditionalDurationPerLevel = additionalDurationPerLevel;
    }

    public abstract Buff Clone();
}

public enum BuffType
{
    TripleJump,
    Invincibility,
    Bridge,


    Count
}