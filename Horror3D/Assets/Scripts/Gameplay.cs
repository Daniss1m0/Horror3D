using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public NPCMovement npc;
    public Transform[] waypoints;
    public float waitTimeAtPoint = 2f;

    void Start()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("Brak waypointów przypisanych do GameplayManager.");
            return;
        }

        npc.WalkPathOnce(waypoints, waitTimeAtPoint);
    }

}
