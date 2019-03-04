using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float x;
    private float z;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal") * speed;
        z = Input.GetAxisRaw("Vertical") * speed;

        transform.position = transform.position + new Vector3(x, 0, z);
    }
}
