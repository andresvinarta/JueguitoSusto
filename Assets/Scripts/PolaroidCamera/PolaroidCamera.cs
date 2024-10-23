using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PolaroidCamera : MonoBehaviour
{
    public Camera polaroidCamera; // Cámara que captura la foto
    public RenderTexture renderTexture; // RenderTexture asignada a la cámara
    public GameObject photoPrefab; // Prefab que representa la foto impresa en el mundo del juego
    public Transform spawnPoint; // Punto donde aparecerá la foto "impresa"
    GameObject printedPhoto;
    public GameObject PictureManagerx;
    public InventoryManager InventoryManager;
    private InputSystem_Actions PlayerInput;
    private bool TakingPicture = false;

    private void Awake()
    {
        polaroidCamera.enabled = false;
        PlayerInput = new InputSystem_Actions();
        PlayerInput.Enable();
        PlayerInput.Player.TakePicture.performed += TakePicture;
        InventoryManager = GameObject.Find("UICamera").GetComponent<InventoryManager>();
    }

    public void TakePicture(InputAction.CallbackContext context)
    {
        if (PictureManagerx.GetComponent<PictureManager>().IsShowing() || TakingPicture || InventoryManager.IsInventoryShowing()) return;


        // Activa la cámara Polaroid (si está desactivada)
        polaroidCamera.enabled = true;

        //Activamos cooldown
        StartCoroutine(ExecuteAfterTime(1.5f));

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

        //Desactivamos el objeto
        printedPhoto.SetActive(false);

        //PictureManagerx.GetComponent<PictureManager>().JustTookAPicture();

        // Desactiva la cámara después de capturar la imagen
        polaroidCamera.enabled = false;
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


    public bool IsTakingPicture()
    {
        return TakingPicture;
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
