using System;
using System.Collections.Generic;

[Serializable]
public class PlayerSaveBody
{
    public int Money { get; set; }
    public List<int> PlayerSkins { get; private set; }
    public List<int> ChickenSkins { get; private set; }
    public BuffSaveBody Buffs { get; private set; }

    public PlayerSaveBody()
    {
        Buffs = new BuffSaveBody();
        PlayerSkins = new List<int>();
        ChickenSkins = new List<int>();
        PlayerSkins.Add(0);
        ChickenSkins.Add(0);
#if UNITY_EDITOR
        Money = 10000;
#else
        Money = 0;
#endif

    }
}
