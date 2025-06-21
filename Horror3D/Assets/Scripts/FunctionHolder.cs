using UnityEngine;

public class FunctionHolder : MonoBehaviour
{
    public Material newSkyboxMaterial;
    public GameObject DirLight;

    public void ChangeSkybox()
    {
        RenderSettings.skybox = newSkyboxMaterial;

        if(DirLight != null)
            DirLight.SetActive(false);

        DynamicGI.UpdateEnvironment();
    }
}
