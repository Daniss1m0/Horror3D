using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BatteryUI : MonoBehaviour
{
    [SerializeField] private FlashlightUsecase flashlight;
    [SerializeField] private Slider batterySlider;
    [SerializeField] private TextMeshProUGUI batteryText;

    private void Start()
    {
        if (flashlight != null && batterySlider != null)
        {
            batterySlider.maxValue = flashlight.maxBattery;
            batterySlider.value = flashlight.currentBattery;
        }
    }

    private void Update()
    {
        if (flashlight != null && batterySlider != null)
        {
            batterySlider.value = flashlight.currentBattery;

            if (batteryText != null)
            {
                batteryText.text = $"{Mathf.CeilToInt(flashlight.currentBattery)} / {flashlight.maxBattery}";
            }
        }
    }
}
