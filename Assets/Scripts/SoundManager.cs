using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioSource effectSource, musicSource;
    [SerializeField] private AudioClip[] button_Effects, destroy_Effects, error_Effects, validation_Effects, game_Start_Effects;
    [SerializeField] private AudioClip menu_Music, game_Music;
    public enum Effect {
        BUTTON_CLICK,
        DESTROY,
        ERROR,
        VALIDATION,
        GAME_START
    }

    public enum Music {
        MENU,
        GAME
    }

    public static SoundManager instance;


    private void Awake() {
        if (SoundManager.instance == null) {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else {
            Destroy(this);
        }
    }
    
    public void PlayEffect(Effect effect) {
        switch (effect) {
            case Effect.BUTTON_CLICK:
                effectSource.PlayOneShot(button_Effects[Random.Range((int)0, (int)button_Effects.Length)]);
                break;
            case Effect.DESTROY:
                effectSource.PlayOneShot(destroy_Effects[Random.Range((int)0, (int)destroy_Effects.Length)]);
                break;
            case Effect.ERROR:
                effectSource.PlayOneShot(error_Effects[Random.Range((int)0, (int)error_Effects.Length)]);
                break;
            case Effect.VALIDATION:
                effectSource.PlayOneShot(validation_Effects[Random.Range((int)0, (int)validation_Effects.Length)]);
                break;
            case Effect.GAME_START:
                effectSource.PlayOneShot(game_Start_Effects[Random.Range((int)0, (int)game_Start_Effects.Length)]);
                break;
        }
    }

    public void PlayMusic(Music music) {
        switch (music) {
            case Music.MENU:
                if (musicSource.clip != menu_Music) {
                    musicSource.Stop();
                    musicSource.clip = menu_Music;
                    musicSource.Play();
                }
                break;
            case Music.GAME:
                if (musicSource.clip != game_Music) {
                    musicSource.Stop();
                    musicSource.clip = game_Music;
                    musicSource.Play();
                }
                break;
        }
    }
}
