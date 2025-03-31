using UnityEngine;

public class ItemHolder : MonoBehaviour, IPickable
{
    public ItemBaseClass item;

    public void PickUp(Inventory inventory)
    {
        inventory.AddItem(item);
    }
}
