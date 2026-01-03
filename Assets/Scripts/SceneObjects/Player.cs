using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MoveBehaviour))]
[RequireComponent(typeof(AnimationBehaviour))]

public class Player : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] private MoveBehaviour _move;
    [SerializeField] private AnimationBehaviour _animation;

    private InputSystem_Actions inputActions;

    private bool isMoving = false;
    private bool isSprinting = false;
    private bool isDancing = false;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);

        _move = GetComponent<MoveBehaviour>();
        _animation = GetComponent<AnimationBehaviour>();

        PlayerAE.OnDanceEnded += EndDance;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Update()
    {
        ManageAnimationStates();
    }

    private void ManageAnimationStates()
    {
        _animation.SetRunState(isMoving && isSprinting);
        _animation.SetWalkState(isMoving && !isSprinting);
        _animation.SetDanceState(isDancing);
    }

    // ---------- INTERFACE IMPLEMENTATION ----------
    public void OnAttack(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
    public void OnDance(InputAction.CallbackContext context)
    {
        isDancing = true;
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        isMoving = context.performed ? true : false;
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.performed ? true : false;
    }

    // Animation event
    private void EndDance() => isDancing = false;
}