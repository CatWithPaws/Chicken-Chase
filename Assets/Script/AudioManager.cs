using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public const string MUSIC_TOGGLE_SAVE_INDEX = "MusicToggle";
	public const string SOUND_TOGGLE_SAVE_INDEX = "SoundToggle";



	public AudioSource Music;

    public AudioSource Sounds;
	public AudioSource SFX;

    public bool SoundMuted
    {
        get
        {
            return soundToggle;
        }
        set
        {

            SFX.mute= value;
            Sounds.mute = value;
            soundToggle = value;
            PlayerPrefs.SetInt(SOUND_TOGGLE_SAVE_INDEX, (value) ? 1 : 0);
        }
    }

    private bool soundToggle = true;

    public bool MusicMuted
    {
        get
        {
            return musicToggle;
        }
        set
        {
            Music.mute = value;
            musicToggle = value;
            PlayerPrefs.SetInt(MUSIC_TOGGLE_SAVE_INDEX, (value) ? 1 : 0);
        }
    }


    private bool musicToggle = true;
	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
        MusicMuted = PlayerPrefs.GetInt(MUSIC_TOGGLE_SAVE_INDEX) == 1 ?  true : false;

		SoundMuted = PlayerPrefs.GetInt(SOUND_TOGGLE_SAVE_INDEX) == 1 ? true : false;
		Instance = this;
	}

	public void PlaySound(AudioClip clip)
    {
		Sounds.PlayOneShot(clip);
	}

    public void PlaySFX(AudioClip clip)
    {
		SFX.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip music)
    {
        Music.clip = music;
        Music.Play();
    }

    public void StopMusic()
    {
        Music.Stop();
    }

}
