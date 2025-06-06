using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
        "Я проснулся посреди ночи. Было странное чувство тревоги...",
        "Весь дом казался пустым, но что-то было не так.",
        "Я слышал шорох за дверью...",
        "Нужно было выяснить, что происходит."
    };

    private int currentLine = 0;
    private bool isShowingLore = false;

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

        isShowingLore = true;
        currentLine = 0;
        loreText.text = loreLines[currentLine];
    }

    void Update()
    {
        if (isShowingLore && Input.GetKeyDown(KeyCode.Return))
        {
            currentLine++;
            if (currentLine < loreLines.Length)
            {
                loreText.text = loreLines[currentLine];
            }
            else
            {
                StartCoroutine(LoadNextScene());
            }
        }
    }

    IEnumerator LoadNextScene()
    {
        isShowingLore = false;
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
