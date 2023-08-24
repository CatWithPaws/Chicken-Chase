using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GiantChickenSkins",menuName = "ScriptableObject/CreateGiantChickenSkinsHolder")]
public class GiantChickenSkins : ScriptableObject
{

    public List<Skin> skins = new List<Skin>();


    private void OnValidate()
    {
        for (int i = 0; i < skins.Count; i++)
        {
            skins[i].ID = i;
        }
    }
}
