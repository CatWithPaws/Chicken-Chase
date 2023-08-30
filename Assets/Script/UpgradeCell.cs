using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCell : MonoBehaviour
{
    public UpgradeShop upgradeShop;

    [SerializeField] private TextMeshProUGUI BuffNameText;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private Image BuffIcon;
    [SerializeField] private TextMeshProUGUI Price;

    [SerializeField] private GameObject BuyObject;
    [SerializeField] private GameObject MaxLVLObject;

    [SerializeField] private GameObject[] UpgradeLevelIndicators;

    private BuffType BuffType;
    private int buffLevel = 1;
    private int price = 0;

    private static Dictionary<BuffType, string> buffNames = new Dictionary<BuffType, string>() {
        { BuffType.TripleJump,"Triple Jump" },
        { BuffType.Invincibility, "Invincibility" },
        { BuffType.Bridge, "Bridge" }
    };

    public void ShowCell(BuffType type,Sprite icon)
    {
        buffLevel = GameData.GetBuffLevel(type);
        BuffType = type;

        int maxBuffLevel = 10;

        BuffNameText.text = buffNames[type];
        if(buffLevel < maxBuffLevel)
        {
            LevelText.text = "Lvl " + buffLevel.ToString() + "/ 10";
            price = (400 * buffLevel);
            Price.text = price.ToString();
            BuyObject.SetActive(true);
            MaxLVLObject.SetActive(false);
        }
        else
        {
            MaxLVLObject.SetActive(true);
            BuyObject.SetActive(false);
        }
        
        foreach(var indicator in  UpgradeLevelIndicators)
        {
            indicator.SetActive(false);
        }
        for(int i = 0; i < buffLevel; i++)
        {
            UpgradeLevelIndicators[i].SetActive(true);
        }
        BuffIcon.sprite = icon;
        gameObject.SetActive(true);
    }


    public void BuyUpgrade()
    {
        if (GameData.SpendMoney(price))
        {
            GameData.SetBuffLevel(BuffType, buffLevel + 1);
            upgradeShop.OnUpgradeBuy();
        }
    }
}
