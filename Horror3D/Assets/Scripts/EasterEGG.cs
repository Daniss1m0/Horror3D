using UnityEngine;
using System.Collections;

public class EasterEGG : MonoBehaviour
{
    public GameObject �����;
    public Transform player;
    public float activationDistance = 3f;

    private bool isActive = false;

    void Start()
    {
        if (����� != null)
            �����.SetActive(false);
    }

    void Update()
    {
        if (!isActive && Vector3.Distance(transform.position, player.position) <= activationDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(ActivatePapich());
            }
        }
    }

    IEnumerator ActivatePapich()
    {
        isActive = true;
        �����.SetActive(true);
        yield return new WaitForSeconds(15f);
        �����.SetActive(false);
        isActive = false;
    }
}
