using UnityEngine;
using UnityEngine.UI;

public class StartGameButtonScript : MonoBehaviour
{
    public GameObject mainMenuCanvas;

    public void StartGame()
    {
        mainMenuCanvas.SetActive(false);
        Debug.Log("wcisnal start");
        // display start text
    }
}
