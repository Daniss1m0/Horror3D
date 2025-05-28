using UnityEngine;

public class HidingPlace : MonoBehaviour
{
    public Transform player;
    public GameObject hidingCameraObject;
    public Collider hidingTrigger;
    public GameObject hideText; // <-- UI текст "Нажмите E, чтобы спрятаться"

    private Camera mainCamera;
    private Camera hidingCamera;

    private bool isPlayerNear = false;
    private bool isHiding = false;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
            Debug.LogError("MainCamera не найдена!");

        if (hidingCameraObject != null)
        {
            hidingCamera = hidingCameraObject.GetComponent<Camera>();
            if (hidingCamera == null)
                Debug.LogError("На hidingCameraObject нет компонента Camera.");
            hidingCamera.enabled = false;
        }

        if (hideText != null)
            hideText.SetActive(false); // Сначала скрыть
    }

    void Update()
    {
        if (hidingTrigger != null)
        {
            bool wasNear = isPlayerNear;
            isPlayerNear = hidingTrigger.bounds.Contains(player.position);

            // Включить/выключить текст, если статус изменился
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

        // Прятаться = убираем текст
        if (hideText != null)
            hideText.SetActive(!isHiding && isPlayerNear);
    }
}
