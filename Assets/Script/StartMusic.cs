using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour
{
	[SerializeField] private AudioClip music;

	private void Start()
	{
		if(AudioManager.Instance == null)
		{
			Debug.LogWarning("Warning : AudioManager is not loaded");
		}
		AudioManager.Instance?.PlayMusic(music);
	}

}
