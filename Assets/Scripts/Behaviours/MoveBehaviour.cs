using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake() { _rb = GetComponent<Rigidbody>(); }
    public void Move(Vector3 direction, float speed)
    {
        Vector3 velocity = direction * speed;
        velocity.y = _rb.linearVelocity.y;
        _rb.linearVelocity = velocity;
    }
}