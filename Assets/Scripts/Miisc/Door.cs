using UnityEngine;
using UnityEngine.Rendering;

public class Door : MonoBehaviour
{
    [SerializeField]
    private bool InvertedDirection = false;

    [SerializeField]
    private Transform RotationPoint;

    [SerializeField]
    private int DoorID;

    private bool IsOpen, IsSwinging;

    [SerializeField]
    private bool IsLocked;

    [SerializeField]
    private float TimeToSwing = 0.5f;

    private float TimePassed = 0, DegreesToRotate = 0, PrevAnlge = 0;

    private void Update()
    {
        if (IsSwinging)
        {
            if (TimePassed < TimeToSwing)
            {
                // Progresivamente calcular cuánto hemos avanzado en el tiempo
                TimePassed += Time.deltaTime;

                // Calcular el porcentaje de tiempo transcurrido
                float t = Mathf.Clamp01(TimePassed / TimeToSwing);

                // Calcular el ángulo actual usando Lerp
                float CurrentAngle = Mathf.Lerp(0, DegreesToRotate, t);

                float AngleDelta = CurrentAngle - PrevAnlge;

                PrevAnlge = CurrentAngle;

                // Restablecer rotación del objeto y rotarlo con RotateAround
                transform.RotateAround(RotationPoint.transform.position, RotationPoint.transform.up, AngleDelta);
            }
            else
            {
                TimePassed = 0; 
                DegreesToRotate = 0;
                PrevAnlge = 0;
                IsSwinging = false;
            }
        }
    }

    public void OpenDoor()
    {
        if (!IsSwinging && !IsOpen && RotationPoint != null)
        {
            if (InvertedDirection)
            {
                //transform.RotateAround(RotationPoint.transform.position, RotationPoint.transform.up, -90f);
                IsSwinging = true;
                DegreesToRotate = -90f;
            }
            else
            {
                //transform.RotateAround(RotationPoint.transform.position, RotationPoint.transform.up, 90f);
                IsSwinging = true;
                DegreesToRotate = 90f;
            }
            IsOpen = true;
        }
    }

    public void CloseDoor()
    {
        if (!IsSwinging && IsOpen && RotationPoint != null)
        {
            if (InvertedDirection)
            {
                //transform.RotateAround(RotationPoint.transform.position, RotationPoint.transform.up, 90f);
                IsSwinging = true;
                DegreesToRotate = 90f;
            }
            else
            {
                //transform.RotateAround(RotationPoint.transform.position, RotationPoint.transform.up, -90f);
                IsSwinging = true;
                DegreesToRotate = -90f;
            }
            IsOpen = false;
        }
    }

    public bool IsDoorOpen()
    {
        return IsOpen;
    }

    public bool IsDoorLocked()
    {
        return IsLocked;
    }

    public bool IsDoorSwinging()
    {
        return IsSwinging;
    }

    public void UnlockDoor()
    {
        if (IsLocked)
        {
            IsLocked = false;
        }
    }

    public void FailedToUnlock()
    {
        if (IsLocked)
        {
            //Sonido de intentar forzar a que se abra pero no cede.
        }
    }

    public int GetDoorID()
    {
        return DoorID;
    }
}
