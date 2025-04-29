using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotationSpeed = 10f;
    public float stopThreshold = 0.1f;

    private Vector3? targetPosition = null;
    private bool isMoving = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (targetPosition.HasValue && isMoving)
            MoveToTarget();
    }

    public void Move(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
        if (!isMoving)
        {
            isMoving = true;
            animator?.SetBool("isWalking", true);
        }
    }

    private void StopMoving()
    {
        if (isMoving)
        {
            isMoving = false;
            targetPosition = null;
            animator?.SetBool("isWalking", false);
        }
    }

    private void MoveToTarget()
    {
        Vector3 dir = targetPosition.Value - transform.position;
        dir.y = 0f;

        if (dir.magnitude <= stopThreshold)
        {
            targetPosition = null;
            StopMoving();
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition.Value, moveSpeed * Time.deltaTime);
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }

    public bool HasReachedTarget()
    {
        return targetPosition == null;
    }
}
