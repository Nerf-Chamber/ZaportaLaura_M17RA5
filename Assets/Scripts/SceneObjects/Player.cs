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
    [SerializeField] private float aimRotationSpeed;

    private InputSystem_Actions inputActions;

    private Vector3 onMoveDirection = Vector3.zero;
    private Vector2 tempDirection = Vector2.zero;
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
        ManageSpeed();
        ManageRotation();
        ManageFirstPersonRotation();

        if (isAiming) ManageMovementFirstPerson(tempDirection);
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
        tempDirection = context.ReadValue<Vector2>();

        if (!isAiming) ManageMovementThirdPerson(tempDirection);
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.performed ? true : false;
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
        _animation.SetAimState(isAiming && !isAttacking);
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
            float rotationY = CameraManager.Instance.GetFPCamRotationXValue();
            Vector3 currentEuler = transform.eulerAngles;

            Quaternion targetRotation = Quaternion.Euler(currentEuler.x, rotationY, currentEuler.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * aimRotationSpeed);
        }
    }
    private void ManageMovementThirdPerson(Vector2 tempDirection)
    {
        isMoving = tempDirection != Vector2.zero ? true : false;

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
    private void ManageMovementFirstPerson(Vector2 tempDirection)
    {
        if (tempDirection.y < 0 || tempDirection == Vector2.zero)
        {
            isMoving = false;
            onMoveDirection = Vector2.zero;
        }
        else
        {
            isMoving = true;

            Vector3 fpCamPosition = CameraManager.Instance.GetFPCamPosition();
            Vector3 camPlayerVector = new Vector3(fpCamPosition.x - transform.position.x, onMoveDirection.y, fpCamPosition.z - transform.position.z);

            camPlayerVector = Quaternion.Euler(0f, GetAngleFirstPerson(tempDirection), 0f) * camPlayerVector;

            onMoveDirection = -1 * camPlayerVector.normalized;
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

    // ---------- UTILS ----------
    private float GetAngleFirstPerson(Vector2 tempDirection)
    {
        if (tempDirection.x > 0)
        {
            if (tempDirection.y > 0) return 45f;
            return 90f;
        }
        else if (tempDirection.x < 0)
        {
            if (tempDirection.y > 0) return -45f;
            return -90f;
        }
        return 0f;
    }
}