using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake() => Instance = this;

    public void SavePlayer(Player player)
    {
        PlayerData data = new PlayerData();

        data.position = player.transform.position;
        data.rotation = player.transform.rotation;
        data.currentItemId = player.GetCurrentItem() != null ? player.GetCurrentItem().GetItemId() : null;
        data.hasWon = player.hasWon;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PlayerSave", json);
        PlayerPrefs.Save();
    }
    public void SaveObject(GameObject obj, string saveString)
    {
        Data data = new Data();

        data.position = obj.transform.position;
        data.rotation = obj.transform.rotation;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(saveString, json);
        PlayerPrefs.Save();
    }
    public void LoadPlayer(Player player)
    {
        if (!PlayerPrefs.HasKey("PlayerSave")) return;

        string json = PlayerPrefs.GetString("PlayerSave");
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);

        player.transform.position = data.position;
        player.transform.rotation = data.rotation;
        player.hasWon = data.hasWon;

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
    public void LoadObject(GameObject obj, string saveString)
    {
        if (!PlayerPrefs.HasKey(saveString)) return;

        string json = PlayerPrefs.GetString(saveString);
        Data data = JsonUtility.FromJson<Data>(json);

        obj.transform.position = data.position;
        obj.transform.rotation = data.rotation;
    }
    private Item FindWorldItem(string itemId)
    {
        Item[] items = FindObjectsByType<Item>(FindObjectsSortMode.InstanceID);
        foreach (Item item in items)
        {
            if (item.GetItemId() == itemId && !item.GetIsCollected())
                return item;
        }
        return null;
    }
}