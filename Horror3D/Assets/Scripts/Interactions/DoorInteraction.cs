using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    public Animator door;
    public GameObject openText;

    public AudioSource doorSound;

    public void Interact()
    {
        DoorAction();
    }

    void DoorOpens()
    {
        Debug.Log("It Opens");
        door.SetBool("Open", true);
        door.SetBool("Closed", false);
        doorSound.Play();
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
}
