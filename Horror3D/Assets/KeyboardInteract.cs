using UnityEngine;
using UnityEngine.UI;

public class KeyboardInteract : MonoBehaviour
{
    [Header("Player & Distance")]
    public GameObject player;
    public float interactionDistance = 3f;

    [Header("UI Elements")]
    public GameObject codeCanvas;
    public Text displayText;

    [Header("Door")]
    public GameObject door;
    public string correctCode = "1234";

    private string enteredCode = "";
    private bool isPlayerNearby = false;
    private bool isCanvasOpen = false;

    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        isPlayerNearby = distance <= interactionDistance;

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleCanvas(true);
        }
    }

    public void AddDigit(string digit)
    {
        if (enteredCode.Length < 4)
        {
            enteredCode += digit;
            UpdateDisplay();
        }

        if (enteredCode.Length == 4)
        {
            Invoke(nameof(CheckCode), 0.2f);
        }
    }

    public void ClearCode()
    {
        enteredCode = "";
        UpdateDisplay();
    }


    public void TrySubmitCode()
    {
        if (enteredCode.Length == 4)
        {
            CheckCode();
        }
    }

    void CheckCode()
    {
        if (enteredCode == correctCode)
        {
            Debug.Log("✅ Правильный код. Дверь открыта!");
            OpenDoor();
            ToggleCanvas(false);
        }
        else
        {
            Debug.Log("❌ Неверный код. Попробуйте снова.");
            ClearCode();
        }
    }

    void UpdateDisplay()
    {
        if (displayText != null)
            displayText.text = enteredCode;
    }

    void ToggleCanvas(bool state)
    {
        codeCanvas.SetActive(state);
        isCanvasOpen = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;
    }

    void OpenDoor()
    {
        if (door != null)
        {
            door.SetActive(false);
        }
    }
}
