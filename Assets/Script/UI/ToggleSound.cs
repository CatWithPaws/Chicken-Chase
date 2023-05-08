using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSound : MonoBehaviour
{
	[SerializeField] private Image btnImage;

	[SerializeField] private Sprite enabledBtn;
	[SerializeField] private Sprite disabledBtn;

	public void ToggleBtn()
	{
		if (AudioManager.Instance != null)
		{
			bool toggle = AudioManager.Instance.SoundToogle;
			AudioManager.Instance.SoundToogle = !toggle;
			btnImage.sprite = toggle ? enabledBtn : disabledBtn;
		}
	}
}
