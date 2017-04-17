using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameController gameController;

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

    Animator animator;
    Transform cameraT;
    CharacterController controller;

    bool cover = false;
    Collider coverCollider;

    void Start()
    {
        //animator = GetComponent<Animator>();
        cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController>();

        //gameController.SetPlayerStatus("Walking");
    }

    void Update()
    {
        // input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        bool running = Input.GetKey(KeyCode.LeftShift);

        Move(inputDir, running);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        // animator
        //float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
        //animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

    }
    void Move(Vector2 inputDir, bool running)
    {
        if (cover)
        {
            Vector3 normal = new Vector3(0, 1, 0);
            Vector3 tangent = Vector3.Cross(normal, coverCollider.transform.forward);
            if (tangent.magnitude == 0)
            {
                tangent = Vector3.Cross(normal, Vector3.up);
            }
            inputDir = Input.GetAxis("Horizontal") * tangent;
        }
        if (inputDir != Vector2.zero)
        {
            //Camera rotation
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

        if (controller.isGrounded)
        {
            velocityY = 0;
        }

    }


    void Jump()
    {
        if (controller.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (controller.isGrounded)
        {
            return smoothTime;
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / airControlPercent;
    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log("PlayerTriggerStay");
        if ((other.gameObject.name == "Barrier") || (other.gameObject.name == "Barrier(Clone)"))
        {
            cover = true;
            coverCollider = other;
            //gameController.SetPlayerStatus("Hiding");
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Barrier")
        {
            cover = false;
            //gameController.SetPlayerStatus("Walking");
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("OnCollisionEnter");
        Debug.Log(other.gameObject.name);
        if (other.gameObject.name == "Egg(Clone)" || other.gameObject.name == "Egg")
        {
            Destroy(other.gameObject);
            //gameController.AddScore();
        }

    }


}