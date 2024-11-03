using UnityEngine;

public class InCameraDetect : MonoBehaviour
{

    public Camera CameraToDetect;
    private Plane[] CameraPlanes;
    Collider ObjectCollider;
    Bounds ColliderBounds;

    private bool AlreadyDetected = false, AbleToBeDetected = false;

    private PolaroidCamera Polaroid;

    [SerializeField]
    private GameObject[] ObjectsToActivate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ObjectCollider = GetComponent<Collider>();
        Polaroid = GameObject.Find("PolaroidCamera").GetComponent<PolaroidCamera>();
        foreach (GameObject ObjectToActivate in ObjectsToActivate)
        {
            ObjectToActivate.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CameraPlanes = GeometryUtility.CalculateFrustumPlanes(CameraToDetect);
        ColliderBounds = ObjectCollider.bounds;
        if (AbleToBeDetected && GeometryUtility.TestPlanesAABB(CameraPlanes, ColliderBounds) && !AlreadyDetected)
        {
            AlreadyDetected = true;
            Polaroid.JumpscareUpdate(true);
        }
        else if (!AbleToBeDetected || !GeometryUtility.TestPlanesAABB(CameraPlanes, ColliderBounds) && AlreadyDetected)
        {
            AlreadyDetected = false;
            Polaroid.JumpscareUpdate(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AbleToBeDetected = true;
            foreach (GameObject ObjectToActivate in ObjectsToActivate)
            {
                ObjectToActivate.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            AbleToBeDetected = false;
            foreach (GameObject ObjectToActivate in ObjectsToActivate)
            {
                ObjectToActivate.SetActive(false);
            }
        }
    }
}
