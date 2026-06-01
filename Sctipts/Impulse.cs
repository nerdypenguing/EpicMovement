using UnityEngine;

public class Impulse : MonoBehaviour
{

    public Transform player;

    public float Intensity;



    private Rigidbody rb;

    void Start()
    {
        GetComponent<Rigidbody>();
    }

    void Update()
    {

    }
}
