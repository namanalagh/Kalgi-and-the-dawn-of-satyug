using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float speed;
    [SerializeField, Range(0, 100)] private float dashForce;
    public float dashCooldown;
    private float horizontal;
    private float vertical;
    private float dashStartTime;
    private bool dashPressed;
    
    private Rigidbody rb;
    private Vector3 movement;
    private Vector3 currentDir;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 velocity = rb.velocity;

        if (movement.magnitude > 0)
            currentDir = movement;
        
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKey(KeyCode.Space) && !dashPressed)
        {
            StartCoroutine("DashCooldown");
            //rb.AddForce(movement*-dashForce*Time.deltaTime);
            dashStartTime = Time.time;
        }
        float dashTime = Time.time - dashStartTime;
        if (dashTime < .2)
        {
            rb.MovePosition(rb.position+currentDir*-dashForce*Time.deltaTime);
        }
        else
        {
            rb.MovePosition(rb.position+movement*-speed*Time.deltaTime);
        }
    }

    private IEnumerator DashCooldown()
    {
        dashPressed = true;
        yield return new WaitForSeconds(dashCooldown);
        dashPressed = false;
    }
}
