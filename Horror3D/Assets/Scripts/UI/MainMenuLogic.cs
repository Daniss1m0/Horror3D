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
    private GameObject loading;

    public AudioSource buttonSound;

    public Slider volumeSlider;
    public Toggle fullscreenToggle;
    public TMP_Dropdown qualityDropdown;
    public AudioSource masterAudioSource;

    void Start()
    {
        mainMenu = GameObject.Find("MainMenuCanvas");
        optionsMenu = GameObject.Find("OptionsCanvas");
        extrasMenu = GameObject.Find("ExtrasCanvas");
        loading = GameObject.Find("LoadingCanvas");

        mainMenu.GetComponent<Canvas>().enabled = true;
        optionsMenu.GetComponent<Canvas>().enabled = false;
        extrasMenu.GetComponent<Canvas>().enabled = false;
        loading.GetComponent<Canvas>().enabled = false;

        InitVolume();
        InitFullscreen();
        InitQuality();
    }

    public void StartButton()
    {
        Debug.Log("Start");
        loading.GetComponent<Canvas>().enabled = true;
        mainMenu.GetComponent<Canvas>().enabled = false;
        buttonSound.Play();
        SceneManager.LoadScene("SuburbScene");
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

    void Update()
    {

    }
}
