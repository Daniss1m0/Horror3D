using UnityEngine;

public class TurnOffComputerEffects : MonoBehaviour
{
    public Quest questToWatch;
    public GameObject computerScreen;
    public AudioSource computerAudio;

    private bool hasTriggered = false;

    void Update()
    {
        if (!hasTriggered && questToWatch.IsComplete)
        {
            hasTriggered = true;

            if (computerScreen != null)
                computerScreen.SetActive(false);

            if (computerAudio != null)
                computerAudio.Stop();
        }
    }
}
