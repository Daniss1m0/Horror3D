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