using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Object", menuName = "Scriptable Objects/Inventory")]
public class Inventory : ScriptableObject
{
    public List<InventorySlot> InventorySpace = new List<InventorySlot>();
    public void AddItem(ItemBaseClass item)
    {
        InventorySpace.Add(new InventorySlot(item));
    }

    public bool CheckForItem(ItemBaseClass Item)
    {
        foreach (InventorySlot slot in InventorySpace)
        {
            if (slot.item.id == Item.id)
                return true;
        }
        return false;
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemBaseClass item;
    public InventorySlot(ItemBaseClass item)
    {
        this.item = item;
    }
}