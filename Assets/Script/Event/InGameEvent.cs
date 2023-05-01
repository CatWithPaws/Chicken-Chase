using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameEvent : MonoBehaviour
{
    public static InGameEvent instance;

	public Action OnGameStarted;
    public Action OnPlayerLoose;

	private void Awake()
	{
		instance = this;
	}
}
