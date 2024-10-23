using UnityEngine;

public class PictureItem : MonoBehaviour
{
    [SerializeField]
    private InventoryManager InventoryManager;

    [SerializeField]
    private string PictureName = "Picture Nº";

    [SerializeField]
    private Sprite PictureSprite;

    [SerializeField]
    private bool HasJumpScare = false;

    public void Awake()
    {
        InventoryManager = GameObject.Find("UICamera").GetComponent<InventoryManager>();
    }

    public void PickUp()
    {
        InventoryManager.AddPicture(this.gameObject);
    }

    public string GetPictureName()
    {
        return PictureName;
    }

    public void SetPictureNumber(int Number)
    {
        PictureName += Number.ToString();
    }

    public Sprite GetPictureSprite()
    {
        return PictureSprite;
    }

    public bool DoesItHaveJumpScare()
    {
        return HasJumpScare;
    }
}
