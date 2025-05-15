using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [Header("Light Settings")]
    public Light flickerLight;
    public float minDelay = 0.05f;
    public float maxDelay = 0.3f;

    [Header("Emission Settings")]
    public Renderer emissionRenderer;
    public Color emissionOnColor = Color.white;
    public Color emissionOffColor = Color.black;

    private Material _material;

    void Start()
    {
        if (flickerLight == null)
            flickerLight = GetComponent<Light>();

        if (emissionRenderer != null)
        {
            _material = emissionRenderer.material;
        }

        StartCoroutine(FlickerRoutine());
    }

    System.Collections.IEnumerator FlickerRoutine()
    {
        while (true)
        {
            bool isOn = !flickerLight.enabled;
            flickerLight.enabled = isOn;

            if (_material != null)
            {
                SetEmission(isOn);
            }

            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    void SetEmission(bool isOn)
    {
        Color targetColor = isOn ? emissionOnColor : emissionOffColor;
        _material.SetColor("_EmissionColor", targetColor);
        DynamicGI.SetEmissive(emissionRenderer, targetColor);
    }
}
