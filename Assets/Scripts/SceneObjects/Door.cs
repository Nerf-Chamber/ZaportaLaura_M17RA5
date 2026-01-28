using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    bool hasBeenOpened = false;

    public void Interact(Player player)
    {
        Item playerItem = player.GetCurrentItem();
        if (playerItem is Key key && !hasBeenOpened)
        {
            if (key.associatedDoor == this) 
            { 
                // Here code for rotating door
                Debug.Log("Rotate door"); 
                hasBeenOpened = true;
            }
        }
    }
}
