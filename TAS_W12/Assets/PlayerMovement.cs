using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    private Vector3 movement;
    private float x;
    private float z;
    private Vector3 relativePos;

    private float timeCount = 0f;
   
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal") * speed;
        z = Input.GetAxisRaw("Vertical") * speed;

        movement = new Vector3(x, 0, z);
        movement = Vector3.Normalize(movement);
        

        transform.position = transform.position + movement;
        
        Quaternion rotation = Quaternion.LookRotation(movement, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        timeCount = timeCount + Time.deltaTime;

    }
}
