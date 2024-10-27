using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ItemRenderer : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject ItemRendering, RotationReference;
    public TextMeshProUGUI NoteText;

    // Sensibilidad de la rotaci�n
    public float rotationSpeed = 0.2f;

    // Controla si el rat�n est� siendo arrastrado
    private bool isDragging = false;

    // �ltima posici�n del rat�n
    private Vector2 lastMousePosition;

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
        ItemRendering.transform.localRotation = Quaternion.Euler(0, 180, 0);
        if (NewItem.tag != "Picture")
        {
            ItemRendering.layer = LayerMask.NameToLayer("ItemRenderUI");
            Transform[] Transforms = NewItem.GetComponentsInChildren<Transform>();
            foreach (Transform ItemTransform in Transforms)
            {
                ItemTransform.gameObject.layer = LayerMask.NameToLayer("ItemRenderUI");
            }
        }
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

    // Detecta cuando el rat�n hace clic sobre el panel
    public void OnPointerDown(PointerEventData eventData)
    {
        if (ItemRendering != null && ItemRendering.activeSelf)
        {
            isDragging = true;
            lastMousePosition = eventData.position;
        }
        
    }

    // Detecta cuando arrastramos el rat�n
    public void OnDrag(PointerEventData eventData)
    {
        if (ItemRendering != null && ItemRendering.activeSelf)
        {
            if (isDragging)
            {
                // Diferencia entre la posici�n actual y la �ltima posici�n del rat�n
                Vector2 currentMousePosition = eventData.position;
                Vector2 delta = currentMousePosition - lastMousePosition;

                // Aplicamos la rotaci�n en funci�n del movimiento del rat�n
                float rotationX = delta.y * rotationSpeed; // Movimiento vertical rota en el eje X
                float rotationY = -delta.x * rotationSpeed; // Movimiento horizontal rota en el eje Y

                // Rotamos el objeto
                ItemRendering.transform.RotateAround(RotationReference.transform.position, RotationReference.transform.up, rotationY);
                ItemRendering.transform.RotateAround(RotationReference.transform.position, RotationReference.transform.right, rotationX);

                // Actualizamos la posici�n del rat�n para el siguiente frame
                lastMousePosition = currentMousePosition;
            }
        }
    }

    // Detecta cuando soltamos el clic del rat�n
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}
