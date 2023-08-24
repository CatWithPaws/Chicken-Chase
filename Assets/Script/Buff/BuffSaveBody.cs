using System;
using System.Collections.Generic;

[Serializable]
public class BuffSaveBody
{
    Dictionary<BuffType,int> buffLevels = new Dictionary<BuffType,int>();
}
