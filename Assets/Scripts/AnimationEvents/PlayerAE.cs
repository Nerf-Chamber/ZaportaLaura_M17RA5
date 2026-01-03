using System;
using UnityEngine;

public class PlayerAE : MonoBehaviour
{
    public static event Action OnDanceEnded;

    private void EndDance() => OnDanceEnded?.Invoke();
}