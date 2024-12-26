using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;

public class PlayerActions : MonoBehaviour
{
    private InputSystem_Actions PlayerInput;
    public GameObject PictureManagerCam, PolaroidCamera, InventoryManager, PlayerMainCam, PauseMenu;

    public TextMeshProUGUI InteractPrompt;

    [SerializeField]
    private float InteractDistance = 1.0f;

    private LayerMask InteractLayerMask;

    [SerializeField]
    private LayerMask[] LayerMasks;

    private void Awake()
    {
        PlayerInput = new InputSystem_Actions();
        PlayerInput.Enable();
        //PlayerInput.Player.CheckPictures.performed += CheckPictures;
        PlayerInput.Player.CheckPictures.performed += OpenInventory;
        PlayerInput.Player.Interact.performed += Interact;
        PlayerInput.Player.Pause.performed += PauseGame;
        foreach (LayerMask Layer in LayerMasks)
        {
            InteractLayerMask = InteractLayerMask | Layer;
        }
    }

    private void Update()
    {
        Ray InteractRay = new Ray(PlayerMainCam.transform.position, PlayerMainCam.transform.forward);
        if (Physics.Raycast(InteractRay, out RaycastHit Hit, InteractDistance, InteractLayerMask))
        {
            if (Hit.transform.tag == "ObjectItem" || Hit.transform.tag == "NoteItem" || Hit.transform.tag == "Key" || Hit.transform.tag == "Reel")
            {
                InteractPrompt.gameObject.SetActive(true);
                InteractPrompt.text = "Press " + PlayerInput.Player.Interact.bindings.FirstOrDefault().ToDisplayString() + " to pick up";
            }
            else if (Hit.transform.tag == "Door" && !Hit.transform.gameObject.GetComponent<Door>().IsDoorSwinging())
            {
                InteractPrompt.gameObject.SetActive(true);
                if (Hit.transform.gameObject.GetComponent<Door>().IsDoorLocked())
                {
                    if (InventoryManager.GetComponent<InventoryManager>().HaveKeyWithID(Hit.transform.gameObject.GetComponent<Door>().GetDoorID()))
                    {
                        InteractPrompt.text = "Press " + PlayerInput.Player.Interact.bindings.FirstOrDefault().ToDisplayString() + " to unlock";
                    }
                    else
                    {
                        InteractPrompt.text = "The door seems to be locked, find the key";
                    }
                }
                else
                {
                    if (Hit.transform.gameObject.GetComponent<Door>().IsDoorOpen())
                    {
                        InteractPrompt.text = "Press " + PlayerInput.Player.Interact.bindings.FirstOrDefault().ToDisplayString() + " to close";
                    }
                    else
                    {
                        InteractPrompt.text = "Press " + PlayerInput.Player.Interact.bindings.FirstOrDefault().ToDisplayString() + " to open";
                    }
                }
            }
        }
        else
        {
            InteractPrompt.gameObject.SetActive(false);
        }
    }

    public void Interact(InputAction.CallbackContext callbackContext)
    {
        Ray InteractRay = new Ray(PlayerMainCam.transform.position, PlayerMainCam.transform.forward);
        if (Physics.Raycast(InteractRay, out RaycastHit Hit, InteractDistance, InteractLayerMask))
        {
            if (Hit.transform.tag == "ObjectItem" || Hit.transform.tag == "Key")
            {
                Hit.transform.gameObject.GetComponent<ObjectItem>().PickUp();
            }
            else if (Hit.transform.tag == "NoteItem")
            {
                Hit.transform.gameObject.GetComponent<NoteItem>().PickUp();
            }
            else if (Hit.transform.tag == "Reel")
            {
                Hit.transform.gameObject.GetComponent<CameraReel>().PickUp();
            }
            else if (Hit.transform.tag == "Door")
            {
                if (Hit.transform.gameObject.GetComponent<Door>().IsDoorLocked())
                {
                    if (InventoryManager.GetComponent<InventoryManager>().HaveKeyWithID(Hit.transform.gameObject.GetComponent<Door>().GetDoorID()))
                    {
                        Hit.transform.gameObject.GetComponent<Door>().UnlockDoor();
                    }
                    else
                    {
                        Hit.transform.gameObject.GetComponent<Door>().FailedToUnlock();
                    }
                }
                else
                {
                    if (Hit.transform.gameObject.GetComponent<Door>().IsDoorOpen())
                    {
                        Hit.transform.gameObject.GetComponent<Door>().CloseDoor();
                    }
                    else
                    {
                        Hit.transform.gameObject.GetComponent<Door>().OpenDoor();
                    }
                }
            }
        }
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
        if (!InventoryManager.GetComponent<InventoryManager>().IsInventoryShowing() && !PauseMenu.GetComponent<PauseMenuManager>().IsPauseMenuActive() && !PolaroidCamera.GetComponent<PolaroidCamera>().IsTakingPicture())
        {
            InventoryManager.GetComponent<InventoryManager>().OpenInventory();
        }
        else
        {
            InventoryManager.GetComponent<InventoryManager>().CloseInventory();
        }
    }

    public void PauseGame(InputAction.CallbackContext callbackContext)
    {
        if (!PauseMenu.GetComponent<PauseMenuManager>().IsPauseMenuActive() && !InventoryManager.GetComponent<InventoryManager>().IsInventoryShowing())
        {
            PauseMenu.GetComponent<PauseMenuManager>().OpenPauseMenu();
        }
        else
        {
            PauseMenu.GetComponent<PauseMenuManager>().ClosePauseMenu();
        }
    }
}
