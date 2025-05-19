using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    public Animator door;
    public GameObject openText;
    public Inventory inventory;
    public ItemBaseClass itemToCheckFor;
    private bool keyUsed = false;

    public AudioSource doorSound;

    public void Interact()
    {
        CheckKey();
        DoorAction();
    }

    void DoorOpens()
    {
        if (keyUsed)
        {
            Debug.Log("It Opens");
            door.SetBool("Open", true);
            door.SetBool("Closed", false);
            doorSound?.Play();
        }
        else
        {
            Debug.Log("Find Key");
        }
    }

    void DoorCloses()
    {
        Debug.Log("It Closes");
        door.SetBool("Open", false);
        door.SetBool("Closed", true);
    }

    void DoorAction()
    {
        if (door.GetBool("Open"))
        {
            DoorCloses();
        }
        else
        {
            DoorOpens();
        }
    }

    void CheckKey()
    {
        if (inventory.CheckForItem(itemToCheckFor))
        {
            keyUsed = true;
        }
    }
}
