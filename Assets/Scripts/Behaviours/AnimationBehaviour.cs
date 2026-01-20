using UnityEngine;

public class AnimationBehaviour : MonoBehaviour
{
    private Animator _an;

    private int attackLayer = 1;

    private void Awake() { _an = GetComponentInChildren<Animator>(false); }

    public void SetWalkState(bool isWalking) { _an.SetBool("isWalking", isWalking); }
    public void SetRunState(bool isRunning) { _an.SetBool("isRunning", isRunning); }
    public void SetJumpState(bool isJumping) { _an.SetBool("isJumping", isJumping); }
    public void SetDanceState(bool isDancing) { _an.SetBool("isDancing", isDancing); }
    public void SetAttackState(bool isAttacking) { _an.SetBool("isAttacking", isAttacking); }
    public void SetAimState(bool isAiming) { _an.SetBool("isAiming", isAiming); }

    public void SetCurrentLayer(bool isAimingOrAttacking)
    {
        float currentLayerWeight = _an.GetLayerWeight(attackLayer);
        float targetLayerWeight = isAimingOrAttacking ? 1 : 0;

        float newLayerWeight = Mathf.MoveTowards(currentLayerWeight, targetLayerWeight, Time.deltaTime * 5);
        _an.SetLayerWeight(attackLayer, newLayerWeight);
    }
}