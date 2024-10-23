using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    private InputSystem_Actions PlayerInput;
    public GameObject PictureManagerCam, PolaroidCamera, InventoryManager;

    private void Awake()
    {
        PlayerInput = new InputSystem_Actions();
        PlayerInput.Enable();
        //PlayerInput.Player.CheckPictures.performed += CheckPictures;
        PlayerInput.Player.CheckPictures.performed += OpenInventory;
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

    public void OpenInventory(InputAction.CallbackContext callbackContext)
    {
        if (!InventoryManager.GetComponent<InventoryManager>().IsInventoryShowing())
        {
            InventoryManager.GetComponent<InventoryManager>().OpenInventory();
        }
        else
        {
            InventoryManager.GetComponent<InventoryManager>().CloseInventory();
        }
    }
}
