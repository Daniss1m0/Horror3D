using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class PlayerInteractor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractionRange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Interaction();
    }

    void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray InteractionRay = new Ray(InteractorSource.position, InteractorSource.forward);
            Debug.DrawRay(InteractionRay.origin, InteractionRay.direction * InteractionRange, Color.green, 10);
            if (Physics.Raycast(InteractionRay, out RaycastHit hitInfo, InteractionRange))
            {
                if(hitInfo.collider.gameObject.TryGetComponent(out IInteractable InteractableObject))
                {
                    InteractableObject.Interact();
                }
            }
        }
    }
}
