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

    private Animator animator;

    void Start()
    {
        walking = true;
        currentDest = destinations[Random.Range(0, destinations.Count)];
        animator = GetComponentInChildren<Animator>();
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
                    SetAnimStates(false, false, true, false, false); // Searching
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
                    SetAnimStates(false, true, false, false, false); // Chasing
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
                    SetAnimStates(false, false, false, false, true); // Death
                }

                return;
            }
        }

        if (walking)
        {
            dest = currentDest.position;
            ai.destination = dest;
            ai.speed = walkSpeed;
            SetAnimStates(true, false, false, false, false); // Walking

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
                    SetAnimStates(false, false, false, true, false); // Idling
                }
            }
        }
    }

    public void stopChase()
    {
        walking = true;
        chasing = false;
        StopCoroutine("chaseRoutine");
        currentDest = destinations[Random.Range(0, destinations.Count)];
        SetAnimStates(true, false, false, false, false); // Walking
    }

    IEnumerator stayIdle()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        SetAnimStates(false, false, false, true, false); // Idling
        yield return new WaitForSeconds(idleTime);
        walking = true;
        isIdling = false;
        currentDest = destinations[Random.Range(0, destinations.Count)];
        SetAnimStates(true, false, false, false, false); // Walking
    }

    IEnumerator searchRoutine()
    {
        SetAnimStates(false, false, true, false, false); // Searching
        yield return new WaitForSeconds(Random.Range(minSearchTime, maxSearchTime));
        searching = false;
        walking = true;
        isIdling = false;
        currentDest = destinations[Random.Range(0, destinations.Count)];
        SetAnimStates(true, false, false, false, false); // Walking
    }

    IEnumerator chaseRoutine()
    {
        SetAnimStates(false, true, false, false, false); // Chasing
        yield return new WaitForSeconds(Random.Range(minChaseTime, maxChaseTime));
        stopChase();
    }

    IEnumerator deathRoutine()
    {
        SetAnimStates(false, false, false, false, true); // Dead
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
            SetAnimStates(true, false, false, false, false); // Walking
        }
    }

    private void SetAnimStates(bool isWalking, bool isChasing, bool isSearching, bool isIdling, bool isDead)
    {
        if (animator == null) return;

        animator.SetBool("isWalk", isWalking);
        Debug.Log(isWalking);
        animator.SetBool("isRun", isChasing);
       // animator.SetBool("isSearching", isSearching);
       // animator.SetBool("isWalk_Run", isIdling);
       // animator.SetBool("isDead", isDead);
    }
}
