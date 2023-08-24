using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMusic : MonoBehaviour
{
	[SerializeField] private Image btnImage;

	[SerializeField] private Sprite enabledBtn;
	[SerializeField] private Sprite disabledBtn;

    private void OnEnable()
    {
		UpdateBtnSprite();
    }

    public void ToggleBtn()
	{
		if (AudioManager.Instance != null)
		{
			bool newMuted = !AudioManager.Instance.MusicMuted;
			AudioManager.Instance.MusicMuted = newMuted;
            UpdateBtnSprite();
        }
	}

	private void UpdateBtnSprite()
	{
        btnImage.sprite = AudioManager.Instance.MusicMuted ? disabledBtn : enabledBtn;
    }
}
