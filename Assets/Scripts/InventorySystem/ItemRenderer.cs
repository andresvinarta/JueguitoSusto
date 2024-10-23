using UnityEngine;
using TMPro;

public class ItemRenderer : MonoBehaviour
{
    public GameObject ItemRendering;
    public TextMeshProUGUI NoteText;

    public void NewItemToRender(GameObject NewItem)
    {
        if (NewItem == null)
        {
            NoteText.text = "No item selected";
        }
        if (ItemRendering != null) 
        { 
            ItemRendering.SetActive(false);
        }
        NoteText.text = "";
        ItemRendering = NewItem;
        NewItem.transform.SetParent(this.transform, false);
        ItemRendering.transform.localScale = new Vector3(200, 200, 200);
        ItemRendering.transform.localRotation = Quaternion.Euler(90, 180, 0);
        ItemRendering.SetActive(true);
    }

    public void NewNoteToRender(GameObject NewNote)
    {
        if (NewNote == null)
        {
            NoteText.text = "No item selected";
        }
        if (ItemRendering != null)
        {
            ItemRendering.SetActive(false);
            ItemRendering = null;
        }
        NoteText.text = NewNote.GetComponent<NoteItem>().GetNoteText();
    }

    public void NoItemSelected()
    {
        if (ItemRendering != null)
        {
            ItemRendering.SetActive(false);
        }
        NoteText.text = "Select an item from your inventory";
    }
}
