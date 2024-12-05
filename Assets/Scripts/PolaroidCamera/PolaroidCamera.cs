using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PolaroidCamera : MonoBehaviour
{
    public Camera polaroidCamera; // Cámara que captura la foto
    public RenderTexture renderTexture; // RenderTexture asignada a la cámara
    public GameObject photoPrefab; // Prefab que representa la foto impresa en el mundo del juego
    public Transform spawnPoint; // Punto donde aparecerá la foto "impresa"
    GameObject printedPhoto;
    public GameObject PictureManagerx, CameraFlash;
    public InventoryManager InventoryManager;
    private InputSystem_Actions PlayerInput;
    private bool TakingPicture = false;
    private bool IsFlashing = false;

    private bool FlashActive = true;

    [SerializeField]
    private float FlashTime = 0.25f, PictureTime = 1.5f;


    [SerializeField]
    private int PicturesPerReel = 8;

    private int PicturesLeft = 0;
    private int ReelsLeft = 0;

    [SerializeField]
    private TextMeshProUGUI PicturesLeftText;

    [SerializeField]
    private TextMeshProUGUI ReelsLeftText;

    [SerializeField]
    private GameObject PolaroidPreviewCanvas;

    private bool InPreview = false, MovingCam = false, FadingIn = false;

    [SerializeField]
    private GameObject CameraModel, OffViewPosition;

    private Vector3 CameraOriginalPos;
    private Quaternion CameraOriginalRotation;

    private float CameraPosThreshold = 0.005f, VelToOffsetPos = 15f, VelToOffsetRot = 500f, FadeInRate = 7.5f, FadeInTreshold = 0.025f;

    private bool JumpscareInPicture = false;

    private void Awake()
    {
        CameraOriginalPos = CameraModel.transform.localPosition;
        CameraOriginalRotation = CameraModel.transform.localRotation;
        polaroidCamera.enabled = false;
        PlayerInput = new InputSystem_Actions();
        PlayerInput.Enable();
        PlayerInput.Player.PolaroidPreview.started += CameraToOffView;
        PlayerInput.Player.PolaroidPreview.canceled += HidePreview;
        PlayerInput.Player.TakePicture.performed += TakePicture;
        PlayerInput.Player.LoadReel.performed += LoadNewReel;
        InventoryManager = GameObject.Find("UICamera").GetComponent<InventoryManager>();
        CameraFlash.GetComponent<Light>().enabled = false;
        PicturesLeft = PicturesPerReel;
        UpdateLeftsTexts();
    }

    private void Update()
    {
        if (MovingCam)
        {
            if (Vector3.Distance(CameraModel.transform.localPosition, OffViewPosition.transform.localPosition) > CameraPosThreshold && !InPreview)
            {
                CameraModel.transform.localPosition = Vector3.Lerp(CameraModel.transform.localPosition, OffViewPosition.transform.localPosition, VelToOffsetPos * Time.deltaTime);
                CameraModel.transform.localRotation = Quaternion.Lerp(CameraModel.transform.localRotation, OffViewPosition.transform.localRotation, VelToOffsetRot * Time.deltaTime);
            }

            if (Vector3.Distance(CameraModel.transform.localPosition, OffViewPosition.transform.localPosition) < FadeInTreshold && !InPreview)
            {
                FadingIn = true;
            }

            if (FadingIn && !InPreview)
            {
                PolaroidPreviewCanvas.GetComponent<CanvasGroup>().alpha += Mathf.Lerp(0, 1, FadeInRate * Time.deltaTime);
            }

            if (PolaroidPreviewCanvas.GetComponent<CanvasGroup>().alpha >= 1 && !InPreview)
            {
                ShowPreview();
            }
        }
        else
        { 
            if (Vector3.Distance(CameraOriginalPos, CameraModel.transform.localPosition) > CameraPosThreshold)
            {
                CameraModel.transform.localPosition = Vector3.Lerp(CameraModel.transform.localPosition, CameraOriginalPos, VelToOffsetPos * Time.deltaTime);
                CameraModel.transform.localRotation = Quaternion.Lerp(CameraModel.transform.localRotation, CameraOriginalRotation, VelToOffsetRot * Time.deltaTime);
            }
        }
    }

    public void TakePicture(InputAction.CallbackContext context)
    {
        if (PictureManagerx.GetComponent<PictureManager>().IsShowing() || TakingPicture || IsFlashing || InventoryManager.IsInventoryShowing()) return;

        if (PicturesLeft <= 0)
        {
            return;
        }

        // Activa la cámara Polaroid (si está desactivada)
        polaroidCamera.enabled = true;

        //Activamos cooldown
        StartCoroutine(ExecuteAfterTime(PictureTime));

        if (FlashActive)
        {
            StartCoroutine(Flashing(FlashTime));
        }

        //Sonido de foto
        this.gameObject.GetComponents<AudioSource>()[0].Play();

        // Captura la imagen como una textura
        Texture2D photoTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        polaroidCamera.Render();
        photoTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        photoTexture.Apply();
        RenderTexture.active = null;

        //Instanciamos prefab de la foto
        printedPhoto = Instantiate(photoPrefab, spawnPoint.position, spawnPoint.rotation);

        //Añadimos la foto como textura al prefab
        GameObject Photo = printedPhoto.GetComponentInChildren<Transform>().gameObject.GetComponentsInChildren<Transform>()[3].gameObject;
        Photo.GetComponent<Renderer>().material.mainTexture = photoTexture;

        //Añadimos la foto al InventoryManager
        printedPhoto.GetComponent<PictureItem>().SetPictureNumber(InventoryManager.GetNumberOfPictures() + 1);
        InventoryManager.AddPicture(printedPhoto);
        //PictureManagerx.GetComponent<PictureManager>().AddPicture(printedPhoto);

        //Establecemos el layer de la foto como UI para poder verla después
        printedPhoto.layer = LayerMask.NameToLayer("ItemRenderUI");
        Transform[] Children = printedPhoto.GetComponentsInChildren<Transform>();
        for (int i = 0; i < printedPhoto.GetComponentsInChildren<Transform>().Length; i++)
        {
            Children[i].gameObject.layer = LayerMask.NameToLayer("ItemRenderUI");
        }

        //Actualizamos la escala y rotación de la foto
        //printedPhoto.transform.localScale = new Vector3(200, 200, 200);
        //printedPhoto.transform.rotation = Quaternion.Euler(90, 180, 0);
        //printedPhoto.transform.Rotate(new Vector3(0, 180, 0));
        //printedPhoto.transform.Rotate(new Vector3(90, 0, 0));

        if (JumpscareInPicture)
        {
            printedPhoto.GetComponent<PictureItem>().SetJumpscare();
        }

        //Desactivamos el objeto
        printedPhoto.SetActive(false);

        //PictureManagerx.GetComponent<PictureManager>().JustTookAPicture();

        // Desactiva la cámara después de capturar la imagen
        polaroidCamera.enabled = false;

        PicturesLeft--;
        UpdateLeftsTexts();
    }


    public void CameraToOffView(InputAction.CallbackContext context)
    {
        if (PictureManagerx.GetComponent<PictureManager>().IsShowing() || TakingPicture || IsFlashing || InventoryManager.IsInventoryShowing()) return;
        MovingCam = true;
    }

    public void ShowPreview()
    {
        FadingIn = false;
        InPreview = true;
    }

    public void HidePreview(InputAction.CallbackContext context)
    {
        PolaroidPreviewCanvas.GetComponent<CanvasGroup>().alpha = 0f;
        InPreview = false;
        MovingCam = false;
    }

    public bool IsInPreview()
    {
        return InPreview;
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        if (TakingPicture)
        {
            yield break;
        }

        TakingPicture = true;

        yield return new WaitForSeconds(time);

        TakingPicture = false;
    }

    IEnumerator Flashing(float time)
    {
        if (IsFlashing)
        {
            yield break;
        }

        IsFlashing = true;
        CameraFlash.GetComponent<Light>().enabled = true;

        yield return new WaitForSeconds(time);

        CameraFlash.GetComponent<Light>().enabled = false;
        IsFlashing = false;
    }


    public bool IsTakingPicture()
    {
        return TakingPicture;
    }

    public void ActivateOrDeactivateFlash()
    {
        FlashActive = !FlashActive;
    }

    public void AddReelAmount(int Amount)
    {
        ReelsLeft += Amount;
        UpdateLeftsTexts();
    }

    public void RemoveReelAmount(int Amount) 
    {
        ReelsLeft -= Amount;
        UpdateLeftsTexts();
    }

    private void LoadNewReel(InputAction.CallbackContext context)
    {
        if ((ReelsLeft <= 0 || PicturesLeft > 0) || (TakingPicture || IsFlashing || InventoryManager.IsInventoryShowing()))
        {
            return;
        }
        ReelsLeft -= 1;
        PicturesLeft = PicturesPerReel;
        UpdateLeftsTexts();
    }

    private void UpdateLeftsTexts()
    {
        PicturesLeftText.text = PicturesLeft.ToString() + " Pictures left";
        ReelsLeftText.text = ReelsLeft.ToString() + " Reels left";
    }

    public bool OutOfPictures()
    {
        return (PicturesLeft <= 0 && ReelsLeft >= 0);
    }

    public void JumpscareUpdate(bool NewValue)
    {
        JumpscareInPicture = NewValue;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    Debug.Log("BBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
    //}
}
