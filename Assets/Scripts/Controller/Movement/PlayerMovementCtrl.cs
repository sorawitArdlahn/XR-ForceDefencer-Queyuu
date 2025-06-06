using System.Collections;
using Controller.Stats;
using Model.Stats;
using Presenter.Sound;
using UnityEngine;

namespace Controller.Movement
{
    public class PlayerMovementCtrl : MonoBehaviour
    {
        [Header("Input Receiver")] public PlayerInputReceiver playerInputReceiver;

        [Header("Character Info")] public RobotInGameStatsCtrl robotInGameStatsCtrl; //use fuel form robot info class instead
        [Header("Settings")] 
        public float gravity = 9.81f; // use for config gravity in game
        private bool _isDragToGroundActive; // use for turn on-off DragToGround() 
        [SerializeField] private MovementState movementState;
        private MoveCockpitMode moveCockpitMode;
        private bool _isCanUseFuel; // for delay using fuel, solve frame looping using fuel so much in 1 second

        [Header("Ground Check")] public float playerHeight;
        public Transform groundCheck;
        public LayerMask groundLayer;
        private bool isGrounded;
        
        [Header("Aim Assistant")]
        public AimAssistantCtrl AimAssistantCtrl;

        [Header("Cockpit Movement")] public Transform cockpitTransform;
        public float sensitivityX;
        public float sensitivityY;
        public float angleLimit = 90; // degree

        private float lookX;
        private float lookY;
        private float cockpitRotationX;
        private float cockpitRotationY;

        [Header("Character Movement | Walk&Sprint")] [SerializeField]
        private float moveSpeed = 0;
        public float walkSpeed;
        public float sprintSpeed;
        public int sprintRequiredFuel; //only sprint 
        
        private float horizontalInput;
        private float verticalInput;
        private bool isInputSprinting;
        private Vector3 moveDirection;
        private Rigidbody rb;
        public float groundDrag;

        [Header("Character Movement | Dash")] 
        public float dashForce;
        public int dashRequiredFuel;
        private bool isInputDashing;
        
        [Header("Character Movement | Jump")] 
        public float jumpForce;
        private bool isInputJumping;

        [Header("Character Movement | Fly")] 
        public float ascendSpeed;
        public float airMultiplier;
        public int flyRequiredFuel;
        private bool isInputFlying;

        [Header("Sound Effects")] 
        public AudioManagerV2 audioManager;
        public AudioClip walkSound;
        public AudioClip flySound;
        
        
        

        private void Start()
        {
            // make cursor drag in the middle of the screen and disappear 
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            Physics.gravity = new Vector3(0, -this.gravity, 0);
            _isDragToGroundActive = true;
            // _isCanUseFuel = true;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape)){
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            ReceiveInput();
            GroundCheck();
            MovementStateHandler();
            MoveCockpitModeHandler();
            SpeedControl();
            DragCharacter();
            MoveCockpit();
            
        }

        private void FixedUpdate()
        {
            if (isInputDashing && (movementState != MovementState.Sprinting) && CheckFuel(dashRequiredFuel)) Dash();
            if (!isInputFlying && !isInputJumping && !isGrounded) DragToGround();
            if (isInputJumping && isGrounded) Jump();
            if (isInputFlying && !isGrounded) Fly();
            MoveCharacter();
            
        }

        private void ReceiveInput()
        {
            // get variable from PlayerInputReceiver
            lookX = playerInputReceiver.LookPosition.x * Time.deltaTime * sensitivityX;
            lookY = playerInputReceiver.LookPosition.y * Time.deltaTime * sensitivityY;

            horizontalInput = playerInputReceiver.Movement.x;
            verticalInput = playerInputReceiver.Movement.z;
            
            isInputJumping = playerInputReceiver.IsJumping;
            isInputFlying = playerInputReceiver.IsFlying;
            isInputSprinting = playerInputReceiver.IsSprinting;
            isInputDashing = playerInputReceiver.IsDashing;
        }

        private void GroundCheck()
        {
            //isGrounded = Physics.Raycast(groundCheck.position, Vector3.down,playerHeight * 0.5f + 0.5f, groundLayer);
            isGrounded = Physics.CheckSphere(groundCheck.position, 1f, groundLayer);
        }

        private void MovementStateHandler()
        {
            // Mode - Sprinting
            if (isGrounded && isInputSprinting && CheckFuel(sprintRequiredFuel))
            {
                movementState = MovementState.Sprinting;
                moveSpeed = sprintSpeed;
                // StartCoroutine(UseFuel(sprintRequiredFuel));
                robotInGameStatsCtrl.StartCoroutine(robotInGameStatsCtrl.UseFuel(sprintRequiredFuel));
                //audioManager.PlayNextClip(walkSound);
            }

            // Mode - Walking
            else if (isGrounded && rb.velocity != Vector3.zero)
            {
                movementState = MovementState.Walking;
                moveSpeed = walkSpeed;
                //walkingSoundAudioSource.PlayOneShot(walkingSound);
                audioManager.PlayNextClip(walkSound, 0.05f);
            }

            // Mode - Air
            else if (!isGrounded)
            {
                movementState = MovementState.Air;
                moveSpeed = walkSpeed * airMultiplier;
                audioManager.PlayNextClip(flySound, 0.05f);
            }

            else
            {
                movementState = MovementState.None;
                audioManager.Stop();
            }
        }

        private void MoveCockpitModeHandler()
        {
            if (playerInputReceiver.IsLockOn)
            {
                moveCockpitMode = MoveCockpitMode.Locked;
            }
            else
                moveCockpitMode = MoveCockpitMode.FreeLock;
        }

        private void DragCharacter()
        {
            if (isGrounded) rb.drag = groundDrag;
            else rb.drag = 0;
        }

        private void MoveCockpit()
        {
            if (!playerInputReceiver.IsLockOn || !AimAssistantCtrl.IsEnemyAvailable())
            {
                AimAssistantCtrl.enabled = false;
                cockpitRotationY += lookX;
                cockpitRotationX -= lookY;
                
                // make cockpit have a limit of only 90 degrees of head tilt.
                cockpitRotationX = Mathf.Clamp(cockpitRotationX, -angleLimit, angleLimit);

                cockpitTransform.rotation = Quaternion.Euler(cockpitRotationX, cockpitRotationY, 0);
                transform.rotation = Quaternion.Euler(0, cockpitRotationY, 0);
                
                if (!AimAssistantCtrl.IsEnemyAvailable())
                {
                    AimAssistantCtrl.enabled = true;
                }
            }
            else
            {
                cockpitRotationY = AimAssistantCtrl.GetNewAngles(cockpitRotationY).y;
                cockpitRotationX = AimAssistantCtrl.GetNewAngles(cockpitRotationX).x;
                AimAssistantCtrl.enabled = true;
            }
            
            
        }

        private void MoveCharacter()
        {
            //calculate movement direction
            moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
            //moveDirection = cockpitTransform.forward * verticalInput + cockpitTransform.right * horizontalInput;
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * rb.mass), ForceMode.Force);
        }

        private void SpeedControl()
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            Vector3 flyFlatVelocity = new Vector3(0, rb.velocity.y, 0);

            // limit velocity of needed
            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }

            if (flyFlatVelocity.magnitude > ascendSpeed)
            {
                Vector3 limitedVelocity = flyFlatVelocity.normalized * ascendSpeed;
                rb.velocity = new Vector3(rb.velocity.x, limitedVelocity.y, rb.velocity.z);
            }
        }

        private void Jump()
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * (jumpForce * rb.mass), ForceMode.Impulse);
        }

        private void Fly()
        {
            if (CheckFuel(flyRequiredFuel)){

                robotInGameStatsCtrl.StartCoroutine(robotInGameStatsCtrl.UseFuel(flyRequiredFuel));
                ; // not hold fly and character still stand on ground
                // StartCoroutine(UseFuel(flyRequiredFuel));
                rb.AddForce(transform.up * (ascendSpeed * rb.mass), ForceMode.Impulse);
            }

            else{
                
                DragToGround();
            }
            
        }

        private void Dash()
        {
            // StartCoroutine(UseFuel(dashRequiredFuel));
            robotInGameStatsCtrl.StartCoroutine(robotInGameStatsCtrl.UseFuel(dashRequiredFuel));
            if (moveDirection == new Vector3(0,0,0))
            {
                moveDirection = transform.forward;
            }
            
            rb.AddForce(moveDirection.normalized * (dashForce * rb.mass), ForceMode.Impulse);
        }
        
        private void DragToGround()
        {
            if (!_isDragToGroundActive) return;
            float tempVelocity = rb.velocity.y;
            tempVelocity += -gravity;
            rb.velocity = new Vector3(rb.velocity.x, tempVelocity * 1, rb.velocity.z);
        }

        // private IEnumerator UseFuel(int fuelAmount)
        // {
        //     if (_isCanUseFuel)
        //     {
        //         _isCanUseFuel = false;
        //         fuel -= fuelAmount;
        //         //place api use fuel here
        //         yield return new WaitForSeconds(0.1f);
        //         _isCanUseFuel = true;
        //     }
        // }

        private bool CheckFuel(int fuelAmount)
        {
            if (robotInGameStatsCtrl.CurrentFuel >= fuelAmount)
            {
                return true;
            }
            
            return false;
        }
    } 
    public enum MovementState
    {
        None,
        Walking,
        Sprinting,
        Air,
        AutoDrive,
    }

    public enum MoveCockpitMode
    {
        FreeLock,
        Locked,
    }
}