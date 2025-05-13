using UnityEngine;

public class RequierItemQuest : MonoBehaviour, IInteractable
{
    [SerializeField] Inventory inventoryToCheck;
    [SerializeField] ItemBaseClass itemToSearchFor;

    private bool isQuestActive { get; set; } = true;
    private bool isQuestComplete { get; set; } = false;

    public void Interact()
    {
        if (isQuestActive && inventoryToCheck.CheckForItem(itemToSearchFor))
        {
            isQuestActive = false;
            isQuestComplete = true;
            inventoryToCheck.RemoveItem(itemToSearchFor);
        }
        else
        {
            Debug.Log($"Interacted, but not complete! {inventoryToCheck.CheckForItem(itemToSearchFor)}");
        }
    }
}
