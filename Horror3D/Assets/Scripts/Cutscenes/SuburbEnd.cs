using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI dialogText;
    public GameObject dialogPanel;
    public Image fadePanel;
    public TMPro.TextMeshProUGUI timeSkipText;

    public GameObject taxi;
    public Transform taxiStopPoint;
    public float taxiSpeed = 2f;

    [System.Serializable]
    public class DialogueLine
    {
        public string speaker;
        [TextArea] public string line;
    }

    public DialogueLine[] lines;

    private int index = 0;
    private bool dialogStarted = false;

    void Start()
    {
        Debug.Log("Start sceny");

        fadePanel.color = new Color(0, 0, 0, 0);
        timeSkipText.gameObject.SetActive(false);
        dialogPanel.SetActive(false);

        if (taxi == null || taxiStopPoint == null)
        {
            Debug.LogError("Taks�wka lub punkt zatrzymania nie s� przypisane!");
        }

        if (lines == null || lines.Length == 0)
        {
            Debug.LogWarning("Brak dialog�w w tablicy 'lines'");
        }

        StartCoroutine(MoveTaxiAndStartDialogue());
    }

    IEnumerator MoveTaxiAndStartDialogue()
    {
        Debug.Log("Rozpoczynam ruch taks�wki");

        while (Vector3.Distance(taxi.transform.position, taxiStopPoint.position) > 0.05f)
        {
            taxi.transform.position = Vector3.MoveTowards(
                taxi.transform.position,
                taxiStopPoint.position,
                taxiSpeed * Time.deltaTime
            );
            yield return null;
        }

        Debug.Log("Taks�wka dojecha�a");

        yield return new WaitForSeconds(0.5f);

        dialogPanel.SetActive(true);
        ShowLine();
        dialogStarted = true;

        Debug.Log("Dialog rozpocz�ty");
    }

    void Update()
    {
        if (!dialogStarted) return;

        Debug.Log("Update dzia�a � nas�uchuj� spacji");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Wci�ni�to SPACJ�");

            index++;
            if (index < lines.Length)
            {
                ShowLine();
            }
            else
            {
                Debug.Log("Dialog zako�czony � zaczynam zako�czenie cutscenki");
                StartCoroutine(EndCutscene());
            }
        }
    }

    void ShowLine()
    {
        if (index >= lines.Length)
        {
            Debug.LogWarning("Indeks poza zakresem dialog�w");
            return;
        }

        Debug.Log($"Pokazuj� lini� {index + 1}/{lines.Length}: {lines[index].speaker} m�wi: \"{lines[index].line}\"");

        nameText.text = lines[index].speaker;
        dialogText.text = lines[index].line;
    }

    IEnumerator EndCutscene()
    {
        dialogPanel.SetActive(false);
        yield return StartCoroutine(FadeToBlack());
        timeSkipText.gameObject.SetActive(true);
        Debug.Log("Wy�wietlam '10 lat p�niej'");
    }

    IEnumerator FadeToBlack()
    {
        Debug.Log("Rozpoczynam fade do czerni");
        float duration = 2f;
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            fadePanel.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t / duration));
            yield return null;
        }
        Debug.Log("Fade zako�czony");
    }
}
