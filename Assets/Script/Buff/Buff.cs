using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Buff
{
    public float Duration;
    protected float baseDuration;
    public float BaseDuration => baseDuration;
    public BuffType Type;
    public Sprite Icon;
    public int Level;
    public float additionalDurationPerLevel;
    public abstract void OnBuffStart(PlayerController player);
    public abstract void OnBuffEnd(PlayerController player);

    public abstract Buff Clone();

}

public enum BuffType
{
    TripleJump,
    Invincibility,



    Count
}