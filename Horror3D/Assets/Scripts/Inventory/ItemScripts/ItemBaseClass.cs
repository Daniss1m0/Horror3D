using NUnit.Framework.Interfaces;
using UnityEngine;

public enum ItemType
{
    Resource,
    QuestItem,
    Default
}
public abstract class ItemBaseClass : ScriptableObject
{
    public int id;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;

    [Header("ItemPrefab")]
    public GameObject prefab;
}
