using UnityEngine;
using TMPro;

public class UIHoverHint : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float detectionRange = 3f;
    [SerializeField] private TextMeshProUGUI hintText;

    [SerializeField] private string pickableMessage = "Press E to pick up";
    [SerializeField] private string interactableMessage = "Press E to interact";

    private void Update()
    {
        UpdateHint();
    }

    void UpdateHint()
    {
        hintText.text = ""; // Clear by default

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, detectionRange))
        {
            GameObject target = hit.collider.attachedRigidbody
                ? hit.collider.attachedRigidbody.gameObject
                : hit.collider.gameObject;

            bool showHint = false;

            if (target.GetComponent<IPickable>() != null)
            {
                hintText.text = pickableMessage;
                showHint = true;
            }

            if (target.GetComponent<IInteractable>() != null)
            {
                hintText.text = interactableMessage;
                showHint = true;
            }

            if (!showHint)
                hintText.text = "";
        }
    }
}
