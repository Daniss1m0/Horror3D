using UnityEngine;

public class HidingPlace : MonoBehaviour
{
    public Transform player;
    public GameObject hidingCameraObject;
    public Collider hidingTrigger;
    public GameObject hideText;

    private Camera mainCamera;
    private Camera hidingCamera;

    private bool isPlayerNear = false;
    private bool isHiding = false;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
            Debug.LogError("MainCamera not found!");

        if (hidingCameraObject != null)
        {
            hidingCamera = hidingCameraObject.GetComponent<Camera>();
            if (hidingCamera == null)
                Debug.LogError("MainCamera not found!");
            hidingCamera.enabled = false;
        }

        if (hideText != null)
            hideText.SetActive(false);
    }

    void Update()
    {
        if (hidingTrigger != null)
        {
            bool wasNear = isPlayerNear;
            isPlayerNear = hidingTrigger.bounds.Contains(player.position);

            if (hideText != null && isPlayerNear != wasNear)
                hideText.SetActive(isPlayerNear);
        }

        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            ToggleHide();
        }
    }

    void ToggleHide()
    {
        isHiding = !isHiding;

        var controller = player.GetComponent<PlayerController>();
        if (controller != null)
            controller.enabled = !isHiding;

        if (mainCamera != null && hidingCamera != null)
        {
            mainCamera.enabled = !isHiding;
            hidingCamera.enabled = isHiding;
        }

        EnemyAI enemy = FindObjectOfType<EnemyAI>();
        if (enemy != null)
            enemy.SetPlayerHidden(isHiding);

        if (hideText != null)
            hideText.SetActive(!isHiding && isPlayerNear);
    }
}
