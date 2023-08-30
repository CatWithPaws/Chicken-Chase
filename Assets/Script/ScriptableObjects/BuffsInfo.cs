using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BuffInfos",menuName = "ScriptableObject/Create BuffInfo")]
public class BuffsInfo : ScriptableObject
{
    public List<BuffInfo> Buffs = new List<BuffInfo>();
}

[Serializable]
public class BuffInfo
{
    public Sprite Sprite;
    public BuffType BuffType;
    public string Name;
    public Buff buff;
}