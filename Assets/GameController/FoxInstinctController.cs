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
    //Animations
    public string sniffingAnimationTrigger;

    //Audio and animation
    private AudioSource mAudioSource;
    private Animator mAnimator;
    private PlayerController mPlayerController;
    private ItemController mItemController;

    //Input
    public string actionButton;

    //Parameters
    public float actionTime;
    public float hintLifetime;
    public float spawnInterval;


    //Objective prefabs
    public GameObject bonusHintPrefab;
    public GameObject toolHintPrefab;
    public GameObject objectiveHintPrefab;
    public GameObject itemUsageHintPrefab;

    //Corutines 
    private IEnumerator mActionCoroutine;

    //Sound manager
    private PlayerSoundManager mSoundManager;
    [Range(0, 1)]
    public float sniffSoundVolume;

    void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
        mAnimator = GetComponentInChildren<Animator>();
        mPlayerController = GetComponent<PlayerController>();
        mItemController = GetComponent<ItemController>();
        mSoundManager = GetComponent<PlayerSoundManager>();

        Debug.Assert(mAudioSource != null && mAnimator != null
            && mPlayerController != null && mItemController != null
            && mSoundManager != null);

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

        mSoundManager.PlayPlayerSound(PlayerSoundManager.PlayerSoundType.SNIFF, sniffSoundVolume);
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

}
