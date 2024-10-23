using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{

    public enum TypeShowing
    {
        Pictures,
        Objects,
        Notes
    }

    public GameObject InventoryCanvas;

    public TypeShowing CurrentTypeShowing;

    private bool IsShowing;

    private List<GameObject> Pictures, Objects, Notes;

    [SerializeField]
    private List<GameObject> ItemSlots;

    private GameObject CurrentSelectedSlot;

    private int CurrentPage = 0;

    private int NumPicturePages = 0;

    private int NumObjectPages = 0;

    private int NumNotePages = 0;

    [SerializeField]
    private TextMeshProUGUI CurrentPageText, CurrentTypeText;

    private void Awake()
    {
        InventoryCanvas.SetActive(false);
        IsShowing = false;
        Pictures = new List<GameObject>();
        Objects = new List<GameObject>();
        Notes = new List<GameObject>();
    }

    public void OpenInventory()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        InventoryCanvas.SetActive(true);
        CurrentPage = 0;
        NumPicturePages = (Pictures.Count + ItemSlots.Count - 1) / ItemSlots.Count;
        NumObjectPages = (Objects.Count + ItemSlots.Count - 1) / ItemSlots.Count;
        NumNotePages = (Notes.Count + ItemSlots.Count - 1) / ItemSlots.Count;
        switch (CurrentTypeShowing)
        {
            case TypeShowing.Pictures:
                LoadPictures(0); //Change for CurrentPage if I want to show what was las seen. Add if to show most recent pic if pic was just taken
                ItemSlots[0].GetComponent<ItemSlot>().ItemSelected();
                break;
            case TypeShowing.Objects:
                LoadObjects(0);
                ItemSlots[0].GetComponent<ItemSlot>().ItemSelected();
                break;
            case TypeShowing.Notes:
                LoadNotes(0);
                ItemSlots[0].GetComponent<ItemSlot>().ItemSelected();
                break;
            default:
                break;
        }
        IsShowing = true;
    }

    public void CloseInventory()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InventoryCanvas.SetActive(false);
        IsShowing = false;
    }

    public bool IsInventoryShowing()
    {
        return IsShowing;
    }

    public void AddPicture(GameObject PictureToAdd)
    {
        if (PictureToAdd != null)
        {
            Pictures.Insert(0, PictureToAdd);
        }
    }

    public void RemovePicture(GameObject PictureToRemove)
    {
        if (PictureToRemove != null && Pictures.Contains(PictureToRemove))
        {
            Pictures.Remove(PictureToRemove);
        }
    }

    public int GetNumberOfPictures()
    {
        return Pictures.Count;
    }

    public void AddObject(GameObject ObjectToAdd)
    {
        if (ObjectToAdd != null && !Objects.Contains(ObjectToAdd))
        {
            Objects.Insert(0, ObjectToAdd);
        }
    }

    public void RemoveObject(GameObject ObjectToRemove)
    {
        if (ObjectToRemove != null && Objects.Contains(ObjectToRemove))
        {
            Objects.Remove(ObjectToRemove);
        }
    }

    public void AddNote(GameObject NoteToAdd)
    {
        if (NoteToAdd != null && !Notes.Contains(NoteToAdd))
        {
            Notes.Insert(0, NoteToAdd);
        }
    }

    public void RemoveNote(GameObject NoteToRemove)
    {
        if (NoteToRemove != null && Notes.Contains(NoteToRemove))
        {
            Notes.Remove(NoteToRemove);
        }
    }

    private void LoadPictures(int Page)
    {
        int ItemToLoad = Page * ItemSlots.Count;
        int i = 0;
        for (; i < ItemSlots.Count && ItemToLoad < Pictures.Count; i++)
        {
            ItemSlots[i].GetComponent<ItemSlot>().LoadItemOnSlot(Pictures[ItemToLoad], ItemSlot.ItemType.Picture);
            ItemToLoad++;
        }
        if (i < ItemSlots.Count)
        {
            for (; i < ItemSlots.Count; i++)
            {
                ItemSlots[i].GetComponent<ItemSlot>().UnloadItemOnSlot();
            }
        }
        ChangePangeText(NumPicturePages);
    }

    private void LoadObjects(int Page)
    {
        int ItemToLoad = Page * ItemSlots.Count;
        int i = 0;
        for (; i < ItemSlots.Count && ItemToLoad < Objects.Count; i++)
        {
            ItemSlots[i].GetComponent<ItemSlot>().LoadItemOnSlot(Objects[ItemToLoad], ItemSlot.ItemType.Object);
            ItemToLoad++;
        }
        if (i < ItemSlots.Count)
        {
            for (; i < ItemSlots.Count; i++)
            {
                ItemSlots[i].GetComponent<ItemSlot>().UnloadItemOnSlot();
            }
        }
        ChangePangeText(NumObjectPages);
    }

    private void LoadNotes(int Page)
    {
        int ItemToLoad = Page * ItemSlots.Count;
        int i = 0;
        for (; i < ItemSlots.Count && ItemToLoad < Notes.Count; i++)
        {
            ItemSlots[i].GetComponent<ItemSlot>().LoadItemOnSlot(Notes[ItemToLoad], ItemSlot.ItemType.Note);
            ItemToLoad++;
        }
        if (i < ItemSlots.Count)
        {
            for (; i < ItemSlots.Count; i++)
            {
                ItemSlots[i].GetComponent<ItemSlot>().UnloadItemOnSlot();
            }
        }
        ChangePangeText(NumNotePages);
    }

    private void UnloadAllItems()
    {
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            ItemSlots[i].GetComponent<ItemSlot>().UnloadItemOnSlot();
        }
    }
    
    public void NewSlotSelected(GameObject NewCurrentSlot)
    {
        if (CurrentSelectedSlot != null)
        {
            CurrentSelectedSlot.GetComponent<ItemSlot>().ItemUnselected();
        }
        CurrentSelectedSlot = NewCurrentSlot;
    }

    public void NextPage()
    {
        
        switch (CurrentTypeShowing)
        {
            case TypeShowing.Pictures:
                if (CurrentPage < NumPicturePages - 1)
                {
                    CurrentPage++;
                    LoadPictures(CurrentPage); 
                    ItemSlots[0].GetComponent<ItemSlot>().ItemSelected();
                }
                break;
            case TypeShowing.Objects:
                if (CurrentPage < NumObjectPages - 1)
                {
                    CurrentPage++;
                    LoadObjects(CurrentPage);
                    ItemSlots[0].GetComponent<ItemSlot>().ItemSelected();
                }
                break;
            case TypeShowing.Notes:
                if (CurrentPage < NumNotePages - 1)
                {
                    CurrentPage++;
                    LoadNotes(CurrentPage);
                    ItemSlots[0].GetComponent<ItemSlot>().ItemSelected();
                }
                break;
            default:
                break;
        }
    }

    public void PreviousPage()
    {

        switch (CurrentTypeShowing)
        {
            case TypeShowing.Pictures:
                if (CurrentPage > 0)
                {
                    CurrentPage--;
                    LoadPictures(CurrentPage);
                    ItemSlots[0].GetComponent<ItemSlot>().ItemSelected();
                }
                break;
            case TypeShowing.Objects:
                if (CurrentPage > 0)
                {
                    CurrentPage--;
                    LoadObjects(CurrentPage);
                    ItemSlots[0].GetComponent<ItemSlot>().ItemSelected();
                }
                break;
            case TypeShowing.Notes:
                if (CurrentPage > 0)
                {
                    CurrentPage--;
                    LoadNotes(CurrentPage);
                    ItemSlots[0].GetComponent<ItemSlot>().ItemSelected();
                }
                break;
            default:
                break;
        }
    }

    private void ChangePangeText(int MaxPages)
    {
        if (MaxPages > 0)
        {
            CurrentPageText.text = (CurrentPage + 1).ToString() + " / " + MaxPages.ToString();
        }
        else
        {
            CurrentPageText.text = "0 / 0";
        }
    }

    public void ChangeToPictures()
    {
        CurrentTypeShowing = TypeShowing.Pictures;
        CurrentPage = 0;
        LoadPictures(CurrentPage);
        ItemSlots[0].GetComponent<ItemSlot>().ItemSelected();
        CurrentTypeText.text = "Pictures";
    }

    public void ChangeToObjects() 
    {  
        CurrentTypeShowing = TypeShowing.Objects;
        CurrentPage = 0;
        LoadObjects(CurrentPage);
        ItemSlots[0].GetComponent<ItemSlot>().ItemSelected();
        CurrentTypeText.text = "Objects";
    }

    public void ChangeToNotes()
    {
        CurrentTypeShowing = TypeShowing.Notes;
        CurrentPage = 0;
        LoadNotes(CurrentPage);
        ItemSlots[0].GetComponent<ItemSlot>().ItemSelected();
        CurrentTypeText.text = "Notes";
    }
}
