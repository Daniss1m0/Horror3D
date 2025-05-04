using UnityEngine;

public class DroppableItem : MonoBehaviour, IUsable
{
    public Light lightspot;
    private ItemBaseClass itemData;

    public void Initialize(ItemBaseClass item)
    {
        this.itemData = item;
        lightspot.enabled = false;
    }

    public void Use()
    {
        lightspot.enabled = !lightspot.enabled;
    }
}