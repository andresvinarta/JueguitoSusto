using UnityEngine;

public class PictureJumpscareDetection : InCameraDetect
{
    public bool DestroyOnPictureTaken = true;

    public override void DetectedByCamera()
    {
        PolaroidCamera Polaroid = GameObject.Find("PolaroidCamera").GetComponent<PolaroidCamera>();
        if (Polaroid != null) Polaroid.ActivateJumpscare(this);
    }

    public override void UndetectedByCamera()
    {
        PolaroidCamera Polaroid = GameObject.Find("PolaroidCamera").GetComponent<PolaroidCamera>();
        if (Polaroid != null) Polaroid.DeactivateJumpscare();
    }

    public override void PictureOfObjectTaken()
    {
        //La cámara polaroid avisa en este caso.
    }

    public void DestroyAfterPicture()
    {
        Destroy(gameObject);
    }
}
