using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public PlayerController Player;

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

	}
}
