using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSound : MonoBehaviour
{
	[SerializeField] private Image btnImage;

	[SerializeField] private Sprite enabledBtn;
	[SerializeField] private Sprite disabledBtn;

    private void OnEnable()
    {
		UpdateSprite();
    }

    public void ToggleBtn()
	{
		if (AudioManager.Instance != null)
		{
			bool newMuted = !AudioManager.Instance.SoundMuted;
			AudioManager.Instance.SoundMuted = newMuted;
            UpdateSprite();
        }
	}

	private void UpdateSprite()
	{
        btnImage.sprite = AudioManager.Instance.SoundMuted ? disabledBtn : enabledBtn;
    }
}
