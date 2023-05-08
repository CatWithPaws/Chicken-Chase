using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public PlayerController Player;


	public int CoinsCount;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if(Instance == this)
		{
			Destroy(gameObject);
		}

		LoadCoins();
	}

	private void LoadCoins()
	{
		CoinsCount = PlayerPrefs.GetInt("Coins",0);
	}

	public void SaveCoins()
	{
		PlayerPrefs.SetInt("Coins", CoinsCount);
	}
}
