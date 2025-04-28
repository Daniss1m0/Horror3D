using System;
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
        if (HasFreeSpace())
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
        try
        {
            return InventorySpace[InventorySpace.Count - 1].item.prefab;
        }
        catch (ArgumentOutOfRangeException e)
        {
            Debug.Log("Exception catched");
        }
        return null;
    }
    public InventorySlot GetInventorySlot(int id)
    {
        if (InventorySpace[id] != null)
            return InventorySpace[id];
        return null;
    }
    public int GetInventorySize()
    {
        return InventorySpace.Count;
    }
    public bool HasFreeSpace()
    {
        Debug.Log(InventorySpace.Count);
        if (InventorySpace.Count >= InventoryMaxSlots)
            return false;
        return true;
    }
    private void OnValidate()
    {
        if (HasFreeSpace())
        {
            Debug.Log("has free space");
        }
        else
        {
            Debug.Log("No free");
            RemoveLastItem();
        }
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

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        InventorySlot other = (InventorySlot)obj;
        return item != null && other.item != null && item.id == other.item.id;
    }

    public override int GetHashCode()
    {
        return item != null ? item.id.GetHashCode() : 0;
    }
}
