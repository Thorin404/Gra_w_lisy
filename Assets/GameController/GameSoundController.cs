using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundController : MonoBehaviour {

    public AudioClip stageStart;
    public AudioClip stageWon;
    public AudioClip stageLost;

    public bool randomPitch;
    public Vector2 pitchMinMax;
    private AudioSource mAudioSource;

    public enum GameSounds { START, WON, LOST };

    void Start () {
        mAudioSource = GetComponent<AudioSource>();
        Debug.Assert(mAudioSource != null);
    }

    public void PlayGameSound(GameSounds sound)
    {
        switch (sound)
        {
            case GameSounds.START:
                PlaySound(stageStart);
                break;
            case GameSounds.WON:
                PlaySound(stageWon);
                break;
            case GameSounds.LOST:
                PlaySound(stageLost);
                break;

            default:
                break;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            //Sound clip
            mAudioSource.clip = clip;

            mAudioSource.pitch = randomPitch ? Random.Range(pitchMinMax.x, pitchMinMax.y) : 1.0f;

            mAudioSource.Play();
        }
    }


    // Update is called once per frame
    void Update () {
		
	}
}
