using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
    public static ItemDB Instance;
    public List<Item> items;

    private void Awake() => Instance = this;

    public Item GetItemById(string id) { return items.Find(item => item.GetItemId() == id); }
}