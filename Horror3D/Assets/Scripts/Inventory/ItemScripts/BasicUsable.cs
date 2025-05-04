using UnityEngine;

[CreateAssetMenu(fileName = "Basic Usable", menuName = "Scriptable Objects/Items/BasicUsable")]
public class BasicUsable : ItemBaseClass
{
    public void Awake()
    {
        this.type = ItemType.Usable;
    }
}
