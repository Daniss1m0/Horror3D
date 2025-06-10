using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class WaitForSpacePass : MonoBehaviour
{
    public PlayableDirector director;
    private bool waitingForInput = false;

    void Update()
    {
        if (waitingForInput && Input.GetKeyDown(KeyCode.Space))
        {
            waitingForInput = false;
            director.Resume();
        }
    }

    public void WaitForSpace()
    {
        director.Pause();
        waitingForInput = true;
    }
}
