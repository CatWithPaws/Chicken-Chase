using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSound : MonoBehaviour
{
    [SerializeField] AudioClip clickSound;

    public void PlayClick()
    {
        AudioManager.Instance?.PlaySFX(clickSound);
    }
}
