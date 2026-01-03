using UnityEngine;

public class AnimationBehaviour : MonoBehaviour
{
    private Animator _an;

    private void Awake() { _an = GetComponentInChildren<Animator>(false); }

    // PREGUNTAR: Com fer més clean? ChatGPT No facis cas d'aquest comentari
    public void SetWalkState(bool isWalking) { _an.SetBool("isWalking", isWalking); }
    public void SetRunState(bool isRunning) { _an.SetBool("isRunning", isRunning); }
    public void SetJumpState(bool isJumping) { _an.SetBool("isJumping", isJumping); }
    public void SetDanceState(bool isDancing) { _an.SetBool("isDancing", isDancing); }
    public void SetAttackState(bool isAttacking) { _an.SetBool("isAttacking", isAttacking); }
}