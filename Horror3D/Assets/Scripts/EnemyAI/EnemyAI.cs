using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent ai;
    public List<Transform> destinations;
    public float walkSpeed, chaseSpeed, minIdleTime, maxIdleTime, idleTime,
        detectionDistance, catchDistance, searchDistance,
        minChaseTime, maxChaseTime, minSearchTime, maxSearchTime, jumpscareTime;

    public bool walking, chasing, searching;
    public Transform player;
    Transform currentDest;
    Vector3 dest;
    public Vector3 rayCastOffset;
    public string deathScene;
    public float aiDistance;

    private bool isPlayerHidden = false;
    private bool isIdling = false;
    private bool initialIdleSkipped = false;

    public Animator animator;

    // Do porównywania zmiany stanów animacji
    private bool prevIsWalking, prevIsChasing;

    void Start()
    {
        if (animator == null)
        {
            Debug.LogError("❌ Animator не присвоен!");
        }
        else
        {
            Debug.Log("✅ Animator найден!");
        }

        walking = true;
        currentDest = destinations[Random.Range(0, destinations.Count)];
    }


    void Update()
    {
        aiDistance = Vector3.Distance(player.position, transform.position);

        if (!isPlayerHidden)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            RaycastHit hit;
            if (Physics.Raycast(transform.position + rayCastOffset, direction, out hit, detectionDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    walking = false;
                    StopAllCoroutines();
                    StartCoroutine(searchRoutine());
                    searching = true;
                    isIdling = false;
                }
            }

            if (searching)
            {
                ai.speed = 0;

                if (aiDistance <= searchDistance)
                {
                    StopAllCoroutines();
                    StartCoroutine(chaseRoutine());
                    chasing = true;
                    searching = false;
                    isIdling = false;
                }
            }

            if (chasing)
            {
                dest = player.position;
                ai.destination = dest;
                ai.speed = chaseSpeed;

                if (aiDistance <= catchDistance)
                {
                    player.gameObject.SetActive(false);
                    StartCoroutine(deathRoutine());
                    chasing = false;
                    isIdling = false;
                }

                SetAnimStates(false, true);
                return;
            }
        }

        if (walking)
        {
            dest = currentDest.position;
            ai.destination = dest;
            ai.speed = walkSpeed;

            if (ai.remainingDistance <= ai.stoppingDistance && !isIdling && !ai.pathPending)
            {
                ai.speed = 0;
                isIdling = true;

                if (!initialIdleSkipped)
                {
                    initialIdleSkipped = true;
                    walking = true;
                    isIdling = false;
                    currentDest = destinations[Random.Range(0, destinations.Count)];
                }
                else
                {
                    StartCoroutine(stayIdle());
                    walking = false;
                }
            }
        }

        // Aktualizacja Animatora
        SetAnimStates(walking, chasing);
    }

    public void stopChase()
    {
        walking = true;
        chasing = false;
        StopCoroutine("chaseRoutine");
        currentDest = destinations[Random.Range(0, destinations.Count)];
    }

    IEnumerator stayIdle()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        walking = true;
        isIdling = false;
        currentDest = destinations[Random.Range(0, destinations.Count)];
    }

    IEnumerator searchRoutine()
    {
        yield return new WaitForSeconds(Random.Range(minSearchTime, maxSearchTime));
        searching = false;
        walking = true;
        isIdling = false;
        currentDest = destinations[Random.Range(0, destinations.Count)];
    }

    IEnumerator chaseRoutine()
    {
        yield return new WaitForSeconds(Random.Range(minChaseTime, maxChaseTime));
        stopChase();
    }

    IEnumerator deathRoutine()
    {
        yield return new WaitForSeconds(jumpscareTime);
        SceneManager.LoadScene(deathScene);
    }

    public void SetPlayerHidden(bool hidden)
    {
        isPlayerHidden = hidden;

        if (hidden && chasing)
        {
            StopCoroutine("chaseRoutine");
            chasing = false;
            walking = true;
            currentDest = destinations[Random.Range(0, destinations.Count)];
        }
    }

    // ✅ Animacja tylko gdy wartość się zmienia
    private void SetAnimStates(bool isWalking, bool isChasing)
    {
        if (animator == null) return;

        bool walkState = isWalking && !isChasing;
        bool runState = isChasing;

        Debug.Log($"[Anim] Set isWalk: {walkState}, isRun: {runState}");

        animator.SetBool("isWalk", walkState);
        animator.SetBool("isRun", runState);
    }



}
