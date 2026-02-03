using UnityEngine;

public class SavingManager : MonoBehaviour
{
    public static SavingManager Instance;
    private void Awake() => Instance = this;

    public void SavePlayer(Player player)
    {
        PlayerData data = new PlayerData();

        data.position = player.transform.position;
        data.rotation = player.transform.rotation;
        data.currentItemId = player.GetCurrentItem() != null ? player.GetCurrentItem().itemId : null;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PlayerSave", json);
        PlayerPrefs.Save();
    }
    public void SaveDoor(Door door)
    {
        DoorData data = new DoorData();

        data.position = door.transform.position;
        data.rotation = door.transform.rotation;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("DoorSave", json);
        PlayerPrefs.Save();
    }
    // En un inventari s'hauria d'escalar per tots els items :)
    public void SaveKey(Item key)
    {
        ItemData data = new ItemData();

        data.position = key.transform.position;
        // data.rotation = key.transform.rotation;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("KeySave", json);
        PlayerPrefs.Save();
    }
    public void LoadPlayer(Player player)
    {
        if (!PlayerPrefs.HasKey("PlayerSave")) return;

        string json = PlayerPrefs.GetString("PlayerSave");
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);

        player.transform.position = data.position;
        // player.transform.rotation = data.rotation;

        if (!string.IsNullOrEmpty(data.currentItemId))
        {
            Item itemPrefab = ItemDB.Instance.GetItemById(data.currentItemId);

            if (itemPrefab != null)
            {
                Item itemInstance = Instantiate(itemPrefab);
                itemInstance.SetAsEquipped();
                player.SetCurrentItem(itemInstance);

                Item worldItem = FindWorldItem(data.currentItemId);

                if (worldItem != null)
                    worldItem.DisableInWorld();
            }
        }
    }
    public void LoadDoor(Door door)
    {
        if (!PlayerPrefs.HasKey("DoorSave")) return;

        string json = PlayerPrefs.GetString("DoorSave");
       DoorData data = JsonUtility.FromJson<DoorData>(json);

        door.transform.position = data.position;
        door.transform.rotation = data.rotation;
    }
    public void LoadKey(Item key)
    {
        // Contradicció entre LoadKey i LoadPlayer
        if (!PlayerPrefs.HasKey("KeySave")) return;

        if (!key.GetIsCollected())
        {
            string json = PlayerPrefs.GetString("KeySave");
            ItemData data = JsonUtility.FromJson<ItemData>(json);

            key.transform.position = data.position;
            key.transform.rotation = data.rotation;
        }
    }
    private Item FindWorldItem(string itemId)
    {
        Item[] items = FindObjectsByType<Item>(FindObjectsSortMode.InstanceID);
        foreach (Item item in items)
        {
            if (item.itemId == itemId && !item.GetIsCollected())
                return item;
        }
        return null;
    }
}