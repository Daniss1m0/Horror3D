using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Object", menuName = "Scriptable Objects/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField] private List<InventorySlot> InventorySpace = new List<InventorySlot>(2);
    private int InventoryMaxSlots = 2;
    public void AddItem(ItemBaseClass item)
    {
        if (InventorySpace.Count < InventoryMaxSlots)
            InventorySpace.Add(new InventorySlot(item));
        else
            Debug.Log("Not enought space in inventory");
    }
    public void RemoveItem(ItemBaseClass Item)
    {
        if (InventorySpace.Count != 0)
            InventorySpace.Remove(new InventorySlot(Item));
        else
            Debug.Log("Inventory is already empty! Can't remove nothing, sorry >:(");
    }
    public void RemoveLastItem()
    {
        if (InventorySpace.Count != 0)
            InventorySpace.Remove(InventorySpace[InventorySpace.Count - 1]);
        else
            Debug.Log("Inventory is already empty! Can't remove nothing, sorry >:(");
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
    public GameObject GetItemToDrop()
    {
        return InventorySpace[InventorySpace.Count - 1].item.prefab == null ?
            null :
            InventorySpace[InventorySpace.Count - 1].item.prefab;
    }
    public InventorySlot GetInventorySlot(int id)
    {
        return InventorySpace[id];
    }
    private void OnValidate()
    {
        if (InventorySpace.Count > InventoryMaxSlots)
        {
            Debug.Log("Can't add more items");
            RemoveLastItem();
        }
    }
    public int GetInventorySize()
    {
        return InventorySpace.Count;
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