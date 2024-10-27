using UnityEngine;

public class ObjectItem : MonoBehaviour
{

    [SerializeField]
    private InventoryManager InventoryManager;

    [SerializeField]
    private string ObjectName;

    [SerializeField]
    private Sprite ObjectSprite;

    public void Start()
    {
        InventoryManager = GameObject.Find("UICamera").GetComponent<InventoryManager>();
    }

    public void PickUp()
    {
        InventoryManager.AddObject(this.gameObject);
        this.gameObject.SetActive(false);
    }

    public string GetObjectName()
    {
        return ObjectName;
    }

    public Sprite GetObjectSprite()
    {
        return ObjectSprite;
    }

}
