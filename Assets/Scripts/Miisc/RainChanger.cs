using UnityEngine;

public class RainChanger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindAnyObjectByType<RainSystem>().ChangeRain(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindAnyObjectByType<RainSystem>().ChangeRain(false);
        }
    }
}
