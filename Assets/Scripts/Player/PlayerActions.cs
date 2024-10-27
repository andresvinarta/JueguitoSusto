using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;

public class PlayerActions : MonoBehaviour
{
    private InputSystem_Actions PlayerInput;
    public GameObject PictureManagerCam, PolaroidCamera, InventoryManager, PlayerMainCam;

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
            if (Hit.transform.tag == "ObjectItem" || Hit.transform.tag == "NoteItem")
            {
                InteractPrompt.gameObject.SetActive(true);
                InteractPrompt.text = "Press " + PlayerInput.Player.Interact.bindings.FirstOrDefault().ToDisplayString() + " to pick up";
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
            if (Hit.transform.tag == "ObjectItem")
            {
                Hit.transform.gameObject.GetComponent<ObjectItem>().PickUp();
            }
            else if (Hit.transform.tag == "NoteItem")
            {
                Hit.transform.gameObject.GetComponent<NoteItem>().PickUp();
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
