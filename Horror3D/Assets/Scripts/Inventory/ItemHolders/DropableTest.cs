using UnityEngine;

public class DroppableItem : MonoBehaviour, IUsable
{
    public Light lightspot;
    public Inventory inventory;
    public ItemBaseClass itemData;

    //public void Initialize(ItemBaseClass item)
    //{
    //    this.itemData = item;
    //    lightspot.enabled = false;
    //}

    public void Use()
    {
        lightspot.enabled = !lightspot.enabled;
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
    }
    public void Update()
    {
        if (inventory != null)
        {
            if (inventory.CheckForItem(itemData))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Use();
                }
            }
        } 
    }

    
}