using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string saveString;

    bool hasBeenOpened = false;
    bool open = false;

    float initialYRotation;

    private void Awake()
    {
        Player.OnSavePressed += SaveDoor;
        initialYRotation = transform.eulerAngles.y;
    }
    private void Start() => GameManager.Instance.LoadObject(gameObject, saveString);
    private void Update() 
    {
        if (open && transform.eulerAngles.y < initialYRotation + 90) OpenDoor(); 
    }
    public void Interact(Player player)
    {
        Item playerItem = player.GetCurrentItem();
        if (playerItem is Key key && !hasBeenOpened)
        {
            if (key.associatedDoor == this) open = true;
        }
    }
    private void OpenDoor()
    {
        transform.Rotate(new Vector3(0, Time.deltaTime * 90, 0), Space.World);
        hasBeenOpened = true;
    }
    private void SaveDoor() => GameManager.Instance.SaveObject(gameObject, saveString);
}
