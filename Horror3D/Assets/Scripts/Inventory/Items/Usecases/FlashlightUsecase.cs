using System.Collections;
using UnityEngine;

public class FlashlightUsecase : MonoBehaviour
{
    [SerializeField] private Inventory inventoryToCheck;
    [SerializeField] private ItemBaseClass FlashLight;
    [SerializeField] private Light left;
    [SerializeField] private Light right;
    [SerializeField] private GameObject chargeSliderUI;

    public Light spotlight;
    public float maxBattery = 100f;
    public float drainPower = 1.0f;
    public float currentBattery = 100f;

    private Coroutine drainRoutine;

    private void Start()
    {
        if (spotlight != null)
            spotlight.enabled = false;

        if (chargeSliderUI != null)
            chargeSliderUI.SetActive(false);

        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
    }

    void Update()
    {
        if (inventoryToCheck.CheckForItem(FlashLight))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ToggleFlashlight();

                if (inventoryToCheck.GetItemIndex(FlashLight) == 0)
                {
                    if (left != null)
                        left.enabled = !left.enabled;
                }
                else
                {
                    if (right != null)
                        right.enabled = !left.enabled;
                }
            }
        }
        else
        {
            DisableFlashlight();

            if (left != null)
                left.enabled = false;

            if (right != null)
                right.enabled = false;
        }

        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
    }

    void ToggleFlashlight()
    {
        if (spotlight == null)
            return;

        if (spotlight.enabled)
        {
            DisableFlashlight();
        }
        else if (currentBattery > 0f)
        {
            spotlight.enabled = true;

            if (chargeSliderUI != null)
                chargeSliderUI.SetActive(true);

            drainRoutine = StartCoroutine(DrainBattery());
        }
    }

    void DisableFlashlight()
    {
        if (spotlight != null)
            spotlight.enabled = false;

        if (chargeSliderUI != null)
            chargeSliderUI.SetActive(false);

        if (drainRoutine != null)
        {
            StopCoroutine(drainRoutine);
            drainRoutine = null;
        }
    }

    private IEnumerator DrainBattery()
    {
        while (currentBattery > 0f)
        {
            yield return new WaitForSeconds(1f);
            currentBattery -= drainPower;

            if (currentBattery <= 0f)
            {
                currentBattery = 0f;
                DisableFlashlight();
            }
        }
    }
}
