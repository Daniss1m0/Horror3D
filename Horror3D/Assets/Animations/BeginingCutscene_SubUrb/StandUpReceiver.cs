using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneSignalReceiver : MonoBehaviour
{
    public PlayableDirector director;
    public TextMeshProUGUI textMeshProUGUI;
    private bool waitingForInput = false;

    void Update()
    {
        if (waitingForInput && Input.GetKeyDown(KeyCode.Space))
        {
            textMeshProUGUI.text = "";
            waitingForInput = false;
            director.Resume();
        }
    }

    public void WaitForSpace()
    {
        director.Pause();
        textMeshProUGUI.text = "Press \"Space\" to stand up";
        waitingForInput = true;
    }
}
