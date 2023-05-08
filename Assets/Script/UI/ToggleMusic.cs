using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMusic : MonoBehaviour
{
	[SerializeField] private Image btnImage;

	[SerializeField] private Sprite enabledBtn;
	[SerializeField] private Sprite disabledBtn;

	public void ToggleBtn()
	{
		if (AudioManager.Instance != null)
		{
			bool toggle = AudioManager.Instance.MusicToggle;
			AudioManager.Instance.MusicToggle = !toggle;
			btnImage.sprite = toggle ? enabledBtn : disabledBtn;
		}
	}
}
