using UnityEngine;

public class RotationBehaviour : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake() { _rb = GetComponent<Rigidbody>(); }
    public void Rotate(Vector3 direction, float rotationSpeed) 
    {
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}