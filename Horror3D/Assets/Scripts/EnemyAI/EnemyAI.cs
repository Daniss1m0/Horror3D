using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent ai;
    public List<Transform> destinations;
    public float walkSpeed, chaseSpeed;
    public float minIdleTime, maxIdleTime;
    public float detectionDistance, catchDistance, searchDistance;
    public float minChaseTime, maxChaseTime;
    public float minSearchTime, maxSearchTime;
    public float jumpscareTime;

    public bool walking, chasing, searching;
    public Transform player;
    public Vector3 rayCastOffset;
    public string deathScene;
    public float aiDistance;

    private bool isPlayerHidden = false;
    private bool isIdling = false;

    private Transform currentDest;
    private Vector3 dest;

    private Coroutine idleCoroutine;
    private Coroutine searchCoroutine;
    private Coroutine chaseCoroutine;

    public Animator animator;

    void Start()
    {
        walking = true;
        chasing = false;
        searching = false;

        ChooseNextDestination();
    }

    void Update()
    {
        aiDistance = Vector3.Distance(player.position, transform.position);

        if (!isPlayerHidden)
        {
            TryDetectPlayer();
        }

        if (searching)
        {
            ai.speed = 0;

            if (aiDistance <= searchDistance)
            {
                CancelCoroutines();
                chaseCoroutine = StartCoroutine(ChaseRoutine());
                chasing = true;
                searching = false;
                isIdling = false;
            }
        }

        if (chasing)
        {
            ai.destination = player.position;
            ai.speed = chaseSpeed;

            if (aiDistance <= catchDistance)
            {
                player.gameObject.SetActive(false);
                StartCoroutine(DeathRoutine());
                chasing = false;
                isIdling = false;
            }

            SetAnimStates(false, true);
            return;
        }

        if (walking)
        {
            ai.destination = currentDest.position;
            ai.speed = walkSpeed;

            if (!ai.pathPending && ai.remainingDistance <= ai.stoppingDistance && !isIdling)
            {
                StartIdling();
            }
        }

        SetAnimStates(walking, chasing);
    }

    private void TryDetectPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position + rayCastOffset, direction, out hit, detectionDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                CancelCoroutines();

                walking = false;
                searching = true;
                isIdling = false;

                searchCoroutine = StartCoroutine(SearchRoutine());
            }
        }
    }

    private void StartIdling()
    {
        isIdling = true;
        walking = false;
        ai.speed = 0;

        idleCoroutine = StartCoroutine(IdleRoutine());
    }

    private IEnumerator IdleRoutine()
    {
        float idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);

        isIdling = false;
        walking = true;
        ChooseNextDestination();
    }

    private IEnumerator SearchRoutine()
    {
        float searchTime = Random.Range(minSearchTime, maxSearchTime);
        yield return new WaitForSeconds(searchTime);

        searching = false;
        walking = true;
        isIdling = false;
        ChooseNextDestination();
    }

    private IEnumerator ChaseRoutine()
    {
        float chaseTime = Random.Range(minChaseTime, maxChaseTime);
        yield return new WaitForSeconds(chaseTime);

        StopChase();
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(jumpscareTime);
        SceneManager.LoadScene(deathScene);
    }

    public void StopChase()
    {
        if (chaseCoroutine != null)
        {
            StopCoroutine(chaseCoroutine);
            chaseCoroutine = null;
        }

        chasing = false;
        walking = true;
        ChooseNextDestination();
    }

    private void CancelCoroutines()
    {
        if (idleCoroutine != null)
        {
            StopCoroutine(idleCoroutine);
            idleCoroutine = null;
        }

        if (searchCoroutine != null)
        {
            StopCoroutine(searchCoroutine);
            searchCoroutine = null;
        }

        if (chaseCoroutine != null)
        {
            StopCoroutine(chaseCoroutine);
            chaseCoroutine = null;
        }

        isIdling = false;
    }

    public void SetPlayerHidden(bool hidden)
    {
        isPlayerHidden = hidden;

        if (hidden && chasing)
        {
            StopChase();
        }
    }

    private void ChooseNextDestination()
    {
        if (destinations.Count > 0)
        {
            currentDest = destinations[Random.Range(0, destinations.Count)];
        }
    }

    private void SetAnimStates(bool isWalking, bool isChasing)
    {
        if (animator == null) return;

        animator.SetBool("isWalk", isWalking && !isChasing);
        animator.SetBool("isRun", isChasing);
    }
}
