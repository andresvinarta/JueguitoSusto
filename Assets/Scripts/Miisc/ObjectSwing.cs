using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ObjectSwing : MonoBehaviour
{
    public GameObject Player;

    private Vector3 StartPos;
    public float Amplitude = 0.05f;
    public float Frequency = 5.0f;
    private InputSystem_Actions PlayerInput;

    private float TimerOn = 0;

    private float TimerOff = 0;
    public float ReturnDuration = 100f;

    private AudioSource[] StepSounds;

    [SerializeField]
    private GameObject DirtStepSounds, WoodStepSounds, ConcreteStepSounds;

    public enum GroundType
    {
        Dirt,
        Wood,
        Concrete
    }

    private bool JustStepped;

    public void Awake()
    {
        StartPos = this.transform.localPosition;
        PlayerInput = new InputSystem_Actions();
        PlayerInput.Enable();
        StepSounds = DirtStepSounds.GetComponents<AudioSource>();
    }


    public void Update()
    {
        if (Player != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(Player.transform.position, Vector3.down, out hit, Mathf.Infinity))
            {
                switch (hit.collider.gameObject.tag)
                {
                    case "DirtGround":
                        ChangeGroundType(GroundType.Dirt);
                        break;
                    case "WoodGround":
                        ChangeGroundType(GroundType.Wood);
                        break;
                    case "ConcreteGround":
                        ChangeGroundType(GroundType.Concrete);
                        break;
                    default:
                        ChangeGroundType(GroundType.Dirt);
                        break;
                }
            }

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
            int RandomNumber = Random.Range((int)0, (int)StepSounds.Length);
            StepSounds[RandomNumber].Play();
            JustStepped = true;
        }
        else if (Swing > -Amplitude + 0.015f)
        {
            JustStepped = false;
        }

        transform.localPosition = NewPos;
    }

    public void ChangeGroundType(GroundType NewType)
    {
        switch (NewType) 
        {
            case GroundType.Dirt:
                StepSounds = DirtStepSounds.GetComponents<AudioSource>();
                break;
            case GroundType.Wood:
                StepSounds = WoodStepSounds.GetComponents<AudioSource>();
                break;
            case GroundType.Concrete:
                StepSounds = ConcreteStepSounds.GetComponents<AudioSource>();
                break;
            default:
                break;
        }
    }
}
