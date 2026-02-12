using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Box : MonoBehaviour, IPunchable
{
    [SerializeField] private Transform playerTransform;
    private Rigidbody rb;

    private void Awake() => rb = GetComponent<Rigidbody>();

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Punch")) 
            Punched(playerTransform);
    }
    public void Punched(Transform playerTransform) => rb.AddForce((transform.position - playerTransform.position) * 400);
}