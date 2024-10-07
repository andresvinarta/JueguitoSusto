using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;   // Velocidad de movimiento
    private Rigidbody rb;      // Referencia al Rigidbody
    private Vector2 moveInput; // Input de movimiento
    private InputSystem_Actions PlayerInput;

    private void Awake()
    {
        // Obtener la referencia al Rigidbody
        rb = GetComponent<Rigidbody>();
        PlayerInput = new InputSystem_Actions();
        PlayerInput.Enable();
    }


    private void FixedUpdate()
    {
        // Movemos el personaje bas�ndonos en el input recibido
        moveInput = PlayerInput.Player.Move.ReadValue<Vector2>();
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Convertir el input de movimiento en una direcci�n
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        // Aplicar la velocidad y el movimiento al Rigidbody
        if (moveDirection != Vector3.zero)
        {
            rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
        }
       
    }
}
