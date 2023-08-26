using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI BuffNameText;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private Image BuffIcon;
    [SerializeField] private TextMeshProUGUI Price;

    [SerializeField] private GameObject BuyObject;
    [SerializeField] private GameObject MaxLVLObject;
    
    private BuffType BuffType;
    private int buffLevel = 1;

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
            Price.text = (200 * (2 * buffLevel)).ToString();
            BuyObject.SetActive(true);
            MaxLVLObject.SetActive(false);
        }
        else
        {
            MaxLVLObject.SetActive(true);
            BuyObject.SetActive(false);
        }
        
        BuffIcon.sprite = icon;
        
    }


    public void BuyUpgrade()
    {
        GameData.SetBuffLevel(BuffType, buffLevel + 1);
    }
}
