using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public void Interact(Player player)
    {
        Item playerItem = player.GetCurrentItem();
        if (playerItem is Key key)
        {
            if (key.associatedDoor == this) 
            { 
                // Here code for rotating door
                Debug.Log("Rotate door"); 
            }
        }
    }
}
