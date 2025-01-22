using UnityEngine;

public class PolaroidPreview : MonoBehaviour
{
    [HideInInspector]
    public bool IsShowing = true;

    public RenderTexture RenderText;

    private void LateUpdate()
    {
        if (IsShowing)
        {
            GetComponent<Renderer>().material.mainTexture = RenderText;
        }
    }
}
