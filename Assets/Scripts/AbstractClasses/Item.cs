using UnityEngine;

public abstract class Item : MonoBehaviour, IInteractable
{
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 1f;
    public float rotationSpeed = 45;

    protected Vector3 pos;
    protected Quaternion startRotation;
    protected float startY;
    protected bool isCollected = false;

    private void Awake()
    {
        Player.OnDropItem += RestoreState;
        startY = transform.position.y;
    }
    private void Start() 
    {
        pos = transform.position;
        startRotation = transform.rotation;
    }
    private void Update() => FloatUpDown();
    private void RestoreState(Vector3 playerPos)
    {
        if (isCollected)
        {
            if (TryGetComponent<Collider>(out var col)) { col.enabled = true; }

            pos = new Vector3(playerPos.x, playerPos.y + 1f, playerPos.z);
            isCollected = false;
            transform.SetPositionAndRotation(playerPos, startRotation);
        }
    }
    protected void FloatUpDown()
    {
        if (!isCollected)
        {
            float newY = pos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
            transform.position = new Vector3(pos.x, newY, pos.z);

            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
        }
    }
    public void Interact(Player player)
    {
        if (TryGetComponent<Collider>(out var col)) { col.enabled = false; }
    
        isCollected = true;
        player.SetCurrentItem(this);
    }
}
