using UnityEngine;

public class HidingPlace : MonoBehaviour
{
    public GameObject player;
    private bool isPlayerHidden = false;
    private Renderer playerRenderer;
    private EnemyAI enemyAI;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            playerRenderer = player.GetComponent<Renderer>();

        enemyAI = FindObjectOfType<EnemyAI>();
    }

    void OnMouseDown()
    {
        if (player == null || playerRenderer == null || enemyAI == null)
        {
            Debug.LogWarning("HideSpot: Missing references!");
            return;
        }

        if (!isPlayerHidden)
            HidePlayer();
        else
            UnhidePlayer();
    }

    void HidePlayer()
    {
        isPlayerHidden = true;
        playerRenderer.enabled = false;
        // Вместо отключения коллайдера:
        player.transform.position = transform.position + new Vector3(0, 0.5f, 0); // Переместить внутрь объекта
        if (enemyAI.hideText) enemyAI.hideText.SetActive(false);
        if (enemyAI.stopHideText) enemyAI.stopHideText.SetActive(true);
        enemyAI.detectionDistance = 0;
    }

    void UnhidePlayer()
    {
        isPlayerHidden = false;
        playerRenderer.enabled = true;
        if (enemyAI.hideText) enemyAI.hideText.SetActive(true);
        if (enemyAI.stopHideText) enemyAI.stopHideText.SetActive(false);
        enemyAI.detectionDistance = 10f; // или другое значение
    }
}
