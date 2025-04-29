using UnityEngine;
using System.Collections;

public class GameplayManager : MonoBehaviour
{
    public NPCMovement npc;

    private Vector3 pointA = new Vector3(-46.2f, 1.2f, -61.6f);
    private Vector3 pointB = new Vector3(-31.15f, 1.2f, -61.6f);

    private bool goingToA = false;

    void Start()
    {
        StartCoroutine(MoveLoop());
    }

    IEnumerator MoveLoop()
    {
        while (true)
        {
            Vector3 target = goingToA ? pointA : pointB;
            npc.Move(target);

            yield return new WaitUntil(() => npc.HasReachedTarget());

            yield return new WaitForSeconds(2f);

            goingToA = !goingToA;
        }
    }
}
