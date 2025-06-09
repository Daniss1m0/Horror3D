using UnityEngine;
using UnityEngine.Playables;

public class TriggerCutsceneWithQuest : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public Quest triggerQuest;
    public GameObject ObjectToActivate;
    void Update()
    {
        if (triggerQuest.IsComplete)
        {
            ObjectToActivate.SetActive(true);
            if (playableDirector.enabled)
            {
                playableDirector.Play();
                enabled = false;
            }
            else
            {
                playableDirector.enabled = true;
                playableDirector.Play();
                enabled = false;
            }
            
        }
    }
}
