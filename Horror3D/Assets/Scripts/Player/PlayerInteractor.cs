using UnityEngine;

interface IInteractable
{
    public void Interact();
}

interface IPickable
{
    public void PickUp(Inventory inventory);
}

interface IUsable
{
    public void Use();
}

public class PlayerInteractor : MonoBehaviour
{
    public Transform InteractorSource;
    public Transform dropPoint;
    public Transform LeftHand;
    public Transform RightHand;
    public Inventory inventory;
    public float InteractionRange;

    private void Awake()
    {
        AttachToHands();
    }
    void Update()
    {
        Interaction();
        Drop();
    }

    void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray InteractionRay = new Ray(InteractorSource.position, InteractorSource.forward);
            Debug.DrawRay(InteractionRay.origin, InteractionRay.direction * InteractionRange, Color.green, 10);
            if (Physics.Raycast(InteractionRay, out RaycastHit hitInfo, InteractionRange))
            {
                GameObject target = hitInfo.collider.attachedRigidbody
                    ? hitInfo.collider.attachedRigidbody.gameObject
                    : hitInfo.collider.gameObject;

                Debug.Log("Interacted with: " + target.name);

                if (target.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact();
                }

                if (target.TryGetComponent(out IPickable pickable))
                {
                    if (inventory.HasFreeSpace())
                    {
                        pickable.PickUp(inventory);
                        Destroy(target);
                    }
                    else
                        Debug.Log("No free space in inventory");
                }
            }
            AttachToHands();
        }
    }

    void Drop()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (inventory.GetItemToDrop())
            {
                GameObject DropedItem = Instantiate(inventory.GetItemToDrop(), dropPoint.position, Quaternion.identity);
                inventory.RemoveLastItem();
            }
            else
                Debug.Log("No Items to drop");
            AttachToHands();
        }
    }

    void AttachToHands()
    {
        foreach (Transform child in LeftHand)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in RightHand)
        {
            Destroy(child.gameObject);
        }

        if (inventory.GetInventorySize() >= 1)
        {
            GameObject leftItemPrefab = inventory.GetInventorySlot(0).item.prefab;
            if (leftItemPrefab != null)
            {
                GameObject leftItemInstance = Instantiate(leftItemPrefab, LeftHand);
                Rigidbody rb = leftItemInstance.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.detectCollisions = false;
                leftItemInstance.transform.localPosition = Vector3.zero;
                leftItemInstance.transform.localRotation = Quaternion.identity;
            }
        }

        if (inventory.GetInventorySize() >= 2)
        {
            GameObject rightItemPrefab = inventory.GetInventorySlot(1).item.prefab;
            if (rightItemPrefab != null)
            {
                GameObject rightItemInstance = Instantiate(rightItemPrefab, RightHand);
                Rigidbody rb = rightItemInstance.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.detectCollisions = false;
                rightItemInstance.transform.localPosition = Vector3.zero;
                rightItemInstance.transform.localRotation = Quaternion.identity;
            }
        }
    }

}