using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PictureManager : MonoBehaviour
{
    public List<GameObject> Pictures;
    public GameObject SpawnPoint;
    private InputSystem_Actions PlayerInput;
    private int CurrentPicture;
    private bool Showing;

    private void Awake()
    {
        PlayerInput = new InputSystem_Actions();
        PlayerInput.Enable();
    }

    public void AddPicture(GameObject Picture)
    {
        Pictures.Add(Picture);
    }

    public void SetUp()
    {
        CurrentPicture = 0;
        Pictures[CurrentPicture].transform.position = SpawnPoint.transform.position;
        Pictures[CurrentPicture].SetActive(true);
    }

    public void CloseUp()
    {
        Pictures[CurrentPicture].SetActive(false);
    }

    public void ChangePicture(bool NextPrev)
    {
        if (NextPrev)
        {
            if (CurrentPicture < Pictures.Count - 1)
            {
                Pictures[CurrentPicture].SetActive(false);
                CurrentPicture += 1;
                Pictures[CurrentPicture].transform.position = SpawnPoint.transform.position;
                Pictures[CurrentPicture].SetActive(true);
            }
        }
        else
        {
            if (CurrentPicture > 0)
            {
                Pictures[CurrentPicture].SetActive(false);
                CurrentPicture -= 1;
                Pictures[CurrentPicture].transform.position = SpawnPoint.transform.position;
                Pictures[CurrentPicture].SetActive(true);
            }
        }
    }

}
