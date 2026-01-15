using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MoveBehaviour))]
[RequireComponent(typeof(RotationBehaviour))]
[RequireComponent(typeof(AnimationBehaviour))]

public class Player : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] private MoveBehaviour _move;
    [SerializeField] private RotationBehaviour _rotation;
    [SerializeField] private AnimationBehaviour _animation;
    [SerializeField] private int walkingSpeed;
    [SerializeField] private int runningSpeed;
    [SerializeField] private float rotationSpeed;

    private InputSystem_Actions inputActions;

    private Vector3 onMoveDirection = Vector3.zero;
    private int speed = 0;

    private bool isMoving = false;
    private bool isSprinting = false;
    private bool isJumping = false;
    private bool isAttacking = false;
    private bool isDancing = false;
    private bool isAiming = false;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);

        _move = GetComponent<MoveBehaviour>();
        _rotation = GetComponent<RotationBehaviour>();
        _animation = GetComponent<AnimationBehaviour>();

        PlayerAE.OnJumpEnded += EndJump;
        PlayerAE.OnAttackEnded += EndAttack;
        PlayerAE.OnDanceEnded += EndDance;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Update()
    {
        _move.Move(onMoveDirection, speed);
        ManageAnimationStates();
        ManageRotation();
    }

    // ---------- INTERFACE IMPLEMENTATION ----------
    public void OnAttack(InputAction.CallbackContext context)
    {
        isAttacking = isDancing ? false : true;
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        isJumping = isDancing ? false : true;
    }
    public void OnDance(InputAction.CallbackContext context)
    {
        if (!isDancing)
        {
            isDancing = true;
            CameraManager.Instance.SetCameraTopPriority(Cameras.Dance);
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        isMoving = context.performed ? true : false;
        Vector2 tempDirection = context.ReadValue<Vector2>();

        // Impediment de moviment en primera persona cap endarrere
        if (tempDirection.y < 0 && isAiming)
        {
            tempDirection = new Vector2(tempDirection.x, 0);
            if (tempDirection.x == 0) isMoving = false;
        }

        ManageMovement(tempDirection);
        ManageSpeed();
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.performed ? true : false;
        ManageSpeed();
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        if (!isDancing)
        {
            if (context.performed)
            {
                CameraManager.Instance.SetCameraTopPriority(Cameras.FirstPerson);
                isAiming = true;
            }
            else
            {
                CameraManager.Instance.SetCameraTopPriority(Cameras.ThirdPerson);
                isAiming = false;
            }
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
    private void ManageMovement(Vector2 tempDirection)
    {
        switch (CameraManager.Instance.GetCameraZone(transform.position))
        {
            case CameraRotationZones.ZoneA:
                Debug.Log("A");
                onMoveDirection = new Vector3(-tempDirection.x, onMoveDirection.y, -tempDirection.y);
                break;
            case CameraRotationZones.ZoneB:
                Debug.Log("B");
                onMoveDirection = new Vector3(-tempDirection.y, onMoveDirection.y, tempDirection.x);
                break;
            case CameraRotationZones.ZoneC:
                Debug.Log("C");
                onMoveDirection = new Vector3(tempDirection.x, onMoveDirection.y, tempDirection.y);
                break;
            case CameraRotationZones.ZoneD:
                Debug.Log("D");
                onMoveDirection = new Vector3(tempDirection.y, onMoveDirection.y, -tempDirection.x);
                break;
        }
    }

    // ---------- ANIMATION EVENTS ----------
    private void EndJump() => isJumping = false;
    private void EndDance()
    {
        isDancing = false;
        CameraManager.Instance.SetCameraTopPriority(Cameras.ThirdPerson);
    }
    private void EndAttack() => isAttacking = false;
}