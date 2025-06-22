using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    public Animator door;
    public Inventory inventory;
    public ItemBaseClass itemToCheckFor;
    private bool keyUsed = false;

    //public AudioSource doorSound;

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
            door.SetBool("Opened", true);
            //doorSound?.Play();
        }
        else
        {
            Debug.Log("Find Key");
        }
    }

    void DoorCloses()
    {
        Debug.Log("It Closes");
        door.SetBool("Opened", false);
    }

    void DoorAction()
    {
        if(door != null){
            if (door.GetBool("Opened"))
                DoorCloses();
            else
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
