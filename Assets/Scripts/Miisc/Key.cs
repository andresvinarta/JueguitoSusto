using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    private int KeyID;


    public int GetKeyID()
    {
        return KeyID;
    }
}
