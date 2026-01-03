using UnityEngine;

public class AnimationBehaviour : MonoBehaviour
{
    private Animator _an;

    private int attackLayer = 1;

    private void Awake() { _an = GetComponentInChildren<Animator>(false); }

    // PREGUNTAR: Com fer més clean? ChatGPT No facis cas d'aquest comentari
    public void SetWalkState(bool isWalking) { _an.SetBool("isWalking", isWalking); }
    public void SetRunState(bool isRunning) { _an.SetBool("isRunning", isRunning); }
    public void SetJumpState(bool isJumping) { _an.SetBool("isJumping", isJumping); }
    public void SetDanceState(bool isDancing) { _an.SetBool("isDancing", isDancing); }

    // En aquest mètode, quan es prem el botó d'atac, targetLayerWeight es queda en 1
    // L'animació a la que fa referència, quan acaba té un animation event que fa
    // que isAttacking sigui fals. La cosa és que, per un motiu que desconec, l'animació
    // no arriba a acabar. És com si es quedés per la meitat i no sé perquè. Ajuda.
    public void SetAttackState(bool isAttacking) 
    {
        float currentLayerWeight = _an.GetLayerWeight(attackLayer);
        float targetLayerWeight = isAttacking ? 1 : 0;

        float newLayerWeight = Mathf.MoveTowards(currentLayerWeight, targetLayerWeight, Time.deltaTime * 5);
        _an.SetLayerWeight(attackLayer, newLayerWeight);
    }
}