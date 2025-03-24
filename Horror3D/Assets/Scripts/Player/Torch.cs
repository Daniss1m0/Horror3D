using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class NewMonoBehaviourScript : MonoBehaviour
{

    public GameObject torch;
    public Light torchSpotLight;

    public AudioSource flashlightOn;
    public AudioSource flashlightOff;

    private void Start()
    {
        torchSpotLight.enabled = false;
        torch.SetActive(false);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            torchSpotLight.enabled = !torchSpotLight.enabled;
            torch.SetActive(torchSpotLight.enabled);

            if (torchSpotLight.enabled )
                flashlightOn.Play();
            else 
                flashlightOff.Play();
        }
    }
}
