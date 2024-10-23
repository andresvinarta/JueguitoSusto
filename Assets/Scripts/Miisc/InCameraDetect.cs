using UnityEngine;

public class InCameraDetect : MonoBehaviour
{

    public Camera CameraToDetect;
    private Plane[] CameraPlanes;
    Collider ObjectCollider;
    Bounds ColliderBounds;

    private bool AlreadyDetected = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ObjectCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraPlanes = GeometryUtility.CalculateFrustumPlanes(CameraToDetect);
        ColliderBounds = ObjectCollider.bounds;
        if (GeometryUtility.TestPlanesAABB(CameraPlanes, ColliderBounds) && !AlreadyDetected)
        {
            AlreadyDetected = true;
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        }
        else if (!GeometryUtility.TestPlanesAABB(CameraPlanes, ColliderBounds) && AlreadyDetected)
        {
            AlreadyDetected = false;
            Debug.Log("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
        }
    }
}
