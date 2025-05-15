using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light flickerLight;
    public float minDelay = 0.5f;
    public float maxDelay = 2.0f;

    void Start()
    {
        if (flickerLight == null)
            flickerLight = GetComponent<Light>();

        StartCoroutine(FlickerRoutine());
    }

    System.Collections.IEnumerator FlickerRoutine()
    {
        while (true)
        {
            flickerLight.enabled = !flickerLight.enabled;
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
        }
    }
}
