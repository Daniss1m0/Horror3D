using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotationSpeed = 10f;

    private Vector3? targetPosition = null;

    void Update()
    {
        if (targetPosition.HasValue)
        {
            MoveToTarget();
        }
    }

    public void Move(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
    }

    private void MoveToTarget()
    {
        Vector3 direction = targetPosition.Value - transform.position;
        direction.y = 0;

        if (direction.magnitude < 0.1f)
        {
            targetPosition = null;
            return;
        }

        // Ruch w kierunku celu
        transform.position = Vector3.MoveTowards(transform.position, targetPosition.Value, moveSpeed * Time.deltaTime);

        // Obrót w kierunku celu
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
