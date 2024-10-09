using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;   // Velocidad de movimiento
    private Rigidbody rb;      // Referencia al Rigidbody
    private Vector2 moveInput; // Input de movimiento
    private InputSystem_Actions PlayerInput;
    public GameObject PictureManagerCam;

    private void Awake()
    {
        // Obtener la referencia al Rigidbody
        rb = GetComponent<Rigidbody>();
        PlayerInput = new InputSystem_Actions();
        PlayerInput.Enable();
    }


    private void FixedUpdate()
    {
        // Movemos el personaje basándonos en el input recibido
        moveInput = PlayerInput.Player.Move.ReadValue<Vector2>();
        if (!PictureManagerCam.GetComponent<PictureManager>().IsShowing())
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        // Convertir el input de movimiento en una dirección
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        moveDirection = Vector3.Normalize(transform.forward * moveInput.y + transform.right * moveInput.x);
        // Aplicar la velocidad y el movimiento al Rigidbody
        if (moveDirection != Vector3.zero)
        {
            rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
        }
       
    }
}
