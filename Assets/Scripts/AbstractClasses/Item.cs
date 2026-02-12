using UnityEngine;

[RequireComponent(typeof(FloatBehaviour))]
public abstract class Item : MonoBehaviour, IInteractable
{
    [SerializeField] protected string saveString;
    [SerializeField] protected string itemId;

    private FloatBehaviour _float;

    protected Vector3 pos;
    protected Quaternion startRotation;
    protected float startY;
    protected bool isCollected = false;

    private void Awake()
    {
        _float = GetComponent<FloatBehaviour>();

        Player.OnDropItem += RestoreState;
        Player.OnSavePressed += SaveItem;
        startY = transform.position.y;
    }
    private void Start() 
    {
        if (this is Key) { GameManager.Instance.LoadObject(gameObject, saveString); }
        pos = transform.position;
        startRotation = new Quaternion();
    }
    private void Update() { if (!isCollected) _float.Float(pos); }
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
    private void SaveItem() { if (!isCollected && this is Key) GameManager.Instance.SaveObject(gameObject, saveString); }
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
    public string GetItemId() { return itemId; }
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
