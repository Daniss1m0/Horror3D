using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public NPCMovement npc;

    void Start()
    {
        npc.Move(new Vector3(10f, 0f, 5f));
    }
}
