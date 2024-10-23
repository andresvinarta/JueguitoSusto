using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ItemRenderer : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject ItemRendering;
    public TextMeshProUGUI NoteText;

    // Sensibilidad de la rotación
    public float rotationSpeed = 0.2f;

    // Controla si el ratón está siendo arrastrado
    private bool isDragging = false;

    // Última posición del ratón
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

    // Detecta cuando el ratón hace clic sobre el panel
    public void OnPointerDown(PointerEventData eventData)
    {
        if (ItemRendering != null && ItemRendering.activeSelf)
        {
            isDragging = true;
            lastMousePosition = eventData.position;
        }
        
    }

    // Detecta cuando arrastramos el ratón
    public void OnDrag(PointerEventData eventData)
    {
        if (ItemRendering != null && ItemRendering.activeSelf)
        {
            if (isDragging)
            {
                // Diferencia entre la posición actual y la última posición del ratón
                Vector2 currentMousePosition = eventData.position;
                Vector2 delta = currentMousePosition - lastMousePosition;

                // Aplicamos la rotación en función del movimiento del ratón
                float rotationX = delta.y * rotationSpeed; // Movimiento vertical rota en el eje X
                float rotationY = -delta.x * rotationSpeed; // Movimiento horizontal rota en el eje Y

                // Rotamos el objeto
                ItemRendering.transform.Rotate(Vector3.up, rotationY, Space.World);
                ItemRendering.transform.Rotate(Vector3.right, rotationX, Space.World);

                // Actualizamos la posición del ratón para el siguiente frame
                lastMousePosition = currentMousePosition;
            }
        }
    }

    // Detecta cuando soltamos el clic del ratón
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}
