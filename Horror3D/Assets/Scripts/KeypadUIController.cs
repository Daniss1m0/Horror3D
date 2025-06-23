using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class KeypadController : MonoBehaviour
{
    public GameObject keyboardCanvas;
    public TMP_Text inputText;
    public Transform player;
    public float interactionDistance = 3f;

    private string currentInput = "";
    private string correctCode = "2142";
    private bool isPlayerNearby = false;
    private bool isCanvasOpen = false;

    void Start()
    {
        keyboardCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        isPlayerNearby = distance <= interactionDistance;

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (!isCanvasOpen)
                OpenCanvas();
            else
                CloseCanvas();
        }
    }

    public void AddDigit(string digit)
    {
        if (currentInput.Length >= 4) return;

        currentInput += digit;
        inputText.text = currentInput;
    }

    public void ClearInput()
    {
        currentInput = "";
        inputText.text = "";
    }

    public void EnterPressed()
    {
        if (currentInput == correctCode)
        {
            SceneManager.LoadScene("EndCutscene");
        }
        else
        {
            ClearInput();
        }
    }

    void OpenCanvas()
    {
        keyboardCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isCanvasOpen = true;
    }

    void CloseCanvas()
    {
        keyboardCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCanvasOpen = false;
        ClearInput();
    }
}
