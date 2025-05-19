using UnityEngine;

public class RequierItemQuest : Quest
{
    [SerializeField] Inventory inventoryToCheck;
    [SerializeField] ItemBaseClass itemToSearchFor;
    public override void Interact()
    {
        if (IsActive && inventoryToCheck.CheckForItem(itemToSearchFor))
        {
            CompleteQuest();
            inventoryToCheck.RemoveItem(itemToSearchFor);
        }
        else
        {
            Debug.Log($"Interacted, but not complete! Quest is active:{IsActive}");
        }
    }
}
