using UnityEngine;

public class BatteryInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private float chargeAmount = 25f;
    [SerializeField] private FlashlightUsecase chargeTarget;
    public void Interact()
    {
        if (chargeTarget != null)
        {
            chargeTarget.currentBattery = Mathf.Min(
                chargeTarget.currentBattery + chargeAmount,
                chargeTarget.maxBattery
            );
        }
        else
        {
            Debug.LogWarning("No charge target assigned to battery pickup.");
        }

        Destroy(gameObject);
    }
}
