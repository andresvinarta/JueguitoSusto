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
    private InputSystem_Actions PlayerInput;
    private bool TakingPicture = false;

    private void Awake()
    {
        polaroidCamera.enabled = false;
        PlayerInput = new InputSystem_Actions();
        PlayerInput.Enable();
        PlayerInput.Player.TakePicture.performed += TakePicture;
    }

    public void TakePicture(InputAction.CallbackContext context)
    {
        if (PictureManagerx.GetComponent<PictureManager>().IsShowing() || TakingPicture) return;


        // Activa la cámara Polaroid (si está desactivada)
        polaroidCamera.enabled = true;

        StartCoroutine(ExecuteAfterTime(1.5f));

        // Captura la imagen como una textura
        Texture2D photoTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        polaroidCamera.Render();
        photoTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        photoTexture.Apply();
        RenderTexture.active = null;

        printedPhoto = Instantiate(photoPrefab, spawnPoint.position, spawnPoint.rotation);
        printedPhoto.transform.Rotate(new Vector3(0, 180, 0));
        printedPhoto.transform.Rotate(new Vector3(90, 0, 0));
        GameObject Photo = printedPhoto.GetComponentInChildren<Transform>().gameObject.GetComponentsInChildren<Transform>()[3].gameObject;
        Photo.GetComponent<Renderer>().material.mainTexture = photoTexture;
        PictureManagerx.GetComponent<PictureManager>().AddPicture(printedPhoto);
        printedPhoto.layer = LayerMask.NameToLayer("UI");
        printedPhoto.SetActive(false);

        PictureManagerx.GetComponent<PictureManager>().JustTookAPicture();

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
}
