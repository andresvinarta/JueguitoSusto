using UnityEngine;

public class PolaroidCamera : MonoBehaviour
{
    public Camera polaroidCamera; // Cámara que captura la foto
    public RenderTexture renderTexture; // RenderTexture asignada a la cámara
    public GameObject photoPrefab; // Prefab que representa la foto impresa en el mundo del juego
    public Transform spawnPoint; // Punto donde aparecerá la foto "impresa"

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Si presionas la barra espaciadora, tomas la foto
        {
            TakePicture();
        }
    }

    void TakePicture()
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
        GameObject printedPhoto = Instantiate(photoPrefab, spawnPoint.position, spawnPoint.rotation);
        printedPhoto.transform.Rotate(new Vector3(-90, 180, 0));
        printedPhoto.GetComponent<Renderer>().material.mainTexture = photoTexture;

        // Desactiva la cámara después de capturar la imagen
        polaroidCamera.enabled = false;
    }
}
