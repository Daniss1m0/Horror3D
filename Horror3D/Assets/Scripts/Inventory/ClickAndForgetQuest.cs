using UnityEngine;

public class ClickAndForgetQuest : MonoBehaviour, IInteractable
{
    private bool isQuestActive { get; set; } = true;
    private bool isQuestComplete { get; set; } = false;

    public void Interact()
    {
        if (isQuestActive)
        {
            isQuestActive = false;
            isQuestComplete = true;
            //Additional logic
        }
        else
        {
            Debug.Log($"Interacted, but not complete!");
        }
    }
}
