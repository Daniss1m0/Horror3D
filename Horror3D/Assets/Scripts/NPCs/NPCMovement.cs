using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotationSpeed = 10f;
    public float stopThreshold = 0.1f;
    public AudioClip footstepSound;

    private Vector3? targetPosition = null;
    private bool isMoving = false;
    private Animator animator;
    private AudioSource audioSource;
    private Coroutine footstepCoroutine;
    private Coroutine pathRoutine;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
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

    public void TurnOnSteps()
    {
        if (footstepCoroutine == null)
            footstepCoroutine = StartCoroutine(PlayFootsteps());
    }

    public void TurnOffSteps()
    {
        if (footstepCoroutine != null)
        {
            StopCoroutine(footstepCoroutine);
            footstepCoroutine = null;
        }
    }

    private IEnumerator PlayFootsteps()
    {
        while (true)
        {
            if (footstepSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(footstepSound);
            }
            yield return new WaitForSeconds(1.3f);
        }
    }

    public void WalkPath(Transform[] waypoints, float waitTimeAtPoint = 2f)
    {
        if (pathRoutine != null)
        {
            StopCoroutine(pathRoutine);
        }

        pathRoutine = StartCoroutine(WalkPathRoutine(waypoints, waitTimeAtPoint));
    }

    private IEnumerator WalkPathRoutine(Transform[] waypoints, float waitTime)
    {
        int index = 0;

        TurnOnSteps();

        while (true)
        {
            Move(waypoints[index].position);

            yield return new WaitUntil(() => HasReachedTarget());

            yield return new WaitForSeconds(waitTime);

            index = (index + 1) % waypoints.Length;
        }
    }

    public void WalkPathOnce(Transform[] waypoints, float waitTimeAtPoint = 2f)
    {
        if (pathRoutine != null)
        {
            StopCoroutine(pathRoutine);
        }

        pathRoutine = StartCoroutine(WalkPathOnceRoutine(waypoints, waitTimeAtPoint));
    }

    private IEnumerator WalkPathOnceRoutine(Transform[] waypoints, float waitTime)
    {
        TurnOnSteps();

        for (int i = 0; i < waypoints.Length; i++)
        {
            Move(waypoints[i].position);
            yield return new WaitUntil(() => HasReachedTarget());
            yield return new WaitForSeconds(waitTime);
        }

        TurnOffSteps();
    }
}
