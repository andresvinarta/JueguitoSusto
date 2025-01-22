using UnityEngine;
using UnityEngine.UI;

public class RawImageCorrector : MonoBehaviour
{
    public RawImage rawImage;          // La Raw Image a la que se asignará la textura convertida
    public RenderTexture renderTexture; // Tu RenderTexture (R16G16B16A16_SFLOAT)

    void Start()
    {
        if (rawImage == null || renderTexture == null)
        {
            Debug.LogError("RawImage o RenderTexture no están asignados.");
            return;
        }
    }

    void LateUpdate()
    {
        // Crear una textura intermedia en formato adecuado (RGBA32)
        Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        // Copiar los datos de la RenderTexture a la Texture2D
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = null;

        // Asignar la textura convertida a la Raw Image
        rawImage.texture = texture2D;
    }
}
