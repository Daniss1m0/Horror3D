using UnityEngine;
using UnityEngine.Playables;

public class TriggerCutsceneWithQuest : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public Quest triggerQuest;
    void Update()
    {
        if (triggerQuest.IsComplete)
        {
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
