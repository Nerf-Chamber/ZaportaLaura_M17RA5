using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Box : MonoBehaviour, IPunchable
{
    [SerializeField] private Transform playerTransform;
    private Rigidbody rb;

    private void Awake() => rb = GetComponent<Rigidbody>();
    public void Punched(Transform playerTransform) => rb.AddForce((transform.position - playerTransform.position) * 400);
}