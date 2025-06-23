using UnityEngine;

public class RequierNotDestroyItemQuest : Quest
{
    [SerializeField] Inventory inventoryToCheck;
    [SerializeField] ItemBaseClass itemToSearchFor;
    public override void Interact()
    {
        if (IsActive && inventoryToCheck.CheckForItem(itemToSearchFor))
        {
            CompleteQuest();
        }
        else
        {
            Debug.Log($"Interacted, but not complete! Quest is active:{IsActive}");
        }
    }
}
