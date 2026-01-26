using UnityEngine;

public abstract class Item : MonoBehaviour, IInteractable
{
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 1f;

    protected Vector3 startPos;
    protected bool isCollected = false;

    private void Start() => startPos = transform.position;
    private void Update() => FloatUpDown();
    protected void FloatUpDown()
    {
        if (!isCollected)
        {
            float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
            transform.position = new Vector3(startPos.x, newY, startPos.z);
        }
    }
    protected virtual void Collected() { }

    public void Interact(Player player)
    {
        isCollected = true;
        player.SetCurrentItem(this);
    }
}