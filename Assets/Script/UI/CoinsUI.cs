using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinsText;

	private void Start()
	{
		if (GameData.Instance == null)
		{
			coinsText.text = 0.ToString();
		}
		else
		{
			coinsText.text = GameData.Instance.CoinsCount.ToString();
		}
	}

	public void UpdateCoins()
    {
        coinsText.text = GameData.Instance?.CoinsCount.ToString();
    }
}
