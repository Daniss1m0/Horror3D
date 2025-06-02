using System.Collections;
using UnityEngine;

public class FlashlightUsecase : MonoBehaviour
{
    [SerializeField] private Inventory inventoryToCheck;
    [SerializeField] private ItemBaseClass FlashLight;
    [SerializeField] private Light left;
    [SerializeField] private Light right;

    public Light spotlight;
    public float maxBattery = 100f;
    public float drainPower = 1.0f;
    public float currentBattery = 100f;

    private Coroutine drainRoutine;

    private void Start()
    {
        spotlight.enabled = false;
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
                    left.enabled = !left.enabled;
                }
                else
                {
                    right.enabled = !left.enabled;
                }
            }
        }
        else
        {
            DisableFlashlight();
            left.enabled = false;
            right.enabled = false;
        }

        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
    }

    void ToggleFlashlight()
    {
        if (spotlight.enabled)
        {
            DisableFlashlight();
        }
        else if (currentBattery > 0f)
        {
            spotlight.enabled = true;
            drainRoutine = StartCoroutine(DrainBattery());
        }
    }

    void DisableFlashlight()
    {
        spotlight.enabled = false;
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
