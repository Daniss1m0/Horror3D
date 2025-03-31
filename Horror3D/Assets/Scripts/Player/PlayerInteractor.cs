using UnityEngine;

interface IInteractable
{
    public void Interact();
}

interface IPickable
{
    public void PickUp(Inventory inventory);
}

public class PlayerInteractor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractionRange;
    public Inventory inventory;
    void Start()
    {
        
    }
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
                if (hitInfo.collider.gameObject.TryGetComponent(out IPickable PickableObject))
                {
                    PickableObject.PickUp(inventory); ;
                    Destroy(hitInfo.collider.gameObject);
                }
            }
        }
    }
}
