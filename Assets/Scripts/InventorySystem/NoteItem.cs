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
        InventoryManager = GameObject.Find("UI").GetComponent<InventoryManager>();
    }

    public void PickUp()
    {
        InventoryManager.AddNote(this.gameObject);
        this.gameObject.SetActive(false);
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
