using UnityEngine;

public class FunctionHolder : MonoBehaviour
{
    public QuestManager QuestManager;
    public Material newSkyboxMaterial;
    public GameObject DirLight;

    private void Start()
    {
        if (!QuestManager.GetStartedQuestRoutine())
        {
            QuestManager.StartQuestRoutine();
        }
    }
    public void ChangeSkybox()
    {
        RenderSettings.skybox = newSkyboxMaterial;

        if(DirLight != null)
            DirLight.SetActive(false);

        DynamicGI.UpdateEnvironment();
    }
}
