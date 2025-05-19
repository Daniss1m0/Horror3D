using UnityEngine;

public class DoorOpenInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] Animator doorAnim;

    public void Interact()
    {
        if (doorAnim.GetBool("Opened"))
        {
            doorAnim.SetBool("Opened", false);
        }
        else
        {
            doorAnim.SetBool("Opened", true);
        }
    }
}
