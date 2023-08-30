using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWindow : MonoBehaviour
{
    [SerializeField] private GameObject startGameUI;
    [SerializeField] private GameObject wrapper;

    [SerializeField] private SkinShop skinShop;
    [SerializeField] private UpgradeShop upgradeShop;

    public void OpenUpgradeShop()
    {
        wrapper.SetActive(true);
        skinShop.gameObject.SetActive(false);
        upgradeShop.gameObject.SetActive(true);
        startGameUI.SetActive(false);
    }

    public void OpenChickenSkinShop()
    {
        wrapper.SetActive(true);
        skinShop.ChangeToChickenShop();
        skinShop.gameObject.SetActive(true);
        startGameUI.SetActive(false);
        upgradeShop.gameObject.SetActive(false);
        
    }

    public void OpenPlayerSkinShop()
    {
        wrapper.SetActive(true);
        skinShop.gameObject.SetActive(true);
        skinShop.ChangeToPlayerShop();
        startGameUI.SetActive(false);
        upgradeShop.gameObject.SetActive(false);
    }

    public void CloseShop()
    {
        wrapper.SetActive(false);
        startGameUI.gameObject.SetActive(true);
    }

}
