using System;
using UnityEngine;

public class PlayerAE : MonoBehaviour
{
    public static event Action OnDanceEnded;
    public static event Action OnJumpEnded;
    public static event Action OnAttackEnded;

    private void EndDance() => OnDanceEnded?.Invoke();
    private void EndJump() => OnJumpEnded?.Invoke();
    private void EndAttack() => OnAttackEnded?.Invoke();
}