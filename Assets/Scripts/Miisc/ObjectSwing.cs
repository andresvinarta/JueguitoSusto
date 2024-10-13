using UnityEngine;

public class ObjectSwing : MonoBehaviour
{
    public GameObject Player;

    private Vector3 StartPos;
    private Vector3 PlayerLastPos;
    public float Amplitude = 0.05f;
    public float Frequency = 5.0f;
    private InputSystem_Actions PlayerInput;


    private void Awake()
    {
        StartPos = this.transform.localPosition;
        PlayerInput = new InputSystem_Actions();
        PlayerInput.Enable();
    }


    private void Update()
    {
        if (Player != null)
        {
            if (PlayerInput.Player.Move.ReadValue<Vector2>().magnitude > 0.15)
            {
                Swing();
                //PlayerLastPos = Player.transform.position;
            }
            else
            {
                transform.localPosition = StartPos;
                //Debug.Log(Player.GetComponent<Rigidbody>().angularVelocity.magnitude.ToString());
                //Debug.Log((Player.transform.position - PlayerLastPos).magnitude.ToString());
            }
        }
    }


    private void Swing()
    {
        float Swing = Mathf.Sin(Time.time * Frequency) * Amplitude;

        Vector3 NewPos = StartPos;
        NewPos.y += Swing;

        transform.localPosition = NewPos;
    }
}
