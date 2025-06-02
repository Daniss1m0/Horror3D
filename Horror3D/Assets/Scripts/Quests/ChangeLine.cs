using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeLine : MonoBehaviour
{
    public TMP_Text textComponent;
    public List<string> newText;

    private int counter = 0;

    public void ChangeText()
    {
        if (textComponent != null)
        {
            textComponent.text = newText[counter];
            counter++;
        }
    }
}
