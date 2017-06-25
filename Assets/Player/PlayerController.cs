using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public string horizontalAxis;
    public string vertivcalAxis;
    public string runningButton;
    public string jumpButton;

    public Camera playerCamera;

    public string animatorParamName;
    public string jumpAnimationTriggerName;

    public float walkSpeed = 2;
    public float runSpeed = 6;
    public float gravity = -12;
    public float jumpHeight = 1;
    [Range(0, 1)]
    public float airControlPercent;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;
    private bool moving;

    private Animator mAnimatior;
    private Transform mCameraTarget;
    private CharacterController mController;

    //Behaviour scripts
    private PickUpController mPickupController;
    private FoxInstinctController mFoxInstinctController;
    private ItemController mItemController;

    public bool AbleToMove
    {
        set
        {
            moving = value;
        }
        get
        {
            return moving;
        }
    }

    public bool PlayerGrounded
    {
        get
        {
            return mController.isGrounded;
        }
    }

    public Camera PlayerCamera
    {
        get
        {
            return playerCamera;
        }
    }

    public float SetAnimatorState
    {
        set
        {
            if (mAnimatior != null)
            {
                mAnimatior.SetFloat(animatorParamName, value);
            }
        }
    }

    void OnEnable()
    {
        //Debug.Log("Enable PC");

        if (mPickupController == null
            || mFoxInstinctController == null
            || mItemController == null)
        {
            InitControllers();
        }
        else
        {
            EnableControllers(enabled);
        }
    }

    void OnDisable()
    {
        //Debug.Log("Disable PC");
        EnableControllers(enabled);
    }

    void Start()
    {
        Debug.Log("Player start");
        mAnimatior = GetComponentInChildren<Animator>();
        //cameraT = Camera.main.transform;
        mCameraTarget = playerCamera.transform;

        mAnimatior.SetFloat(animatorParamName, 0.0f);
        moving = true;
    }

    private void InitControllers()
    {
        //Controller references
        mController = GetComponent<CharacterController>();
        mPickupController = GetComponent<PickUpController>();
        mFoxInstinctController = GetComponent<FoxInstinctController>();
        mItemController = GetComponent<ItemController>();
    }

    private void EnableControllers(bool e)
    {
        mPickupController.enabled = e;
        mFoxInstinctController.enabled = e;
        mItemController.enabled = e;
    }

    void Update()
    {
        // input
        Vector2 input = new Vector2(Input.GetAxisRaw(horizontalAxis), Input.GetAxisRaw(vertivcalAxis));
        Vector2 inputDir = input.normalized;

        bool running = Input.GetButton(runningButton);

        if (moving)
        {
            Move(inputDir, running);
        }

        if (Input.GetButtonDown(jumpButton))
        {
            Jump();
        }
        // animator
        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
        mAnimatior.SetFloat(animatorParamName, animationSpeedPercent, speedSmoothTime, Time.deltaTime);

    }

    void Move(Vector2 inputDir, bool running)
    {
        //TODO : fix player time scale problem

        if (inputDir != Vector2.zero)
        {
            //Camera rotation
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + mCameraTarget.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;



        mController.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(mController.velocity.x, mController.velocity.z).magnitude;

        if (mController.isGrounded)
        {
            velocityY = 0;
        }

    }

    void Jump()
    {
        if (mController.isGrounded)
        {
            mAnimatior.SetTrigger(jumpAnimationTriggerName);
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (mController.isGrounded)
        {
            return smoothTime;
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / airControlPercent;
    }

    public void SetPlayerPosition(Transform target)
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}