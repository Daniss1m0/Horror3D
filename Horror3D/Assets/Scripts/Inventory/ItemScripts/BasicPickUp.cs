using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Pick-up", menuName = "Scriptable Objects/Items/BasicPickUp")]
public class BasicPickUp : ItemBaseClass
{
    public void Awake()
    {
        this.type = ItemType.QuestItem;
    }
}
