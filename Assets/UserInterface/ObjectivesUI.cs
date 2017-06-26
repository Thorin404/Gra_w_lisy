using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesUI : MonoBehaviour
{
    public GameObject objectivePrefab;
    public GameObject objectiveHolder;
    public Sprite activeSprite;
    public Sprite unactiveSprite;

    public float spawnInterval;

    private bool mObjectivesCreated = false;
    private int mTotalObjectives = -1;
    private int mCounter = -1;

    private GameObject[] mObjectives;
    private Image[] mImages;

    public AudioClip spawnSound;
    public AudioClip activateSound;
    private AudioSource mAudioSource;

    void Awake()
    {
        mAudioSource = GetComponent<AudioSource>();
        Debug.Assert(spawnSound != null && activateSound != null && mAudioSource != null);
    }

    public void CreateObjectives(int number)
    {
        if (!mObjectivesCreated)
        {
            mTotalObjectives = number;
            mCounter = 0;

            //Create arrays
            StartCoroutine(CreateUiElements(mTotalObjectives));         
        }
    }

    private void PlayClip(AudioClip audioClip)
    {
        mAudioSource.clip = audioClip;
        mAudioSource.Play();
    }

    private IEnumerator CreateUiElements(int number)
    {
        mObjectives = new GameObject[number];
        mImages = new Image[number];

        for (int i = 0; i < number; i++)
        {
            CreateObjective(out mObjectives[i], out mImages[i]);
            yield return new WaitForSeconds(spawnInterval);
        }

        mObjectivesCreated = true;

        yield return null;
    }

    private void CreateObjective(out GameObject objectiveArrayElem, out Image imageArrayElement)
    {
        PlayClip(spawnSound);
        objectiveArrayElem = null;
        imageArrayElement = null;
        objectiveArrayElem = Instantiate(objectivePrefab, objectiveHolder.transform);
        imageArrayElement = objectiveArrayElem.GetComponentInChildren<Image>();
    }

    public void IncrementObjetiveCounter()
    {
        //Debug.Log("Add objective poit");
        //check if is in range
        if (mObjectivesCreated)
        {
            if (mCounter >= 0 && mCounter < mImages.Length)
            {
                PlayClip(activateSound);
                //TODO : play 2d sound ?
                mImages[mCounter].sprite = activeSprite;
                mImages[mCounter].color = Color.white;
                mCounter++;
            }
        }
    }

  

}
