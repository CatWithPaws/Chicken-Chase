using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsLoader : MonoBehaviour
{
    [SerializeField] SkinComponent playerSkin;
    [SerializeField] SkinComponent chickenSkin;

    private void Awake()
    {
        GameData.OnSkinUpdate += UpdateSkins;
        UpdateSkins();
    }

    private void UpdateSkins()
    {
        playerSkin.SetSkin(GameData.GetCurrentPlayerSkin());
        chickenSkin.SetSkin(GameData.GetCurrentChickenSkin());
    } 
}
