using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffLevelCost : ScriptableObject
{
    public static readonly List<BuffLevel> levelingList = new List<BuffLevel>()
    {
        new BuffLevel(){Level = 0,Price = 100},
    };



}

public class BuffLevel
{
    public int Level;
    public int Price;
}