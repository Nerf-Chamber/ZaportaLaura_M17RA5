using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MoveBehaviour))]
[RequireComponent(typeof(JumpBehaviour))]
[RequireComponent(typeof(RotationBehaviour))]
[RequireComponent(typeof(AnimationBehaviour))]

public class Player : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] private MoveBehaviour _move;
    [SerializeField] private JumpBehaviour _jump;
    [SerializeField] private RotationBehaviour _rotation;
    [SerializeField] private AnimationBehaviour _animation;
    [SerializeField] private Transform handSocket;
    [SerializeField] private Transform followFor3P;
    [SerializeField] private Item currentItem;
    [SerializeField] private int walkingSpeed;
    [SerializeField] private int runningSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float aimRotationSpeed;

    private InputSystem_Actions inputActions;
    private IInteractable nearbyInteractable;

    private Vector3 onMoveDirection = Vector3.zero;
    private Vector2 tempDirection = Vector2.zero;

    // Jump force fixa
    private int jumpForce = 35;
    private int speed = 0;

    private bool isMoving = false;
    private bool isSprinting = false;
    private bool isJumping = false;
    private bool isAttacking = false;
    private bool isDancing = false;
    private bool isAiming = false;

    private bool hasWon = false;

    public static event Action<Vector3> OnDropItem;
    public static event Action OnSavePressed;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);

        _move = GetComponent<MoveBehaviour>();
        _jump = GetComponent<JumpBehaviour>();
        _rotation = GetComponent<RotationBehaviour>();
        _animation = GetComponent<AnimationBehaviour>();

        PlayerAE.OnJumpEnded += EndJump;
        PlayerAE.OnAttackEnded += EndAttack;
        PlayerAE.OnDanceEnded += EndDance;
    }
    private void Start()
    {
        SavingManager.Instance.LoadPlayer(this);
        followFor3P.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Update()
    {
        _move.Move(onMoveDirection, speed);
        ManageAnimationStates();
        ManageSpeed();
        ManageRotation();
        ManageFirstPersonRotation();

        if (isAiming) ManageMovementFirstPerson(tempDirection);
        else ManageMovementThirdPerson(tempDirection);
    }

    // ---------- INTERFACE IMPLEMENTATION ----------
    public void OnAttack(InputAction.CallbackContext context) => isAttacking = isDancing? false : true;
    public void OnSave(InputAction.CallbackContext context)
    {
        SavingManager.Instance.SavePlayer(this);
        OnSavePressed?.Invoke();
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (nearbyInteractable != null)
            nearbyInteractable.Interact(this);
    }
    public void OnDrop(InputAction.CallbackContext context)
    {
        if (currentItem != null)
            DropCurrentItem();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (_jump.IsGrounded(transform.position) && !isDancing && !isJumping)
        {
            _jump.Jump(jumpForce);
            isJumping = true;
        }
    }
    public void OnDance(InputAction.CallbackContext context)
    {
        if (!isDancing)
        {
            isDancing = true;
            CameraManager.Instance.SetCameraTopPriority(Cameras.Dance);
        }
    }
    public void OnMove(InputAction.CallbackContext context) => tempDirection = context.ReadValue<Vector2>();
    public void OnSprint(InputAction.CallbackContext context) => isSprinting = context.performed ? true : false;
    public void OnAim(InputAction.CallbackContext context)
    {
        if (!isDancing)
        {
            Cameras cam = context.performed ? Cameras.FirstPerson : Cameras.ThirdPerson;
            isAiming = context.performed ? true : false;

            CameraManager.Instance.SetCameraTopPriority(cam);
        }
    }

    // ---------- MANAGE METHODS ----------
    private void ManageAnimationStates()
    {
        _animation.SetRunState(isMoving && isSprinting);
        _animation.SetWalkState(isMoving && !isSprinting);
        _animation.SetJumpState(isJumping);
        _animation.SetAttackState(isAttacking);
        _animation.SetDanceState(isDancing);
        _animation.SetAimState(isAiming);
        _animation.SetCurrentLayer(isAiming || isAttacking);
    }
    private void ManageSpeed()
    {
        if (isMoving && !isDancing) { speed = isSprinting ? runningSpeed : walkingSpeed; }
        else { speed = 0; }
    }
    private void ManageRotation()
    {
        if (isMoving && !isDancing) { _rotation.Rotate(onMoveDirection, rotationSpeed); }
    }
    private void ManageFirstPersonRotation()
    {
        if (isAiming)
        {
            float rotationY = CameraManager.Instance.GetCamRotationXValue(Cameras.FirstPerson);
            Vector3 currentEuler = transform.eulerAngles;

            Quaternion targetRotation = Quaternion.Euler(currentEuler.x, rotationY, currentEuler.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * aimRotationSpeed);
        }
    }
    private void ManageMovementThirdPerson(Vector2 tempDirection)
    {
        isMoving = tempDirection != Vector2.zero ? true : false;

        if (isMoving)
        {
            Vector3 tpCamPosition = CameraManager.Instance.GetCamPosition(Cameras.ThirdPerson);
            Vector3 camPlayerVector = new Vector3(tpCamPosition.x - transform.position.x, onMoveDirection.y, tpCamPosition.z - transform.position.z);

            camPlayerVector = Quaternion.Euler(0f, CameraManager.Instance.GetAngleCam(tempDirection), 0f) * camPlayerVector;

            onMoveDirection = -1 * camPlayerVector.normalized;
        }
    }
    private void ManageMovementFirstPerson(Vector2 tempDirection)
    {
        if (tempDirection.y <= 0 || tempDirection == Vector2.zero)
        {
            isMoving = false;
            onMoveDirection = Vector2.zero;
        }
        else
        {
            isMoving = true;

            Vector3 fpCamPosition = CameraManager.Instance.GetCamPosition(Cameras.FirstPerson);
            Vector3 camPlayerVector = new Vector3(fpCamPosition.x - transform.position.x, onMoveDirection.y, fpCamPosition.z - transform.position.z);

            camPlayerVector = Quaternion.Euler(0f, CameraManager.Instance.GetAngleCam(tempDirection), 0f) * camPlayerVector;

            onMoveDirection = -1 * camPlayerVector.normalized;
        }
    }

    // ---------- ANIMATION EVENTS ----------
    private void EndJump() => isJumping = false;
    private void EndAttack() => isAttacking = false;
    private void EndDance()
    {
        isDancing = false;
        CameraManager.Instance.SetCameraTopPriority(Cameras.ThirdPerson);
    }

    // ---------- ITEM INTERACTION ----------
    public void SetCurrentItem(Item item)
    {
        item.transform.SetParent(null);

        currentItem = item;
        currentItem.transform.SetParent(handSocket);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.identity;
        currentItem.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    public Item? GetCurrentItem() { return currentItem; }
    private void DropCurrentItem() 
    { 
        currentItem.transform.SetParent(null); 
        currentItem = null; 
        OnDropItem?.Invoke(transform.position);
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out IInteractable interactable))
            nearbyInteractable = interactable;
        if (collider.gameObject.layer == LayerMask.NameToLayer("Win") && !hasWon)
        {
            hasWon = true;
            isDancing = true;
            CameraManager.Instance.SetCameraTopPriority(Cameras.Dance);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(out IInteractable interactable))
        {
            if (nearbyInteractable == interactable)
                nearbyInteractable = null;
        }
    }
}
