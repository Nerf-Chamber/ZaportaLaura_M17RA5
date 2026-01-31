using UnityEngine;

public abstract class Item : MonoBehaviour, IInteractable
{
    public string itemId;
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
        Player.OnSavePressed += SaveItem;
        startY = transform.position.y;
    }
    private void Start() 
    {
        if (this is Key) { SavingManager.Instance.LoadKey(this); }
        pos = transform.position;
        startRotation = new Quaternion();
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
    // En un inventari s'hauria d'escalar per tots els items :)
    private void SaveItem() { if (!isCollected && this is Key) SavingManager.Instance.SaveKey(this); }
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
        if (player.GetCurrentItem() == null)
        {
            if (TryGetComponent<Collider>(out var col)) { col.enabled = false; }

            isCollected = true;
            player.SetCurrentItem(this);
        }
    }
    public bool GetIsCollected() { return isCollected; }
    public void SetAsEquipped()
    {
        isCollected = true;
        if (TryGetComponent<Collider>(out var col)) col.enabled = false;
    }
    public void DisableInWorld()
    {
        isCollected = true;
        if (TryGetComponent<Collider>(out var col)) col.enabled = false;
        gameObject.SetActive(false);
    }
}
