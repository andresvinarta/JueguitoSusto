using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;        // Referencia al transform del jugador
    public float mouseSensitivity = 100f;  // Sensibilidad del rat�n o gamepad
    public float minYAngle = -60f;  // �ngulo m�nimo en el eje Y
    public float maxYAngle = 60f;   // �ngulo m�ximo en el eje Y

    private Vector2 lookInput;      // Input de rotaci�n
    private float xRotation = 0f;   // Rotaci�n actual en el eje X

    private InputSystem_Actions PlayerInput;

    public GameObject InventoryManager, PauseMenu;

    private void Start()
    {
        // Bloquear el cursor al iniciar el juego (opcional)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        PlayerInput = new InputSystem_Actions();
        PlayerInput.Enable();
    }

    private void Update()
    {
        // Rotar la c�mara
        lookInput = PlayerInput.Player.Look.ReadValue<Vector2>();
        if (!InventoryManager.GetComponent<InventoryManager>().IsInventoryShowing() && !PauseMenu.GetComponent<PauseMenuManager>().IsPauseMenuActive())
        {
            RotateCamera();
        }
    }

    private void RotateCamera()
    {
        // Calcular la rotaci�n del eje X y Y bas�ndonos en el input y la sensibilidad
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        // Actualizar la rotaci�n en el eje X (vertical) y limitar el �ngulo
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minYAngle, maxYAngle);

        // Aplicar la rotaci�n de la c�mara en el eje X
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // Rotar el jugador en el eje Y (horizontal) para que siga al rat�n
        player.Rotate(Vector3.up * mouseX);
    }
}
