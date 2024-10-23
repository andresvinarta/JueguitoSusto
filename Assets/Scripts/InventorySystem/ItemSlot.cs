using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public enum ItemType
    {
        Picture,
        Object,
        Note
    }

    [SerializeField]
    private InventoryManager InventoryManager;

    [SerializeField]
    private GameObject Item;

    [SerializeField]
    private Image SlotImage;

    [SerializeField]
    private ItemType Type;

    [SerializeField]
    private GameObject ItemRenderPanel;

    [SerializeField]
    private TextMeshProUGUI ItemNameInPanel;

    [SerializeField]
    private GameObject SlotSelectedPanel;

    public void LoadItemOnSlot(GameObject Item, ItemType Type)
    {
        this.Item = Item;
        this.Type = Type;

        ShowItemInSlot();
    }

    public void UnloadItemOnSlot()
    {
        if (Item != null)
        {
            Item = null;
            SlotImage.sprite = null;
            SlotImage.gameObject.SetActive(false);
        }
    }

    private void ShowItemInSlot()
    {
        if (Item != null)
        {
            switch (Type)
            {
                case ItemType.Picture:
                    SlotImage.sprite = Item.GetComponent<PictureItem>().GetPictureSprite();
                    SlotImage.gameObject.SetActive(true);
                    break;
                case ItemType.Object:
                    SlotImage.sprite = Item.GetComponent<ObjectItem>().GetObjectSprite();
                    SlotImage.gameObject.SetActive(true);
                    break;
                case ItemType.Note:
                    SlotImage.sprite = Item.GetComponent<NoteItem>().GetNoteSprite();
                    SlotImage.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

    public void ItemSelected()
    {
        if (this.Item != null)
        {
            InventoryManager.NewSlotSelected(this.gameObject);
            SlotSelectedPanel.SetActive(true);
            switch (Type)
            {
                case ItemType.Picture:
                    ItemNameInPanel.text = Item.GetComponent<PictureItem>().GetPictureName();
                    ItemRenderPanel.GetComponent<ItemRenderer>().NewItemToRender(Item);
                    break;
                case ItemType.Object:
                    ItemNameInPanel.text = Item.GetComponent<ObjectItem>().GetObjectName();
                    ItemRenderPanel.GetComponent<ItemRenderer>().NewItemToRender(Item);
                    break;
                case ItemType.Note:
                    ItemNameInPanel.text = Item.GetComponent<NoteItem>().GetNoteName();
                    ItemRenderPanel.GetComponent<ItemRenderer>().NewNoteToRender(Item);
                    //ItemRenderPanel.transform.SetParent(Item.transform, false);
                    break;
                default:
                    break;
            }
        }
        else
        {
            InventoryManager.NewSlotSelected(this.gameObject);
            ItemNameInPanel.text = "No Item Selected";
            ItemRenderPanel.GetComponent<ItemRenderer>().NoItemSelected();
        }
    }

    public void ItemUnselected()
    {
        SlotSelectedPanel.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
        {
            ItemSelected();
        }
    }
}
