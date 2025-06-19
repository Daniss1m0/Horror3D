using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class ChangeLine : MonoBehaviour
{
    public TMP_Text Line;
    public TMP_Text Person;
    public List<string> NextLine;
    public List<string> NextPerson;

    public PlayableDirector director;

    private int Dcounter = 0;
    private int Bcounter = 0;
    private bool hasStartedCutscene = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeText();
            Bcounter++;

            if (Bcounter >= 2 && !hasStartedCutscene)
            {
                hasStartedCutscene = true;
                StartCoroutine(PlayCutsceneAfterDelay(2f));
            }
        }
    }

    public void ChangeText()
    {
        if (Dcounter < NextLine.Count)
        {
            Line.text = NextLine[Dcounter];
            Person.text = NextPerson[Dcounter];
            Dcounter++;
        }
    }

    public void StopCutscene()
    {
        director.Pause();
    }

    private IEnumerator PlayCutsceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        director.Play();
    }
}
