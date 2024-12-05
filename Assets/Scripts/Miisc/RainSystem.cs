using UnityEngine;

public class RainSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject RainSFX;

    public void ChangeRain(bool IsIndoors)
    {
        if (IsIndoors)
        {
            RainSFX.GetComponents<AudioSource>()[0].enabled = false;
            RainSFX.GetComponents<AudioSource>()[1].enabled = true;
        }
        else
        {
            RainSFX.GetComponents<AudioSource>()[1].enabled = false;
            RainSFX.GetComponents<AudioSource>()[0].enabled = true;
        }
    }
}
