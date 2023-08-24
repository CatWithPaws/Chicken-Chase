using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Delegates;

public class SkinShopCell : MonoBehaviour
{
    enum CellStatus
    {
        Locked,Unlocked,Equiped
    }

    public enum WhoseSkin
    {
        Player,
        Chicken
    }

    private delegate void UnlockSkinDelegate(int id);
    private delegate Skin GetOwnerSkinDelegate(int id);
    private delegate bool IsSkinBoughtDelegate(int id);
    private delegate Skin GetCurrentSkinDelegate();
    private delegate void EquipSkinDelegate(int id);

    public SkinShop SkinShop;

    [SerializeField] private Image skinPreview;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private int skinID;

    [SerializeField] private GameObject buyWrapper;
    [SerializeField] private GameObject equippedWrapper;

    [SerializeField] private TextMeshProUGUI equipedText;

    private CellStatus status;

    private WhoseSkin whoseSkin;

    private int price;

    private UnlockSkinDelegate[] UnlockSkin = new UnlockSkinDelegate[]
    {
        GameData.UnlockPlayerSkin,
        GameData.UnlockChickenSkin
    };

    private GetOwnerSkinDelegate[] GetSkin = new GetOwnerSkinDelegate[]
    {
        GameData.GetPlayerSkinByID,
        GameData.GetChickenSkinByID
    };

    private IsSkinBoughtDelegate[] IsSkinBought = new IsSkinBoughtDelegate[]
    {
        GameData.IsPlayerSkinBought,
        GameData.IsChickenSkinBought

    };

    private GetCurrentSkinDelegate[] GetCurrentSkin = new GetCurrentSkinDelegate[]
    {
        GameData.GetCurrentPlayerSkin,
        GameData.GetCurrentChickenSkin
    };

    private EquipSkinDelegate[] EquipSkin = new EquipSkinDelegate[]
    {
        GameData.SetPlayerSkin,
        GameData.SetChickenSkin
    };


    public void ShowSkin(int ID,WhoseSkin skinOwner)
    {
        whoseSkin = skinOwner;

        skinID = ID;
        Skin pickedSkin = GetSkin[(int)whoseSkin](ID);
        skinPreview.sprite = pickedSkin.SkinPreview;

        nameText.text = pickedSkin.Name;

        if (IsSkinBought[(int)whoseSkin](ID)) 
        {
            
            if (GetCurrentSkin[(int)whoseSkin]().ID == ID)
            {
                equipedText.text = "Equipped";
                status = CellStatus.Equiped;
                SkinShop.equipedCell = this;
            }
            else
            {
                equipedText.text = "Equip";
                status = CellStatus.Unlocked;
            }
            buyWrapper.SetActive(false);
            equippedWrapper.SetActive(true);
        }
        else
        {
            price = pickedSkin.price;

            priceText.text = pickedSkin.price.ToString();
            status = CellStatus.Locked;
            buyWrapper.SetActive(true);
            equippedWrapper.SetActive(false);
        }
        
    }

    private void BuySkin()
    {
        if (GameData.CoinsCount >= price)
        {
            GameData.CoinsCount -= price;
            UnlockSkin[(int)whoseSkin](skinID);
        }
    }


    public void OnButtonClick()
    {
        switch (status)
        {
            case CellStatus.Locked:
                BuySkin();
                break;
            case CellStatus.Equiped:
                break;
            case CellStatus.Unlocked:
                EquipSkin[(int)whoseSkin](skinID);
                break;
        }

        SkinShop.OnShopUpdate.Invoke();
    }


}
