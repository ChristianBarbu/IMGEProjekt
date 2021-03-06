using System;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
///     Controller that handles the character controls and camera controls of the first person player.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour, ICharacterSignals
{
    #region Character Signals

    public IObservable<Vector3> Moved => _moved;
    private Subject<Vector3> _moved;

    public ReactiveProperty<bool> IsRunning => _isRunning;
    private ReactiveProperty<bool> _isRunning;

    public ReactiveProperty<bool> IsShooting => _isShooting;
    private ReactiveProperty<bool> _isShooting;

    public ReactiveProperty<bool> IsUsingAbility => _isUsingAbility;
    private ReactiveProperty<bool> _isUsingAbility;

    public IObservable<Unit> Landed => _landed;
    private Subject<Unit> _landed;

    public IObservable<Unit> Jumped => _jumped;
    private Subject<Unit> _jumped;

    public IObservable<Unit> Stepped => _stepped;
    private Subject<Unit> _stepped;

    [SerializeField]
    private Button resume;

    [SerializeField]
    private Button controls;

    [SerializeField]
    private Button exit;

    private bool crouched;
    private bool jumped = false;

    [SerializeField]
    private GameObject _cam;

    private int index = 0;
    [SerializeField]
    private int sprintDur;
    private int cooldown = 0;
    [SerializeField]
    private int sprintCooldown;
    private bool menu;
    private float time = 1f;

    #endregion

    #region Configuration

    [Header("References")]
    [SerializeField] private FirstPersonControllerInput firstPersonControllerInput;
    private CharacterController _characterController;
    private Camera _camera;

    [Header("Locomotion Properties")]
    [SerializeField] private float walkSpeed = 20f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpForceMagnitude = 10f;

    [SerializeField] private float strideLength = 4f;
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private bool jetpacks = false;
    [SerializeField] private float timeChange = 1f;
    [SerializeField] private float jetpackBoost = 100;
    public float StrideLength => strideLength;

    ReactiveProperty<bool> ICharacterSignals.IsUsingAbility => throw new NotImplementedException();

    [SerializeField] private float stickToGroundForceMagnitude = 5f;

    [Header("Look Properties")]
    [Range(-90, 0)] [SerializeField] private float minViewAngle = -60f;
    [Range(0, 90)] [SerializeField] private float maxViewAngle = 60f;

    #endregion

    private WeaponController currentWeapon;

    public WeaponController[] Weapons = new WeaponController[6];

    public ConsumableTypes consumable;
    
    [SerializeField] private GameObject jetpackIcon;
    [SerializeField] private GameObject speedBoosterIcon;
    [SerializeField] private GameObject timeLapseIcon;

    private GameObject currentIcon;

    public void ChangeIcon()
    {
        if (currentIcon != null)
        {
            currentIcon.SetActive(false);
        }
        switch (consumable)
        {
            case ConsumableTypes.Jetpack:
                currentIcon = jetpackIcon;
                break;
            case ConsumableTypes.None:
                break;
            case ConsumableTypes.Speedbooster:
                currentIcon = speedBoosterIcon;
                break;
            case ConsumableTypes.TimeLapse:
                currentIcon = timeLapseIcon;
                break;
        }
        currentIcon.SetActive(true);
    }
    
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _camera = GetComponentInChildren<Camera>();
        _isRunning = new ReactiveProperty<bool>(false);
        _isShooting = new ReactiveProperty<bool>(false);
        _moved = new Subject<Vector3>().AddTo(this);
        _jumped = new Subject<Unit>().AddTo(this);
        _landed = new Subject<Unit>().AddTo(this);
        _stepped = new Subject<Unit>().AddTo(this);
        crouched = false;
        menu = false;

        currentWeapon = Weapons[0];
        currentWeapon.gameObject.SetActive(true);
    }

    private void Start()
    {
        HandleLocomotion();

        HandleSteppedCharacterSignal();

        HandleLook();

        controls.gameObject.SetActive(false);
        resume.gameObject.SetActive(false);
        exit.gameObject.SetActive(false);
        resume.onClick.AddListener(resumeGame);
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(this._camera.transform.position, 
            this._camera.transform.forward, out hit, float.PositiveInfinity, ~10, QueryTriggerInteraction.Ignore) && hit.distance > 5)
        {
            this.currentWeapon.bulletSpawn.transform.LookAt(hit.point);
        }
        else
        {
            this.currentWeapon.bulletSpawn.transform.rotation = this.currentWeapon.transform.rotation;
        }
        //used to make the shooting cleaner
    }

    public void ChangeWeapon(int weaponId)
    {
        this.currentWeapon.GetComponent<WeaponController>().stopShooting();
        this.currentWeapon.gameObject.SetActive(false);
        this.currentWeapon = Weapons[weaponId];
        this.currentWeapon.gameObject.SetActive(true);
    }

    private void resumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = time;
        controls.gameObject.SetActive(false);
        resume.gameObject.SetActive(false);
        exit.gameObject.SetActive(false);
    }

    private void HandleLocomotion()
    {

        firstPersonControllerInput.Escape.Subscribe(_ =>
        {
            Application.Quit();
        }).AddTo(this);



        // Ensures the first frame counts as "grounded".
        _characterController.Move(-stickToGroundForceMagnitude * transform.up);

        // Create a jump latch for sync + map from events to true/false values.
        var jumpLatch = LatchObservables.Latch(this.UpdateAsObservable(), firstPersonControllerInput.Jump, false);

        _ = firstPersonControllerInput.Crouch.Subscribe(_ =>
        {
            crouched = !crouched;
            if (crouched)
            {
                _cam.transform.localPosition = _cam.transform.localPosition + new Vector3(0, -0.5f, 0);
            }
            else
            {
                _cam.transform.localPosition = _cam.transform.localPosition + new Vector3(0, 0.5f, 0);
            }

        }).AddTo(this);



        // Handle move:
        _ = firstPersonControllerInput.Move
            .Zip(jumpLatch, (m, j) => new MoveInputData(m, j))
            .Where(moveInputData => moveInputData.Jump ||
                                    moveInputData.Move != Vector2.zero ||
                                    _characterController.isGrounded == false)
            .Subscribe(i =>
            {

                if (index == 80)
                {
                    index = 0;
                    cooldown = 30;
                    crouched = false;
                }
                else if (cooldown > 0)
                {
                    cooldown--;
                }

                if (sprintDur == 200)
                {
                    sprintDur = 0;
                    sprintCooldown = 200;
                }
                else if (sprintCooldown > 0)
                {
                    sprintCooldown--;
                }


                var wasGrounded = _characterController.isGrounded;

                // Vertical movement:

                var verticalVelocity = 0f;
                // The character is ...
                RaycastHit hit;
                // ... in the air.
                if (firstPersonControllerInput.Jump2.Value && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 100, ~15, QueryTriggerInteraction.Ignore) && hit.transform.gameObject.CompareTag("Climb"))
                {
                    verticalVelocity += jumpForceMagnitude + 1;

                }
                else if (jetpacks && firstPersonControllerInput.Jump2.Value)
                {
                    verticalVelocity += jetpackBoost;

                }
                else if (i.Jump && wasGrounded)
                {
                    crouched = false;
                    // ... grounded and wants to jump.
                    verticalVelocity = jumpForceMagnitude;
                    _jumped.OnNext(Unit.Default);



                }
                else if (!wasGrounded)
                {
                    crouched = false;
                    verticalVelocity = _characterController.velocity.y + Physics.gravity.y * Time.deltaTime * 3.0f;
                }

                else
                {
                    // ... otherwise grounded.
                    verticalVelocity = -Mathf.Abs(stickToGroundForceMagnitude);
                }

                float currentSpeed;

                if (crouched)
                {


                    if (firstPersonControllerInput.Run.Value && index <= 80 && cooldown == 0)
                    {
                        currentSpeed = runSpeed * 2;
                        index++;
                    }
                    else
                    {
                        currentSpeed = walkSpeed / 2;
                    }
                }
                else
                {
                    // Horizontal movement:
                    if (firstPersonControllerInput.Run.Value && sprintDur <= 500 && sprintCooldown == 0)
                    {
                        currentSpeed = runSpeed;
                        sprintDur++;
                    }
                    else
                    {
                        currentSpeed = walkSpeed;
                    }
                }

                currentSpeed = currentSpeed * speedMultiplier * timeChange;
                verticalVelocity -= 9.81f * Time.deltaTime * timeChange;

                var horizontalVelocity = i.Move * currentSpeed; //Calculate velocity (direction * speed).

                // Combine horizontal and vertical movement.
                var characterVelocity = transform.TransformVector(new Vector3(
                horizontalVelocity.x,
                verticalVelocity,
                horizontalVelocity.y));

                // Apply movement.
                var motion = characterVelocity * Time.deltaTime;
                _characterController.Move(motion);
                // Set ICharacterSignals output signals related to the movement.
                HandleLocomotionCharacterSignalsIteration(wasGrounded, _characterController.isGrounded);
            }).AddTo(this);


        firstPersonControllerInput.Shoot.Subscribe(input =>

        {
            if (input)
            {
                this.currentWeapon.startShooting();
            }
            else
            {
                this.currentWeapon.stopShooting();
            }

        }
        ).AddTo(this);

        firstPersonControllerInput.UseAbility.Subscribe(input =>
        {
            if (input)
            {
                switch (consumable)
                {
                    case ConsumableTypes.Speedbooster:
                        speedMultiplier = 10f;
                        StartCoroutine(Timer(15));
                        consumable = ConsumableTypes.None;
                        break;
                    case ConsumableTypes.TimeLapse:
                        Time.timeScale = 0.2f;
                        timeChange = 1 / Time.timeScale;
                        StartCoroutine(Timer(10));
                        consumable = ConsumableTypes.None;
                        break;
                    case ConsumableTypes.Jetpack:
                        jetpacks = true;
                        StartCoroutine(Timer(20));
                        consumable = ConsumableTypes.None;
                        break;
                }
                currentIcon.SetActive(false);
            }
        }
        ).AddTo(this);


    }

    private void ReturnConsumableValues()
    {
        speedMultiplier = 1f;
        timeChange = 1f;
        Time.timeScale = 1f;
        jetpacks = false;
    }

    private IEnumerator Timer(int seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        ReturnConsumableValues();
    }

    private void HandleLocomotionCharacterSignalsIteration(bool wasGrounded, bool isGrounded)
    {
        var tempIsRunning = false;

        if (wasGrounded && isGrounded)
        {
            // The character was grounded at the beginning and end of this frame.

            _moved.OnNext(_characterController.velocity * Time.deltaTime);

            if (_characterController.velocity.magnitude > 0)
            {
                // The character is running if the input is active and
                // the character is actually moving on the ground
                tempIsRunning = firstPersonControllerInput.Run.Value;
            }
        }

        if (!wasGrounded && isGrounded)
        {
            // The character was airborne at the beginning, but grounded at the end of this frame.

            _landed.OnNext(Unit.Default);
        }

        _isRunning.Value = tempIsRunning;
    }

    private void HandleSteppedCharacterSignal()
    {
        // Emit stepped events:

        var stepDistance = 0f;
        Moved.Subscribe(w =>
        {
            stepDistance += w.magnitude;

            if (stepDistance > strideLength)
            {
                _stepped.OnNext(Unit.Default);
            }

            stepDistance %= strideLength;
        }).AddTo(this);
    }

    private void HandleLook()
    {
        firstPersonControllerInput.Look
            .Where(v => v != Vector2.zero)
            .Subscribe(inputLook =>
            {
                // Translate 2D mouse input into euler angle rotations.

                // Horizontal look with rotation around the vertical axis, where + means clockwise.
                var horizontalLook = inputLook.x * Vector3.up * Time.deltaTime;
                transform.localRotation *= Quaternion.Euler(horizontalLook * 25);

                // Vertical look with rotation around the horizontal axis, where + means upwards.
                var verticalLook = inputLook.y * Vector3.left * Time.deltaTime;
                var newQ = _camera.transform.localRotation * Quaternion.Euler(verticalLook * 25);

                _camera.transform.localRotation =
                       RotationTools.ClampRotationAroundXAxis(newQ, -maxViewAngle, -minViewAngle);

            }).AddTo(this);
    }


    public struct MoveInputData
    {
        public readonly Vector2 Move;
        public readonly bool Jump;

        public MoveInputData(Vector2 move, bool jump)
        {
            Move = move;
            Jump = jump;
        }


    }
}