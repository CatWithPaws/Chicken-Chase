using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinsText;

	private void Awake()
	{
		coinsText.text = GameData.CoinsCount.ToString();
		GameData.OnMoneyUpdate += UpdateCoins;
	}

	public void UpdateCoins()
    {
        coinsText.text = GameData.CoinsCount.ToString();
    }
}
