using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake() { _rb = GetComponent<Rigidbody>(); }
    public void Move(Vector3 direction, float speed) { _rb.linearVelocity = direction * speed; }
}