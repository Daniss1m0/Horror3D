using UnityEngine;

public class RequierInteractionQuest : Quest, IInteractable
{
    public override void Interact()
    {
        if (IsActive && !IsComplete)
        {
            CompleteQuest();
        }
        else
        {
            Debug.Log($"Interacted with {gameObject.name}, but quest is not active.");
        }
    }
}
