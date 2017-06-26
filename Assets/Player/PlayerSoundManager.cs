using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public GameObject sandZoneTrigger;

    //Audio
    public AudioClip grassWalkSound;
    public AudioClip grassRunSound;

    public AudioClip sandWalkSound;
    public AudioClip sandRunSound;

    public AudioClip jumpSound;

    public AudioClip[] sniffSounds;

    public bool randomPitch;
    public Vector2 pitchMinMax;

    private AudioSource mAudioSource;

    private bool mPlayerInSandZone;


    public enum PlayerSoundType { WALK, RUN, JUMP, SNIFF };

    // Use this for initialization
    void Start()
    {
        mPlayerInSandZone = false;
        mAudioSource = GetComponent<AudioSource>();
        Debug.Assert(mAudioSource != null);
    }

    public void PlayPlayerSound(PlayerSoundType type, float volume)
    {
        switch (type)
        {
            case PlayerSoundType.WALK:
                PlaySound(mPlayerInSandZone ? sandWalkSound : grassWalkSound, volume);
                break;

            case PlayerSoundType.RUN:
                PlaySound(mPlayerInSandZone ? sandRunSound : grassRunSound, volume);
                break;

            case PlayerSoundType.JUMP:
                PlaySound(jumpSound, volume);
                break;

            case PlayerSoundType.SNIFF:
                PlaySound(sniffSounds[Random.Range(0, sniffSounds.Length)], volume);
                break;

            default:
                break;
        }
    }

    private void PlaySound(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            //Sound clip
            mAudioSource.clip = clip;
            mAudioSource.volume = volume;

            mAudioSource.pitch = randomPitch ? Random.Range(pitchMinMax.x, pitchMinMax.y) : 1.0f;

            mAudioSource.Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == sandZoneTrigger)
        {
            mPlayerInSandZone = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == sandZoneTrigger)
        {
            mPlayerInSandZone = false;
        }
    }
}
