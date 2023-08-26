using System;
using System.Collections.Generic;

[Serializable]
public class BuffSaveBody
{
    public Dictionary<BuffType,int> buffLevels = new Dictionary<BuffType,int>();

    public BuffSaveBody()
    {
        this.buffLevels = new Dictionary<BuffType, int>();
        for(int i = 0;i < (int)BuffType.Count; i++)
        {
            buffLevels.Add((BuffType)i, 1);
        }
    }
}
