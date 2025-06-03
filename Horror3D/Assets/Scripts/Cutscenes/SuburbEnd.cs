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

    public Transform mother;
    public Transform son;
    public Transform cameraTarget;

    public float cameraMoveDuration = 1f;

    public MonoBehaviour otherCameraScript;

    private Vector3 originalCameraPos;
    private Quaternion originalCameraRot;

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
        fadePanel.color = new Color(0, 0, 0, 0);
        timeSkipText.gameObject.SetActive(false);
        dialogPanel.SetActive(false);

        if (cameraTarget != null)
        {
            originalCameraPos = cameraTarget.position;
            originalCameraRot = cameraTarget.rotation;
        }

        StartCoroutine(MoveTaxiAndStartDialogue());
    }

    IEnumerator MoveTaxiAndStartDialogue()
    {
        while (Vector3.Distance(taxi.transform.position, taxiStopPoint.position) > 0.05f)
        {
            taxi.transform.position = Vector3.MoveTowards(
                taxi.transform.position,
                taxiStopPoint.position,
                taxiSpeed * Time.deltaTime
            );
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        Vector3 directionToSon = (son.position - mother.position).normalized;
        directionToSon.y = 0;

        if (directionToSon != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToSon);
            mother.rotation = lookRotation;
        }

        if (cameraTarget != null)
        {
            yield return StartCoroutine(MoveCameraAuto());
        }

        yield return new WaitForSeconds(0.5f);

        if (otherCameraScript != null)
        {
            otherCameraScript.enabled = false;
        }

        dialogPanel.SetActive(true);
        ShowLine();
        dialogStarted = true;
    }

    IEnumerator MoveCameraAuto()
    {
        Vector3 dirToMother = (mother.position - cameraTarget.position).normalized;
        Vector3 endPos = mother.position - dirToMother * 1f + Vector3.up * 1.6f;
        Quaternion endRot = Quaternion.LookRotation(mother.position + Vector3.up * 1.6f - endPos);

        Vector3 startPos = cameraTarget.position;
        Quaternion startRot = cameraTarget.rotation;

        float t = 0f;
        while (t < cameraMoveDuration)
        {
            t += Time.deltaTime;
            float progress = t / cameraMoveDuration;

            cameraTarget.position = Vector3.Lerp(startPos, endPos, progress);
            cameraTarget.rotation = Quaternion.Slerp(startRot, endRot, progress);

            yield return null;
        }

        cameraTarget.position = endPos;
        cameraTarget.rotation = endRot;
    }

    IEnumerator ReturnCameraToOriginal()
    {
        float t = 0f;
        Vector3 startPos = cameraTarget.position;
        Quaternion startRot = cameraTarget.rotation;

        while (t < cameraMoveDuration)
        {
            t += Time.deltaTime;
            float progress = t / cameraMoveDuration;

            cameraTarget.position = Vector3.Lerp(startPos, originalCameraPos, progress);
            cameraTarget.rotation = Quaternion.Slerp(startRot, originalCameraRot, progress);

            yield return null;
        }

        cameraTarget.position = originalCameraPos;
        cameraTarget.rotation = originalCameraRot;

        if (otherCameraScript != null)
        {
            otherCameraScript.enabled = true;
        }
    }

    void Update()
    {
        if (!dialogStarted) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            index++;
            if (index < lines.Length)
            {
                ShowLine();
            }
            else
            {
                StartCoroutine(EndCutscene());
            }
        }
    }

    void ShowLine()
    {
        if (index >= lines.Length) return;

        nameText.text = lines[index].speaker;
        dialogText.text = lines[index].line;
    }

    IEnumerator EndCutscene()
    {
        dialogPanel.SetActive(false);

        yield return StartCoroutine(FadeToBlack());
        yield return StartCoroutine(ReturnCameraToOriginal());

        timeSkipText.gameObject.SetActive(true);
    }

    IEnumerator FadeToBlack()
    {
        float duration = 2f;
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            fadePanel.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t / duration));
            yield return null;
        }
    }
}
