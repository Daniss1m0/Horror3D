using UnityEngine;
using System.Collections;

public class GameplayManager : MonoBehaviour
{
    public NPCMovement npc;
    public Transform[] waypoints;
    public float waitTimeAtPoint = 2f;

    private int currentWaypointIndex = 0;

    void Start()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("Brak waypointów przypisanych do GameplayManager.");
            return;
        }

        StartCoroutine(MoveLoop());
    }

    IEnumerator MoveLoop()
    {
        while (true)
        {
            Vector3 target = waypoints[currentWaypointIndex].position;
            npc.Move(target);

            yield return new WaitUntil(() => npc.HasReachedTarget());

            yield return new WaitForSeconds(waitTimeAtPoint);

            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
