using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Delegates;

public class SkinShop : MonoBehaviour
{
    
    [SerializeField] SkinShopCell[] cells;

    [SerializeField] GameObject wrapper;
    [SerializeField] GameObject startGameUI;

    public SkinShopCell equipedCell;

    public VoidFunc OnShopUpdate;

    private delegate int GetSkinsCountDelegate();

    private GetSkinsCountDelegate[] GetSkinsCount = new GetSkinsCountDelegate[]
    {
        GameData.playerSkinsCount,
        GameData.chickenSkinsCount
    };

    public SkinShopCell.WhoseSkin whoseSkinShop = SkinShopCell.WhoseSkin.Player;

    private void Awake()
    {
        OnShopUpdate += UpdateShop;
    }

    private void OnEnable()
    {
        ChangeToPlayerShop();
    }

    public void ChangeToPlayerShop()
    {
        ChangeShop(SkinShopCell.WhoseSkin.Player);
    }

    public void ChangeToChickenShop()
    {
        ChangeShop(SkinShopCell.WhoseSkin.Chicken);
    }

    private void ChangeShop(SkinShopCell.WhoseSkin whoseSkin)
    {
        whoseSkinShop = whoseSkin;
        UpdateShop();
    }

    private void UpdateShop()
    {
        foreach (var cell in cells)
        {
            cell.gameObject.SetActive(false);
        }
        
        for(int i = 0; i < GetSkinsCount[(int)whoseSkinShop]();i++)
        {
            
            SkinShopCell cell = cells[i];
            cell.SkinShop = this;
           
            cell.ShowSkin(i,whoseSkinShop);
            cells[i].gameObject.SetActive(true);
        }
    }

    public void EnablePanel()
    {
        wrapper.gameObject.SetActive(true);
        startGameUI.gameObject.SetActive(false);
    }

    public void DisablePanel()
    {
        wrapper.gameObject.SetActive(false);
        startGameUI.gameObject.SetActive(true);
    }
}
