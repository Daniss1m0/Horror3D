using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CutsceneController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject bottomPanel;
    public TMP_Text bottomText;
    public GameObject fullScreenPanel;
    public TMP_Text endingText;
    public Image whiteFadePanel;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip narratorClip;

    [Header("Settings")]
    public float bottomTextDuration = 5f;
    public float endingScrollDuration = 10f;
    public float typeSpeed = 0.03f;
    public float whiteFadeDuration = 2f;

    private Vector2 endingStartPos;
    private Vector2 endingEndPos;

    private string endCredits =
@"

EXPERIMENT: 42-Ω COMPLETED
ENVIRONMENT: SIM 04-BUNKER/ESCAPE

––––––––––––––––––––––––––––––––––––––––

Simulation Developer: YOU  
Environmental Design: Procedural Generator v2.4  
Voice Line System: S.V.O. AI Synth  
Memory Repressor: Version 7 [OBSOLETE]  
Subject: ID#42 – Status: RELEASED  
Recovery Probability: UNKNOWN

––––––––––––––––––––––––––––––––––––––––

Loading next environment...

████";

    private void Start()
    {
        bottomPanel.SetActive(false);
        fullScreenPanel.SetActive(false);
        whiteFadePanel.gameObject.SetActive(true);

        endingStartPos = endingText.rectTransform.anchoredPosition;
        endingEndPos = new Vector2(endingStartPos.x, 1000f);

        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        yield return StartCoroutine(FadeFromWhite());

        bottomPanel.SetActive(true);

        yield return ShowBottomText("Is this… the end?");
        yield return ShowBottomText("No... it’s just the beginning.");
        yield return ShowBottomText("I don’t know what I’ll find out there...");
        yield return ShowBottomText("but I’ll never let myself be locked up again.");

        if (audioSource != null && narratorClip != null)
        {
            bottomText.text = "Experiment completed. Subject No. 42 survived the full simulation cycle.\nStarting transfer procedure to the next testing environment.";
            audioSource.clip = narratorClip;
            audioSource.Play();
            yield return new WaitForSeconds(narratorClip.length);
        }
        else
        {
            yield return ShowBottomText("Experiment completed. Subject No. 42 survived the full simulation cycle.");
            yield return ShowBottomText("Starting transfer procedure to the next testing environment.");
        }

        bottomPanel.SetActive(false);
        fullScreenPanel.SetActive(true);

        yield return ScrollEndingText(endCredits);

        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator ShowBottomText(string text)
    {
        bottomText.text = "";
        foreach (char c in text)
        {
            bottomText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
        yield return new WaitForSeconds(bottomTextDuration);
    }

    private IEnumerator ScrollEndingText(string text)
    {
        endingText.text = text;
        endingText.rectTransform.anchoredPosition = endingStartPos;

        float elapsed = 0f;
        while (elapsed < endingScrollDuration)
        {
            float t = elapsed / endingScrollDuration;
            endingText.rectTransform.anchoredPosition = Vector2.Lerp(endingStartPos, endingEndPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        endingText.rectTransform.anchoredPosition = endingEndPos;
    }

    private IEnumerator FadeFromWhite()
    {
        if (whiteFadePanel == null) yield break;

        Color color = whiteFadePanel.color;
        color.a = 1f;
        whiteFadePanel.color = color;
        whiteFadePanel.gameObject.SetActive(true);

        float elapsed = 0f;

        while (elapsed < whiteFadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / whiteFadeDuration);
            whiteFadePanel.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        whiteFadePanel.color = new Color(color.r, color.g, color.b, 0f);
        whiteFadePanel.gameObject.SetActive(false);
    }
}
