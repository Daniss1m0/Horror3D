using UnityEngine;
using System;
using TMPro;

public abstract class Quest : MonoBehaviour, IInteractable
{
    public bool IsActive { get; protected set; } = false;
    public bool IsComplete { get; protected set; } = false;

    public event Action<Quest> OnQuestCompleted;

    public string text;
    public virtual void Activate()
    {
        IsActive = true;
        IsComplete = false;
        Debug.Log($"{gameObject.name} quest activated.");
    }

    public abstract void Interact();

    protected void CompleteQuest()
    {
        IsActive = false;
        IsComplete = true;
        OnQuestCompleted?.Invoke(this);
        Debug.Log($"{gameObject.name} quest completed.");
    }
}
