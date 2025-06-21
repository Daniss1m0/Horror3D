using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MainMenuLogic : MonoBehaviour
{
    private GameObject mainMenu;
    private GameObject optionsMenu;
    private GameObject extrasMenu;
    private GameObject loreCanvas;

    public AudioSource buttonSound;

    public Slider volumeSlider;
    public Toggle fullscreenToggle;
    public TMP_Dropdown qualityDropdown;
    public AudioSource masterAudioSource;

    public TextMeshProUGUI loreText;
    public string nextSceneName = "SuburbScene";

    private string[] loreLines = new string[]
    {
        "Nie wiem, kiedy wszystko się posypało.",
        "Może wtedy, gdy przestałem wychodzić ze swojego pokoju.",
        "Może wtedy, gdy mama przestała mówić ‘dobranoc’.",
        "Czasem zastanawiam się, czy to już się wydarzyło…",
        "…czy dopiero się zaczyna."
    };

    private int currentLine = 0;
    private string fullLoreText = "";
    private bool isShowingLore = false;

    private float loreDelay = 0.2f;
    private float lastInputTime;

    void Start()
    {
        mainMenu = GameObject.Find("MainMenuCanvas");
        optionsMenu = GameObject.Find("OptionsCanvas");
        extrasMenu = GameObject.Find("ExtrasCanvas");
        loreCanvas = GameObject.Find("LoreCanvas");

        mainMenu.GetComponent<Canvas>().enabled = true;
        optionsMenu.GetComponent<Canvas>().enabled = false;
        extrasMenu.GetComponent<Canvas>().enabled = false;
        loreCanvas.GetComponent<Canvas>().enabled = false;

        InitVolume();
        InitFullscreen();
        InitQuality();
    }

    public void StartButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = false;
        loreCanvas.GetComponent<Canvas>().enabled = true;

        EventSystem.current.SetSelectedGameObject(null);

        currentLine = 0;
        fullLoreText = "";
        loreText.text = "";
        isShowingLore = true;
        lastInputTime = Time.time;

        ShowNextLine();
    }

    void Update()
    {
        if (!isShowingLore) return;

        bool inputNotBlockedByUI = EventSystem.current.currentSelectedGameObject == null;

        if (inputNotBlockedByUI &&
            Time.time - lastInputTime > loreDelay &&
            Input.GetKeyDown(KeyCode.Space))
        {
            lastInputTime = Time.time;
            currentLine++;

            if (currentLine < loreLines.Length)
            {
                ShowNextLine();
            }
            else
            {
                isShowingLore = false;
                StartCoroutine(LoadNextScene());
            }
        }
    }

    void ShowNextLine()
    {
        fullLoreText += loreLines[currentLine] + "\n";
        loreText.text = fullLoreText;
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextSceneName);
    }

    public void OptionsButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = false;
        optionsMenu.GetComponent<Canvas>().enabled = true;
    }

    public void ExtrasButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = false;
        extrasMenu.GetComponent<Canvas>().enabled = true;
    }

    public void ExitGameButton()
    {
        buttonSound.Play();
        Application.Quit();
        Debug.Log("App Has Exited");
    }

    public void ReturnToMainMenuButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = true;
        optionsMenu.GetComponent<Canvas>().enabled = false;
        extrasMenu.GetComponent<Canvas>().enabled = false;
    }

    void InitVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume;
        if (masterAudioSource != null)
            masterAudioSource.volume = savedVolume;

        volumeSlider.onValueChanged.AddListener((value) =>
        {
            if (masterAudioSource != null)
                masterAudioSource.volume = value;
            PlayerPrefs.SetFloat("Volume", value);
        });
    }

    void InitFullscreen()
    {
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;

        fullscreenToggle.onValueChanged.AddListener((isOn) =>
        {
            Screen.fullScreen = isOn;
            PlayerPrefs.SetInt("Fullscreen", isOn ? 1 : 0);
        });
    }

    void InitQuality()
    {
        int savedQuality = PlayerPrefs.GetInt("Quality", 2);
        qualityDropdown.value = savedQuality;
        QualitySettings.SetQualityLevel(savedQuality);

        qualityDropdown.onValueChanged.AddListener((level) =>
        {
            QualitySettings.SetQualityLevel(level);
            PlayerPrefs.SetInt("Quality", level);
        });
    }
}
