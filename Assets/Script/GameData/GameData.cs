using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Delegates;
using System;

public class GameData
{
	public static PlayerController Player;

	[SerializeField] private static PlayerSkins playerSkins;
	[SerializeField] private static GiantChickenSkins chickenSkins;


	private static int coinsCount;
	public static int CoinsCount
	{
		get
		{
			return coinsCount;
		}
		set
		{
			coinsCount = value;
			OnMoneyUpdate?.Invoke();
		}
	}

	public static VoidFunc OnSkinUpdate;
	public static VoidFunc OnMoneyUpdate;

	public static void Load()
	{
		Save.LoadSave();

		playerSkins = Resources.Load<PlayerSkins>("Skins/PlayerSkins");
		chickenSkins = Resources.Load<GiantChickenSkins>("Skins/GiantChickenSkins");

		if (playerSkins == null)
		{
			Debug.LogError("Player skins file not found");
		}

		if (chickenSkins == null)
		{
			Debug.LogError("Chicken skins file not found");
		}

		LoadCoins();
	}

	private static void LoadCoins()
	{
		CoinsCount = Save.saveFile.PlayerData.Money;
	}

	public static void SaveCoins()
	{
		Save.saveFile.PlayerData.Money = CoinsCount;
		SaveGame();
	}

	public static void SetChickenSkin(int id)
	{

		Save.saveFile.ChickenSkinID = id;
		SaveGame();
		OnSkinUpdate.Invoke();
	}

	public static void SetPlayerSkin(int id)
	{
		Save.saveFile.PlayerSkinID = id;
		SaveGame();
		OnSkinUpdate.Invoke();
	}

	public static Skin GetPlayerSkinByID(int ID)
	{
		return playerSkins.skins[ID];
	}

	public static Skin GetChickenSkinByID(int ID)
	{
		return chickenSkins.skins[ID];
	}

	public static Skin GetCurrentChickenSkin()
	{
		return chickenSkins.skins[Save.saveFile.ChickenSkinID];
	}

	public static Skin GetCurrentPlayerSkin()
	{
		return playerSkins.skins[Save.saveFile.PlayerSkinID];
	}

	public static void SaveGame()
	{
		Save.SaveGame();
	}

	public static void UnlockPlayerSkin(int id)
	{
		Save.saveFile.PlayerData.PlayerSkins.Add(id);
	}

	public static void UnlockChickenSkin(int id)
	{
		Save.saveFile.PlayerData.ChickenSkins.Add(id);
	}

	public static int playerSkinsCount()
	{
		return playerSkins.skins.Count;
	}
	public static int chickenSkinsCount()
	{
		return chickenSkins.skins.Count;
	}

	public static bool IsPlayerSkinBought(int id) => Save.saveFile.PlayerData.PlayerSkins.Contains(id);
	public static bool IsChickenSkinBought(int id) => Save.saveFile.PlayerData.ChickenSkins.Contains(id);

	public static long GetBestDistance => Save.saveFile.BestDistance;

	public static void SetBestDistance(long distance)
	{
		Save.saveFile.BestDistance = distance;
	}
}
