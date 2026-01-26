using UnityEngine;

public abstract class Item : MonoBehaviour, IInteractable
{
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 1f;

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
        pos = playerPos;
        GetComponent<BoxCollider>().enabled = true;
        isCollected = false;
        transform.SetPositionAndRotation(playerPos, startRotation);
    }
    protected void FloatUpDown()
    {
        if (!isCollected)
        {
            float newY = pos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
            transform.position = new Vector3(pos.x, startY, pos.z);
        }
    }
    public void Interact(Player player)
    {
        GetComponent<BoxCollider>().enabled = false;
        isCollected = true;
        player.SetCurrentItem(this);
    }
}