using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRB;

    protected InputMaster playerInputMaster;
    protected Animator playerAnimator;


    private SpriteRenderer _spriteRenderer;

    [SerializeField] private float jumpForce;
    [SerializeField] protected float speed;
    [SerializeField] protected float Run;
    protected float _defaultSpeed;

    //switch Character
    private bool isSwitchPressed;

    //store players input values
    private Vector3 movementVector;
    private Vector2 inputVector;
    public bool is2DMovement = false;

    //gravity variables
    private float gravity = -9.8f;
    private float groundGravity = -0.5f;// gravity when you walk

    //jumping variables
    private bool isJumpPressed = false;
    private float initialJumpVelicity;
    public float maxJumpHeight = 1f;
    public float maxJumpTime = 0.5f;
    private bool isJumping = false;

    //punch variables
    protected bool isPunchPressed = false;
    protected bool isRecovery = false;
    public GameObject hitBox;
    public GameObject hitParticle;

    //collider for groundCheck
    private float detectCollider;
    public float TEST;
    public float TEST_X;
    public float TEST_Y;
    public float TEST_Z;
    //animation
    protected int isWalkingHash;
    protected int isJumpingHash;
    protected int isPunchingHash;

    //rotation variables
    public float rotationFactorPerFrame = 1f;
    private bool rotateR;
    private bool rotateL;

    public GameObject dust;

    private RaycastHit m_hitinfo;//DELETE LATER AFTER DSTU, GARBAGE CHAT WITH NPC
    private bool m_hitdetect;

    public bool RotateR => rotateR;
    public bool RotateL => rotateL;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        playerRB = GetComponent<Rigidbody>();

        //stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);

        playerAnimator = GetComponent<Animator>();
        detectCollider = GetComponent<CapsuleCollider>().bounds.extents.y;

        isWalkingHash = Animator.StringToHash("isWalking");
        isJumpingHash = Animator.StringToHash("isJumping");
        //isPunchingHash = Animator.StringToHash("isPunching");

        playerInputMaster = new InputMaster();
        playerInputMaster.Player.Enable();
        playerInputMaster.Player.Jump.started += Jump;
        playerInputMaster.Player.Jump.canceled += Jump;
        SetUpJump();
        //playerInputMaster.Player.Punch.started += Punch;
        //playerInputMaster.Player.Punch.canceled += Punch;

        _defaultSpeed = speed;
    }


    private void Update()
    {
        SetUpJump();
        HandleAnimation();
        HandleRotation();
    }



    private void FixedUpdate()
    {
        CheckGround();
        HandleGravity();
        HandleJump();


        if (is2DMovement == true)
        {

            inputVector = playerInputMaster.Player.Movement2D.ReadValue<Vector2>();
            playerRB.velocity = new Vector2(inputVector.x * speed, movementVector.y);
        }
        else
        {

            inputVector = playerInputMaster.Player.Movement.ReadValue<Vector2>();
            playerRB.velocity = new Vector3(inputVector.x * speed, movementVector.y, inputVector.y * speed);
            // Debug.Log(playerRB.velocity);
        }
    }

    private void Switch(InputAction.CallbackContext context)
    {
        isSwitchPressed = context.ReadValueAsButton();
    }


    private void Jump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    private void HandleJump()
    {
        if (isJumpPressed == true && isJumping == false && CheckGround())
        {
            playerAnimator.SetBool(isJumpingHash, true);

            isJumping = true;
            movementVector.y = initialJumpVelicity * 0.5f;
        }
        else if (isJumpPressed == false && isJumping == true && CheckGround())
        {
            isJumping = false;
        }
    }

    private void SetUpJump() // Jump  physics 
    {
        float timeToApex = maxJumpTime / 2;
        gravity = -2 * maxJumpHeight / Mathf.Pow(timeToApex, 2);
        initialJumpVelicity = (2 * maxJumpHeight) / timeToApex;
    }

    private void Punch(InputAction.CallbackContext context)
    {
        isPunchPressed = context.ReadValueAsButton();
    }


    private void HandlePunch()
    {
        if (isPunchPressed)
        {

            isRecovery = true;
            playerAnimator.SetBool(isPunchingHash, true);
        }

        if (CheckGround() && isRecovery)
        {
            speed = 0;
        }
    }

    private void StopMove()
    {

        playerAnimator.SetBool(isPunchingHash, false);
    }

    private void ContinueMove()
    {
        isRecovery = false;
        speed = _defaultSpeed;

    }

    private void EnableHitBox()
    {
        hitBox.SetActive(true);
        hitParticle.SetActive(true);

    }

    private void DisabeleHitBox()
    {
        hitBox.SetActive(false);
        hitParticle.SetActive(false);
    }

    private void HandleAnimation()
    {
        if (inputVector.y != 0 & is2DMovement == false || inputVector.x != 0)
        {
            playerAnimator.SetBool(isWalkingHash, true);
        }
        else if (inputVector.y == 0 || inputVector.x == 0)
        {
            playerAnimator.SetBool(isWalkingHash, false);
        }
    }

    private void HandleRotation()
    {
        Quaternion currentRotation = transform.rotation;
        if (inputVector.x > 0 && inputVector.x <= 1)
        {
            rotateR = true;
            rotateL = false;
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (inputVector.x >= -1 && inputVector.x < 0)
        {
            rotateR = false;
            rotateL = true;
            GetComponent<SpriteRenderer>().flipX = true;
        }

        //if (rotateR == true)
        //{
        //    Quaternion targertRotation = Quaternion.Euler(new Vector2(0, 0));

        //    if (inputVector.x > 0 && inputVector.y > 0)
        //    {
        //        targertRotation = Quaternion.Euler(new Vector2(0, -45));
        //    }
        //    else if (inputVector.x > 0 && inputVector.y < 0)
        //    {
        //        targertRotation = Quaternion.Euler(new Vector2(0, 45));
        //    }


        //    transform.rotation = Quaternion.Lerp(currentRotation, targertRotation, rotationFactorPerFrame * Time.deltaTime);
        //}
        //else if (rotateL == true)
        //{
        //    Quaternion targertRotation = Quaternion.Euler(new Vector2(0, 180));

        //    if (inputVector.x < 0 && inputVector.y > 0)
        //    {
        //        targertRotation = Quaternion.Euler(new Vector2(0, 225));
        //    }
        //    else if (inputVector.x < 0 && inputVector.y < 0)
        //    {
        //        targertRotation = Quaternion.Euler(new Vector2(0, 135));
        //    }

        //    transform.rotation = Quaternion.Lerp(currentRotation, targertRotation, rotationFactorPerFrame * Time.deltaTime);
        //}

    }



    private void HandleGravity()
    {
        bool isFalling = movementVector.y <= 0 || isJumpPressed == false;
        float fallMultiplier = 2f;

        if (CheckGround())
        {
            playerAnimator.SetBool("isJumping", false);
            movementVector.y = groundGravity;
        }

        else if (isFalling)
        {
            float previousYVelocity = movementVector.y;
            float newYVelocity = movementVector.y + (fallMultiplier * gravity * Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * 0.5f, -20.0f);
            movementVector.y = nextYVelocity;
        }
        else
        {
            float previousYVelocity = movementVector.y;
            float newYVelocity = movementVector.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            movementVector.y = nextYVelocity;
        }

    }

    protected bool CheckGround()
    {
        LayerMask playerMask = 1 << 3;

        if (Physics.Raycast(transform.position - new Vector3(GetComponent<CapsuleCollider>().bounds.extents.x * TEST, 0, 0), transform.TransformDirection(Vector2.down), detectCollider + TEST_Y, playerMask)
            || Physics.Raycast(transform.position + new Vector3(GetComponent<CapsuleCollider>().bounds.extents.x * TEST, 0, 0), transform.TransformDirection(Vector2.down), detectCollider + TEST_Y, playerMask))
        {
            Debug.DrawRay(transform.position - new Vector3(GetComponent<CapsuleCollider>().bounds.extents.x * TEST, 0, 0), transform.TransformDirection(Vector2.down) * (GetComponent<CapsuleCollider>().bounds.extents.y + TEST_Y), Color.green);
            Debug.DrawRay(transform.position + new Vector3(GetComponent<CapsuleCollider>().bounds.extents.x * TEST, 0, 0), transform.TransformDirection(Vector2.down) * (GetComponent<CapsuleCollider>().bounds.extents.y + TEST_Y), Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position - new Vector3(GetComponent<CapsuleCollider>().bounds.extents.x * TEST, 0, 0), transform.TransformDirection(Vector2.down) * (GetComponent<CapsuleCollider>().bounds.extents.y + TEST_Y), Color.red);
            Debug.DrawRay(transform.position + new Vector3(GetComponent<CapsuleCollider>().bounds.extents.x * TEST, 0, 0), transform.TransformDirection(Vector2.down) * (GetComponent<CapsuleCollider>().bounds.extents.y + TEST_Y), Color.red);
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy hit");
            if (rotateR == true)
            {
                playerRB.AddForce(new Vector2(-1, -1) * 3000);
            }
            else if (rotateL == true)
            {
                playerRB.AddForce(new Vector2(1, 1) * 3000);
            }
        }
    }


    private void OnEnable()
    {
        playerInputMaster.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputMaster.Player.Disable();
    }
}
