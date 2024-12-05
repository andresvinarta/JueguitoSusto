using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class DisableFogForCamera : MonoBehaviour
{
    [SerializeField]
    private string CameraName;

    // Unity calls this method automatically when it enables this component
    private void OnEnable()
    {
        // Add WriteLogMessage as a delegate of the RenderPipelineManager.beginCameraRendering event
        RenderPipelineManager.beginCameraRendering += BeginRender;
        RenderPipelineManager.endCameraRendering += EndRender;
    }

    // Unity calls this method automatically when it disables this component
    private void OnDisable()
    {
        // Remove WriteLogMessage as a delegate of the  RenderPipelineManager.beginCameraRendering event
        RenderPipelineManager.beginCameraRendering -= BeginRender;
        RenderPipelineManager.endCameraRendering -= EndRender;
    }

    // When this method is a delegate of RenderPipeline.beginCameraRendering event, Unity calls this method every time it raises the beginCameraRendering event
    void BeginRender(ScriptableRenderContext context, Camera camera)
    {
        // Write text to the console
        //Debug.Log($"Beginning rendering the camera: {camera.name}");

        if (camera.gameObject.name == CameraName)
        {
            //Debug.Log("Turn fog off");
            RenderSettings.fog = false;
        }

    }

    void EndRender(ScriptableRenderContext context, Camera camera)
    {
        //Debug.Log($"Ending rendering the camera: {camera.name}");
        if (camera.gameObject.name == CameraName)
        {
            //Debug.Log("Turn fog on");
            RenderSettings.fog = true;
        }
    }
}