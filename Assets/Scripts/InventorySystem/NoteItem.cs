using UnityEditor;
using UnityEngine;

public class NoteItem : MonoBehaviour
{
    [SerializeField]
    private InventoryManager InventoryManager;

    [SerializeField]
    private string NoteName;

    [SerializeField]
    private Sprite NoteSprite;

    [SerializeField]
    private string NoteText;

    public void Start()
    {
        InventoryManager = GameObject.Find("UICamera").GetComponent<InventoryManager>();
    }

    public void PickUp()
    {
        InventoryManager.AddNote(this.gameObject);
    }

    public string GetNoteName()
    {
        return NoteName;
    }

    public Sprite GetNoteSprite()
    {
        return NoteSprite;
    }

    public string GetNoteText()
    {
        return NoteText;
    }

}
