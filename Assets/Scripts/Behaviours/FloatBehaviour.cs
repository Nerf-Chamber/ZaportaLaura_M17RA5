using UnityEngine;

public class FloatBehaviour : MonoBehaviour
{
    [SerializeField] private float floatFrequency;
    [SerializeField] private float floatAmplitude;
    [SerializeField] private float rotationSpeed;

    private Rigidbody _rb;

    private void Awake() { _rb = GetComponent<Rigidbody>(); }
    public void Float(Vector3 position)
    {
        float newY = position.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(position.x, newY, position.z);

        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
}