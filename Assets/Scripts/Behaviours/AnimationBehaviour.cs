using UnityEngine;

public class AnimationBehaviour : MonoBehaviour
{
    private Animator _an;

    private void Awake() { _an = GetComponentInChildren<Animator>(false); }

    // PREGUNTAR: Com fer més clean?
    public void SetWalkState(bool isWalking) { _an.SetBool("isWalking", isWalking); }
    public void SetRunState(bool isRunning) { _an.SetBool("isRunning", isRunning); }
}