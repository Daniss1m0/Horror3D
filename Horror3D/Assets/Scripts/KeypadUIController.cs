using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class KeypadUIController : MonoBehaviour
{
    public GameObject keyboardCanvas;
    public TMP_Text inputText;

    private string currentInput = "";
    private string correctCode = "2142";

    void Start()
    {
        keyboardCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!keyboardCanvas.activeSelf && PlayerLookingAtKeypad())
            {
                OpenKeypad();
            }
            else if (keyboardCanvas.activeSelf)
            {
                ExitKeypad();
            }
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

    public void OpenKeypad()
    {
        keyboardCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ExitKeypad()
    {
        keyboardCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ClearInput();
    }

    private bool PlayerLookingAtKeypad()
    {
        return true;
    }
}
