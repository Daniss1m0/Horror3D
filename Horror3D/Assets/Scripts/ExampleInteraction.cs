using UnityEngine;

public class ExampleInteraction : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Destroy(gameObject);
    }
}
