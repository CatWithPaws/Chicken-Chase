using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public abstract class Buff
{
    public float Duration;
    public BuffType Type;

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