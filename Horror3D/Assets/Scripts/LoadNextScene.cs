using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    public Quest Quest;
    public void Update()
    {
        if (Quest.IsComplete)
        {
            LoadScene();
        }
    }
    public void LoadScene()
    {
        SceneManager.LoadScene("Cutscene", LoadSceneMode.Single);
    }
}
