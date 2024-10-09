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
    int PhotosTaken = 0;
    private InputSystem_Actions PlayerInput;

    private void Awake()
    {
        PlayerInput = new InputSystem_Actions();
        PlayerInput.Enable();
        PlayerInput.Player.TakePicture.performed += TakePicture;
    }

    void TakePicture(InputAction.CallbackContext context)
    {
        // Activa la cámara Polaroid (si está desactivada)
        polaroidCamera.enabled = true;

        // Captura la imagen como una textura
        Texture2D photoTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        polaroidCamera.Render();
        photoTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        photoTexture.Apply();
        RenderTexture.active = null;

        // Instanciar la "foto impresa" en el mundo del juego
        if (printedPhoto != null)
        {
            //Destroy(printedPhoto );
        }
        printedPhoto = Instantiate(photoPrefab, spawnPoint.position, spawnPoint.rotation);
        //PhotosTaken++;
        //printedPhoto.GetComponent<Material>().SetFloat("Smoothness", 0);
        printedPhoto.transform.Rotate(new Vector3(0, 180, 0));
        printedPhoto.transform.Rotate(new Vector3(90, 0, 0));
        printedPhoto.GetComponent<Renderer>().material.mainTexture = photoTexture;
        PictureManagerx.GetComponent<PictureManager> ().AddPicture(printedPhoto);
        printedPhoto.transform.SetParent(PictureManagerx.transform);
        printedPhoto.layer = LayerMask.NameToLayer("UI");
        //printedPhoto.SetActive(false);

        // Desactiva la cámara después de capturar la imagen
        polaroidCamera.enabled = false;
    }
}
