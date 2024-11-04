using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ObjectSwing : MonoBehaviour
{
    public GameObject Player;

    private Vector3 StartPos;
    private Vector3 PlayerLastPos;
    public float Amplitude = 0.05f;
    public float Frequency = 5.0f;
    private InputSystem_Actions PlayerInput;

    private float TimerOn = 0;

    private float TimerOff = 0;
    public float ReturnDuration = 100f;

    [SerializeField]
    private GameObject PlayerStepsManager;

    private AudioSource[] StepSounds;

    private bool JustStepped;

    private void Awake()
    {
        StartPos = this.transform.localPosition;
        PlayerInput = new InputSystem_Actions();
        PlayerInput.Enable();
        StepSounds = PlayerStepsManager.GetComponents<AudioSource>();
    }


    private void Update()
    {
        if (Player != null)
        {
            if (PlayerInput.Player.Move.ReadValue<Vector2>().magnitude > 0.15)
            {
                Swing();
                TimerOn += Time.deltaTime;
                TimerOff = 0;
                //PlayerLastPos = Player.transform.position;
            }
            else
            {
                if (transform.localPosition != StartPos)
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, StartPos, TimerOff/ReturnDuration);
                    TimerOff += Time.deltaTime;
                    TimerOn = 0;
                    JustStepped = false;
                }
                //Debug.Log(Player.GetComponent<Rigidbody>().angularVelocity.magnitude.ToString());
                //Debug.Log((Player.transform.position - PlayerLastPos).magnitude.ToString());
            }
        }
    }


    private void Swing()
    {
        float Swing = Mathf.Sin(TimerOn * Frequency) * Amplitude;

        Vector3 NewPos = StartPos;
        NewPos.y += Swing;

        if (!JustStepped && Swing <= -Amplitude + 0.015f)
        {
            int RandomNumber = UnityEngine.Random.Range((int)0, (int)StepSounds.Length);
            StepSounds[RandomNumber].Play();
            JustStepped = true;
        }
        else if (Swing > -Amplitude + 0.015f)
        {
            JustStepped = false;
        }

        transform.localPosition = NewPos;
    }
}
