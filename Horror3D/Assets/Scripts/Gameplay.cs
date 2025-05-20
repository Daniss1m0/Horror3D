using UnityEngine;
using System.Collections;

public class GameplayManager : MonoBehaviour
{
    public NPCMovement npc;
    public Transform[] waypoints;
    public float waitTimeAtPoint = 2f;
    public AudioClip footstepSound;

    private int currentWaypointIndex = 0;
    private Coroutine footstepCoroutine;
    private AudioSource audioSource;

    void Start()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("Brak waypointów przypisanych do GameplayManager.");
            return;
        }

        audioSource = npc.GetComponent<AudioSource>();
        StartCoroutine(MoveLoop());
    }

    IEnumerator MoveLoop()
    {
        while (true)
        {
            Vector3 target = waypoints[currentWaypointIndex].position;
            npc.Move(target);

            if (footstepCoroutine == null)
                footstepCoroutine = StartCoroutine(PlayFootsteps());

            yield return new WaitUntil(() => npc.HasReachedTarget());

            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
                footstepCoroutine = null;
            }

            yield return new WaitForSeconds(waitTimeAtPoint);
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    IEnumerator PlayFootsteps()
    {
        while (true)
        {
            if (footstepSound != null && audioSource != null && npc.isActiveAndEnabled)
            {
                audioSource.PlayOneShot(footstepSound);
                Debug.Log("playung sound");
            }
            
            yield return new WaitForSeconds(1.3f);
        }
    }
}
