using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WakeUpFade : MonoBehaviour
{
    public Image fadePanel;

    void Start()
    {
        fadePanel.color = new Color(0, 0, 0, 1);
        StartCoroutine(FadeFromBlack());
    }

    IEnumerator FadeFromBlack()
    {
        float duration = 10f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / duration);
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadePanel.color = new Color(0, 0, 0, 0);
    }
}
