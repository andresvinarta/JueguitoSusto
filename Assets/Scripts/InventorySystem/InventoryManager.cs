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

    private int CurrentPictureSelectedSlot = 0;

    private int CurrentObjectSelectedSlot = 0;

    private int CurrentNoteSelectedSlot = 0;

    private int CurrentPicturePage = 0;

    private int CurrentObjectPage = 0;

    private int CurrentNotePage = 0;

    private int NumPicturePages = 0;

    private int NumObjectPages = 0;

    private int NumNotePages = 0;

    private bool PictureJustTaken = false;

    private bool ObjectJustPicked = false;

    private bool NoteJustPicked = false;

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
        //CurrentPage = 0;
        NumPicturePages = (Pictures.Count + ItemSlots.Count - 1) / ItemSlots.Count;
        NumObjectPages = (Objects.Count + ItemSlots.Count - 1) / ItemSlots.Count;
        NumNotePages = (Notes.Count + ItemSlots.Count - 1) / ItemSlots.Count;

        if (NoteJustPicked)
        {
            CurrentNotePage = 0;
            ItemSlots[CurrentNoteSelectedSlot].GetComponent<ItemSlot>().ItemUnselected();
            CurrentNoteSelectedSlot = 0;
            CurrentTypeShowing = TypeShowing.Notes;
            CurrentTypeText.text = "Notes";
            NoteJustPicked = false;
        }
        if (ObjectJustPicked)
        {
            CurrentObjectPage = 0;
            ItemSlots[CurrentObjectSelectedSlot].GetComponent<ItemSlot>().ItemUnselected();
            CurrentObjectSelectedSlot = 0;
            CurrentTypeShowing = TypeShowing.Objects;
            CurrentTypeText.text = "Objects";
            ObjectJustPicked = false;
        }
        if (PictureJustTaken)
        {
            CurrentPicturePage = 0;
            ItemSlots[CurrentPictureSelectedSlot].GetComponent<ItemSlot>().ItemUnselected();
            CurrentPictureSelectedSlot = 0;
            CurrentTypeShowing = TypeShowing.Pictures;
            CurrentTypeText.text = "Pictures";
            PictureJustTaken = false;
        }
        switch (CurrentTypeShowing)
        {
            case TypeShowing.Pictures:
                LoadPictures(CurrentPicturePage); //Change for CurrentPage if I want to show what was last seen. Add if to show most recent pic if pic was just taken
                ItemSlots[CurrentPictureSelectedSlot].GetComponent<ItemSlot>().ItemSelected();
                break;
            case TypeShowing.Objects:
                LoadObjects(CurrentObjectPage);
                ItemSlots[CurrentObjectSelectedSlot].GetComponent<ItemSlot>().ItemSelected();
                break;
            case TypeShowing.Notes:
                LoadNotes(CurrentNotePage);
                ItemSlots[CurrentNoteSelectedSlot].GetComponent<ItemSlot>().ItemSelected();
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
            PictureJustTaken = true;
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
            ObjectJustPicked = true;
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
            NoteJustPicked = true;
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
        ChangePageText(NumPicturePages);
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
        ChangePageText(NumObjectPages);
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
        ChangePageText(NumNotePages);
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
        //if (CurrentSelectedSlot != null)
        //{
        //    CurrentSelectedSlot.GetComponent<ItemSlot>().ItemUnselected();
        //}
        //CurrentSelectedSlot = NewCurrentSlot;

        ItemSlots[CurrentPictureSelectedSlot].GetComponent<ItemSlot>().ItemUnselected();
        ItemSlots[CurrentObjectSelectedSlot].GetComponent<ItemSlot>().ItemUnselected();
        ItemSlots[CurrentNoteSelectedSlot].GetComponent<ItemSlot>().ItemUnselected();

        switch (CurrentTypeShowing)
        {
            case TypeShowing.Pictures:
                CurrentPictureSelectedSlot = ItemSlots.IndexOf(NewCurrentSlot);
                break;
            case TypeShowing.Objects:
                CurrentObjectSelectedSlot = ItemSlots.IndexOf(NewCurrentSlot);
                break;
            case TypeShowing.Notes:
                CurrentNoteSelectedSlot = ItemSlots.IndexOf(NewCurrentSlot);
                break;
            default:
                break;
        }
    }

    public void NextPage()
    {
        
        switch (CurrentTypeShowing)
        {
            case TypeShowing.Pictures:
                if (CurrentPicturePage < NumPicturePages - 1)
                {
                    CurrentPicturePage++;
                    LoadPictures(CurrentPicturePage); 
                    ItemSlots[0].GetComponent<ItemSlot>().ItemSelected();
                }
                break;
            case TypeShowing.Objects:
                if (CurrentObjectPage < NumObjectPages - 1)
                {
                    CurrentObjectPage++;
                    LoadObjects(CurrentObjectPage);
                    ItemSlots[0].GetComponent<ItemSlot>().ItemSelected();
                }
                break;
            case TypeShowing.Notes:
                if (CurrentNotePage < NumNotePages - 1)
                {
                    CurrentNotePage++;
                    LoadNotes(CurrentNotePage);
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
                if (CurrentPicturePage > 0)
                {
                    CurrentPicturePage--;
                    LoadPictures(CurrentPicturePage);
                    ItemSlots[0].GetComponent<ItemSlot>().ItemSelected();
                }
                break;
            case TypeShowing.Objects:
                if (CurrentObjectPage > 0)
                {
                    CurrentObjectPage--;
                    LoadObjects(CurrentObjectPage);
                    ItemSlots[0].GetComponent<ItemSlot>().ItemSelected();
                }
                break;
            case TypeShowing.Notes:
                if (CurrentNotePage > 0)
                {
                    CurrentPicturePage--;
                    LoadNotes(CurrentPicturePage);
                    ItemSlots[0].GetComponent<ItemSlot>().ItemSelected();
                }
                break;
            default:
                break;
        }
    }

    private void ChangePageText(int MaxPages)
    {

        switch (CurrentTypeShowing)
        {
            case TypeShowing.Pictures:
                if (MaxPages > 0)
                {
                    CurrentPageText.text = (CurrentPicturePage + 1).ToString() + " / " + MaxPages.ToString();
                }
                break;
            case TypeShowing.Objects:
                if (MaxPages > 0)
                {
                    CurrentPageText.text = (CurrentObjectPage + 1).ToString() + " / " + MaxPages.ToString();
                }
                break;
            case TypeShowing.Notes:
                if (MaxPages > 0)
                {
                    CurrentPageText.text = (CurrentNotePage + 1).ToString() + " / " + MaxPages.ToString();
                }
                break;
            default:
                break;
        }
        if (MaxPages <= 0)
        {
            CurrentPageText.text = "0 / 0";
        }
    }

    public void ChangeToPictures()
    {
        CurrentTypeShowing = TypeShowing.Pictures;
        //CurrentPicturePage = 0;
        LoadPictures(CurrentPicturePage);
        ItemSlots[CurrentPictureSelectedSlot].GetComponent<ItemSlot>().ItemSelected();
        CurrentTypeText.text = "Pictures";
    }

    public void ChangeToObjects() 
    {  
        CurrentTypeShowing = TypeShowing.Objects;
        //CurrentObjectPage = 0;
        LoadObjects(CurrentObjectPage);
        ItemSlots[CurrentObjectSelectedSlot].GetComponent<ItemSlot>().ItemSelected();
        CurrentTypeText.text = "Objects";
    }

    public void ChangeToNotes()
    {
        CurrentTypeShowing = TypeShowing.Notes;
        //CurrentNotePage = 0;
        LoadNotes(CurrentNotePage);
        ItemSlots[CurrentNoteSelectedSlot].GetComponent<ItemSlot>().ItemSelected();
        CurrentTypeText.text = "Notes";
    }

    public bool HaveKeyWithID(int ID)
    {
        foreach (GameObject Object in Objects)
        {
            if (Object.tag == "Key" && Object.GetComponent<Key>().GetKeyID() == ID)
            {
                return true;
            }
        }
        return false;
    }
}
