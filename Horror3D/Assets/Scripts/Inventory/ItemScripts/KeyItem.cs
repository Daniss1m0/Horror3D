using UnityEngine;

[CreateAssetMenu(fileName = "New Key", menuName = "Scriptable Objects/Items/KeyItem")]
public class KeyItem : ItemBaseClass
{
    public void Awake()
    {
        this.type = ItemType.QuestItem;
    }

}
