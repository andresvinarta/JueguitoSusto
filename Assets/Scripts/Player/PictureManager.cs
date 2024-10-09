using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;

public class PictureManager : MonoBehaviour
{
    public List<GameObject> Pictures;
    public GameObject PictureCanvas, SpawnPoint;
    public TMP_Text PicNumText;
    private InputSystem_Actions PlayerInput;
    private int CurrentPicture;
    private bool Showing = false;
    public bool PictureJustTaken = false;

    private void Awake()
    {
        PictureCanvas.SetActive(false);
        PlayerInput = new InputSystem_Actions();
        PlayerInput.Player.Next.performed += NextPicture;
        PlayerInput.Player.Previous.performed += PreviousPicture;
        PlayerInput.Player.Previous.Disable();
        PlayerInput.Player.Next.Disable();
        CurrentPicture = 0;
    }

    public void AddPicture(GameObject Picture)
    {
        Pictures.Add(Picture);
        Picture.transform.SetParent(PictureCanvas.transform);
    }

    public void SetUp()
    {
        PlayerInput.Player.Move.Disable();
        PlayerInput.Player.Look.Disable();
        PlayerInput.Player.Previous.Enable();
        PlayerInput.Player.Next.Enable();
        if (PictureJustTaken)
        {
            CurrentPicture = Pictures.Count - 1;
            PictureJustTaken = false;
        }
        Pictures[CurrentPicture].transform.position = SpawnPoint.transform.position;
        Pictures[CurrentPicture].SetActive(true);
        PicNumText.SetText((CurrentPicture + 1).ToString() + " / " + Pictures.Count.ToString());
        PictureCanvas.SetActive(true);
        Showing = true;
    }

    public void CloseUp()
    {
        Pictures[CurrentPicture].SetActive(false);
        PlayerInput.Player.Previous.Disable();
        PlayerInput.Player.Next.Disable();
        PlayerInput.Player.Move.Enable();
        PlayerInput.Player.Look.Enable();
        PictureCanvas.SetActive(false);
        Showing = false;
    }


    public void NextPicture(InputAction.CallbackContext CallbackContext)
    {
        if (CurrentPicture < Pictures.Count - 1)
        {
            Pictures[CurrentPicture].SetActive(false);
            CurrentPicture += 1;
            Pictures[CurrentPicture].transform.position = SpawnPoint.transform.position;
            Pictures[CurrentPicture].SetActive(true);
            PicNumText.SetText((CurrentPicture + 1).ToString() + " / " + Pictures.Count.ToString());
        }
    }

    public void PreviousPicture(InputAction.CallbackContext CallbackContext)
    {
        if (CurrentPicture > 0)
        {
            Pictures[CurrentPicture].SetActive(false);
            CurrentPicture -= 1;
            Pictures[CurrentPicture].transform.position = SpawnPoint.transform.position;
            Pictures[CurrentPicture].SetActive(true);
            PicNumText.SetText((CurrentPicture + 1).ToString() + " / " + Pictures.Count.ToString());
        }
    }

    public bool IsShowing() { return Showing; }

    public bool ThereArePictures()
    {
        return Pictures.Count > 0;
    }

    public void JustTookAPicture()
    {
        PictureJustTaken = true;
    }
}
