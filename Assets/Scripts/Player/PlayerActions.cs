using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    private InputSystem_Actions PlayerInput;
    public GameObject PictureManagerCam, PolaroidCamera;

    private void Awake()
    {
        PlayerInput = new InputSystem_Actions();
        PlayerInput.Enable();
        PlayerInput.Player.CheckPictures.performed += CheckPictures;
    }

    public void Interact()
    {

    }

    public void CheckPictures(InputAction.CallbackContext callbackContext)
    {
        if (PictureManagerCam.GetComponent<PictureManager>().ThereArePictures() && !PolaroidCamera.GetComponent<PolaroidCamera>().IsTakingPicture())
        {
            if (!PictureManagerCam.GetComponent<PictureManager>().IsShowing())
            {
                PictureManagerCam.GetComponent<PictureManager>().SetUp();
            }
            else
            {
                PictureManagerCam.GetComponent<PictureManager>().CloseUp();
            }
        }
    }
}
