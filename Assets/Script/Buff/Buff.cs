using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff
{
    public int Duration;
    public BuffType Type;

    public abstract void OnBuffStart(PlayerController player);
    public abstract void OnBuffEnd(PlayerController player);

}

public enum BuffType
{
    TripleJump,
    Invincibility
}