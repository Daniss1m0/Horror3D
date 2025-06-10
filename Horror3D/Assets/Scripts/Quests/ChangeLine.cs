using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeLine : MonoBehaviour
{
    public TMP_Text Line;
    public TMP_Text Person;
    public List<string> NextLine;
    public List<string> NextPerson;

    private int counter = 0;

    public void ChangeText()
    {
        if (Line != null)
        {
            Line.text = NextLine[counter];
            Person.text = NextPerson[counter];
            counter++;
        }
    }
}
