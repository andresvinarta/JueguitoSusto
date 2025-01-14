using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InCameraDetect : MonoBehaviour
{

    public Camera CameraToDetect;
    private Plane[] CameraPlanes;
    Collider ObjectCollider;
    Bounds ColliderBounds;

    private bool AlreadyDetected = false, AbleToBeDetected = false;

    [SerializeField]
    private GameObject[] ObjectsToActivate, AudioHighlighters;

    [SerializeField]
    public bool ActivateDetectionByTrigger = true, PlaySoundsWhenAbleToBeDetected = true;

    private InputSystem_Actions PlayerInput;

    public abstract void DetectedByCamera();

    public abstract void UndetectedByCamera();

    public abstract void PictureOfObjectTaken();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AbleToBeDetected = !ActivateDetectionByTrigger;
        PlayerInput = new InputSystem_Actions();
        PlayerInput.Enable();
        PlayerInput.Player.TakePicture.performed += TakePictureIfDetected;
        ObjectCollider = GetComponent<Collider>();
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
            DetectedByCamera();
        }
        else if (!AbleToBeDetected || !GeometryUtility.TestPlanesAABB(CameraPlanes, ColliderBounds) && AlreadyDetected)
        {
            AlreadyDetected = false;
            UndetectedByCamera();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ActivateDetectionByTrigger && other.tag == "Player")
        {
            AbleToBeDetected = true;
            foreach (GameObject ObjectToActivate in ObjectsToActivate)
            {
                ObjectToActivate.SetActive(true);
            }
            if (PlaySoundsWhenAbleToBeDetected)
            {
                foreach (GameObject AudioHighlighter in AudioHighlighters)
                {
                    AudioHighlighter Highlighter = AudioHighlighter.GetComponent<AudioHighlighter>();
                    Highlighter.PlaySound();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (ActivateDetectionByTrigger && other.tag == "Player")
        {
            AbleToBeDetected = false;
            foreach (GameObject ObjectToActivate in ObjectsToActivate)
            {
                ObjectToActivate.SetActive(false);
            }
            if (PlaySoundsWhenAbleToBeDetected)
            {
                foreach (GameObject AudioHighlighter in AudioHighlighters)
                {
                    AudioHighlighter Highlighter = AudioHighlighter.GetComponent<AudioHighlighter>();
                    Highlighter.StopSound();
                }
            }
        }
    }

    public void TakePictureIfDetected(InputAction.CallbackContext callbackContext)
    {
        if (AlreadyDetected)
        {
            PictureOfObjectTaken();
        }
    }
}
