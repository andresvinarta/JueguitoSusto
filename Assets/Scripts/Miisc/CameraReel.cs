using UnityEngine;

public class CameraReel : MonoBehaviour
{
    [SerializeField]
    private PolaroidCamera Polaroid;

    public void Start()
    {
        Polaroid = GameObject.Find("PolaroidCamera").GetComponent<PolaroidCamera>();
    }

    public void PickUp()
    {
        Polaroid.AddReelAmount(1);
        Destroy(this.gameObject);
    }
}
