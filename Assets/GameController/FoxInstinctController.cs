using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



//Hint types global enum
public enum HintType { OBJECTIVE, FOOD, TOOL, TOOL_USAGE };

//Hint interface
public interface IObjectHint
{
    string GetHintName();

    void SetHintObject(GameObject hintObject);
    bool HasActiveHintObject();

    bool DisplayHint();
    HintType GetHintType();
}



public class FoxInstinctController : MonoBehaviour
{
    //Sound effects
    public AudioClip[] sounds;
    public bool randomPitch;
    public Vector2 pitchMinMax;

    //Animations
    public string sniffingAnimationTrigger;

    //Audio and animation
    private AudioSource mAudioSource;
    private Animator mAnimator;
    private PlayerController mPlayerController;
    private ItemController mItemController;

    //Input
    public string actionButton;
    //public string objectsOfInterestTag;
    //public float pointerDistance;

    //public float reloadSpeedPct;

    //Parameters
    public float actionTime;
    public float hintLifetime;
    public float spawnInterval;


    //Objective prefabs
    public GameObject bonusHintPrefab;
    public GameObject toolHintPrefab;
    public GameObject objectiveHintPrefab;
    public GameObject itemUsageHintPrefab;



    //public GameObject bonusItempointerPrefab;
    //public GameObject keyItempointerPrefab;
    //public float slowmoTimeScale;

    //private GameObject[] mKeyItems;
    //private GameObject[] mBonusItems;
    //private GameObject[] mAllItems;

    //private GameObject[] lastGeneratedPointers;

    //private float mCurrentTime;

    //Corutines 
    private IEnumerator mActionCoroutine;

    void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
        mAnimator = GetComponentInChildren<Animator>();
        mPlayerController = GetComponent<PlayerController>();
        mItemController = GetComponent<ItemController>();

        Debug.Assert(mAudioSource != null && sounds != null && sounds.Length != 0 && mAnimator != null
            && mPlayerController != null && mItemController != null);

        mActionCoroutine = null;

        //mCurrentTime = maxActionTime;
        //RefreshUi();
    }

    //TODO : time constrain, slowmo, bonus pickup pointer

    void Update()
    {
        if (Input.GetButtonDown(actionButton) && !GameController.GamePaused)
        {
            StartAction();
        }
    }

    private void StartAction()
    {
        if (mActionCoroutine == null && mPlayerController.PlayerGrounded)
        {
            mActionCoroutine = FoxInstinctAction();
            StartCoroutine(mActionCoroutine);
        }
    }

    private IEnumerator FoxInstinctAction()
    {
        //Play animations
        PlayAnimationAndSound();

        //Stop player movements
        mPlayerController.AbleToMove = false;


        //Action
        StartCoroutine(SpawnAllHints());


        //Wait for specified time
        yield return new WaitForSeconds(actionTime);

        //Enable player movements
        mPlayerController.AbleToMove = true;

        mActionCoroutine = null;

        yield return null;
    }

    private IEnumerator SpawnAllHints()
    {
        if (KeyItemSpawner.BonusItems != null)
        {
            StartCoroutine(CreateItemHints(KeyItemSpawner.BonusItems));
        }

        yield return new WaitForSeconds(spawnInterval);

        if (KeyItemSpawner.KeyItems != null)
        {
            StartCoroutine(CreateItemHints(KeyItemSpawner.KeyItems));
        }

        yield return new WaitForSeconds(spawnInterval);

        if (KeyItemSpawner.ToolItems != null)
        {
            //Remove holded item from array
            GameObject[] toolItems = KeyItemSpawner.ToolItems;
            toolItems = toolItems.Where(x => x != ItemController.HoldedItem).ToArray();

            StartCoroutine(CreateItemHints(toolItems));
        }

        yield return new WaitForSeconds(spawnInterval);

        if (KeyItemSpawner.ToolUsageItems != null)
        {
            StartCoroutine(CreateItemHints(KeyItemSpawner.ToolUsageItems));
        }

        yield return null;
    }

    private void PlayAnimationAndSound()
    {
        //Animation
        mAnimator.SetTrigger(sniffingAnimationTrigger);

        //Sound clip
        mAudioSource.clip = sounds[Random.Range(0, sounds.Length)];
        if (randomPitch)
        {
            mAudioSource.pitch = Random.Range(pitchMinMax.x, pitchMinMax.y);
        }
        mAudioSource.Play();
    }

    private IEnumerator CreateItemHints(GameObject[] objectsWithHints)
    {
        //Delay between creating hints

        if (objectsWithHints != null)
        {
            foreach (GameObject objectWithHint in objectsWithHints)
            {
                if (objectWithHint != null)
                {
                    SpawnItemHint(objectWithHint);
                }

                //Spawn interval
                yield return new WaitForSeconds(spawnInterval);
            }

        }

        yield return null;
    }

    private void SpawnItemHint(GameObject pickup)
    {
        IObjectHint hintInfo = pickup.GetComponent<IObjectHint>();

        if (hintInfo != null)
        {
            //Do nothing if the item has active hint object and is willing to display hint info
            if (!hintInfo.HasActiveHintObject() && hintInfo.DisplayHint())
            {

                //Default prefab
                GameObject prefab = bonusHintPrefab;

                //Switch prefab
                switch (hintInfo.GetHintType())
                {
                    case HintType.FOOD:
                        prefab = bonusHintPrefab;
                        break;
                    case HintType.TOOL:
                        prefab = toolHintPrefab;
                        break;
                    case HintType.OBJECTIVE:
                        prefab = objectiveHintPrefab;
                        break;
                    case HintType.TOOL_USAGE:
                        prefab = itemUsageHintPrefab;
                        break;
                    default:
                        break;
                }

                //Create prefab
                GameObject gameObject = Instantiate(prefab, pickup.transform.position, Quaternion.identity) as GameObject;

                if (gameObject != null)
                {
                    ItemHint hintScript = gameObject.GetComponent<ItemHint>();
                    if (hintScript != null)
                    {
                        hintInfo.SetHintObject(gameObject);
                        hintScript.CreateHint(hintInfo.GetHintName(), mPlayerController.PlayerCamera.gameObject.transform, pickup.gameObject, hintLifetime);
                    }
                }

            }
        }
    }

    //private void StartAction()
    //{
    //    RefreshUi();


    //    PlayRandomClip();
    //    mAnimator.SetTrigger(sniffingAnimationTrigger);

    //    if (mCurrentTime > 0.0f)
    //    {
    //        mCurrentTime -= Time.deltaTime;
    //        GeneratePointers();
    //    }
    //    else
    //    {
    //        mCurrentTime = 0.0f;
    //        DestroyPointers();
    //    }


    //}

    //private void StopAction()
    //{
    //    RefreshUi();
    //    if (mCurrentTime < maxActionTime)
    //    {
    //        mCurrentTime += Time.deltaTime * reloadSpeedPct;
    //    }
    //    else
    //    {
    //        mCurrentTime = maxActionTime;
    //    }
    //    DestroyPointers();
    //}


    //private void DestroyPointers()
    //{
    //    if (lastGeneratedPointers != null)
    //    {
    //        for (int i = 0; i < mAllItems.Length; i++)
    //        {
    //            Destroy(lastGeneratedPointers[i]);
    //        }
    //        lastGeneratedPointers = null;
    //    }
    //}

    //private void CreatePrefabs(GameObject prefab, GameObject[] positions, int indexStart)
    //{
    //    for (int i = 0; i < positions.Length; i++)
    //    {
    //        if (positions[i] != null)
    //        {
    //            GameObject gameObject = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;
    //            gameObject.GetComponent<ItemPointer>().Target = positions[i].transform;
    //            gameObject.transform.SetParent(this.transform);
    //            lastGeneratedPointers[i + indexStart] = gameObject;
    //        }
    //        else
    //        {
    //            continue;
    //        }
    //    }
    //}

    //private void RefreshUi()
    //{
    //    if (GameUI.Instance != null)
    //    {
    //        ProgressBar progressBar = GameUI.Instance.GetProgressBar(GameUI.ProgressBars.FOXPOWER);
    //        if (progressBar != null)
    //        {
    //            float pct = mCurrentTime / maxActionTime;
    //            progressBar.ProgressBarPct = pct;
    //            progressBar.ValueText = ((int)(pct * 100.0f)) + "%";
    //        }
    //    }
    //}

    //private void GeneratePointers()
    //{
    //    if (mKeyItems == null && mBonusItems == null)
    //    {
    //        mKeyItems = KeyItemSpawner.KeyItems;
    //        mBonusItems = KeyItemSpawner.BonusItems;

    //        if (mKeyItems == null || mBonusItems == null)
    //        {
    //            return;
    //        }

    //        mAllItems = mKeyItems.Concat(mBonusItems).ToArray();
    //    }
    //    else if (lastGeneratedPointers == null && mAllItems != null)
    //    {
    //        lastGeneratedPointers = new GameObject[mAllItems.Length];
    //        CreatePrefabs(bonusItempointerPrefab, mBonusItems, 0);
    //        CreatePrefabs(keyItempointerPrefab, mKeyItems, mBonusItems.Length);
    //    }
    //}
}
