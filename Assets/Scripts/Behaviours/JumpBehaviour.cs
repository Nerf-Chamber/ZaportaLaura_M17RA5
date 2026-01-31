using UnityEngine;

public class JumpBehaviour : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake() { _rb = GetComponent<Rigidbody>(); }
    public void Jump(float jumpForce)
    {
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    public bool IsGrounded(Vector3 position)
    {
        bool hitFloor = Physics.Raycast(position, Vector3.down, 0.05f, LayerMask.GetMask("Floor"));
        return hitFloor;
    }
}