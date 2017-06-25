using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHint : MonoBehaviour
{
    public bool randomPitch;
    public Vector2 pitchMinMax;
    public AudioClip appearSound;
    public AudioClip disappearSound;


    private TextMesh mTextMesh;
    private AudioSource mAudioSource;

    private GameObject mPointedObject;
    private Transform mLookTarget;
    private float mLifeTime;

    //Countdown corutine
    private IEnumerator mCountdownCorutine;

    public void CreateHint(string textToDisplay, Transform target, GameObject pointedObj, float lifeTime)
    {
        mLookTarget = target;
        mLifeTime = lifeTime;
        mPointedObject = pointedObj;

        mTextMesh.text = textToDisplay;

        PlaySound(appearSound);

        //Start countdonw corutine
        mCountdownCorutine = CountdownLifeTime();

        StartCoroutine(mCountdownCorutine);
    }

    private void PlaySound(AudioClip aclip)
    {
        mAudioSource.clip = aclip;
        if (randomPitch)
        {
            mAudioSource.pitch = Random.Range(pitchMinMax.x, pitchMinMax.y);
        }
        mAudioSource.Play();
    }

    // Use this for initialization
    void Awake()
    {
        mTextMesh = GetComponentInChildren<TextMesh>();
        mAudioSource = GetComponent<AudioSource>();
        Debug.Assert(mTextMesh != null && mAudioSource != null);
        Debug.Assert(appearSound != null && disappearSound != null);
        mCountdownCorutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (mLookTarget != null)
        {
            Vector3 targetPosition = new Vector3(mLookTarget.position.x, transform.position.y, mLookTarget.position.z);
            transform.LookAt(targetPosition);
        }

        bool playerGrabbedItem = false;

        playerGrabbedItem = ItemController.HoldedItem == null ? false : (ItemController.HoldedItem.gameObject == mPointedObject);

        if ((mPointedObject == null || playerGrabbedItem) && mCountdownCorutine != null)
        {
            StopCountdown();
        }
    }

    private void StopCountdown()
    {
        StopCoroutine(mCountdownCorutine);
        mCountdownCorutine = null;
        StartCoroutine(DestroyHint());
    }

    private IEnumerator CountdownLifeTime()
    {
        //Wait
        yield return new WaitForSeconds(mLifeTime);

        mCountdownCorutine = null;

        StartCoroutine(DestroyHint());
    }

    private IEnumerator DestroyHint()
    {
        //Play destroy sound
        PlaySound(disappearSound);

        //Wait for sound to play
        yield return new WaitForSeconds(mAudioSource.clip.length);

        //Destroy object
        Destroy(this.gameObject);
    }
}
