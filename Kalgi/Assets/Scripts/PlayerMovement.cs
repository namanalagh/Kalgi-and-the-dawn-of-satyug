using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float speed;
    [SerializeField, Range(0, 100)] private float dashForce;
    private float horizontal;
    private float vertical;
    private float dashStartTime;
    private bool dashPressed;
    private bool canThrow = true;
    
    private Rigidbody rb;
    private Rigidbody rbSpear;
    private Camera cam;
    private Transform player;
    private GameObject spearClone;
   
    private Ray ray;
    private RaycastHit hit;
    private Vector3 lookDir;
    private Vector3 movement;
    private Vector3 currentDir;

    public float dashCooldown;
    
    public Transform throwReference;
    public Transform dirArrow;
    public Transform firePoint;
    public GameObject spear;
    
    public LayerMask map;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rbSpear = spear.transform.GetChild(0).GetComponent<Rigidbody>();
        //rbSpear = spear.GetComponent<Rigidbody>();
        player = gameObject.transform;
        cam = Camera.main;
    }

    void Update()
    {
        if (movement.magnitude > 0)
            currentDir = movement;
        
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKeyDown(KeyCode.Space) && !dashPressed)
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

        if (Input.GetKeyDown(KeyCode.Mouse1) && canThrow)
        {
            ThrowSpear();
        }
        
        Cursor.visible = false;
        lookDir = cam.ScreenToWorldPoint(Input.mousePosition);
        ray = cam.ScreenPointToRay(Input.mousePosition); 
        if (Physics.Raycast(ray, out hit, 100, map)) 
        { 
            lookDir = new Vector3(hit.point.x, 0f, hit.point.z);
        }
        throwReference.position = lookDir;
        //dirArrow.position = (transform.position - lookDir.normalized);
        if(Vector3.Distance(transform.position,lookDir)>3)
            dirArrow.LookAt(throwReference.position);
        Quaternion targetRotation = Quaternion.LookRotation(lookDir - player.GetChild(0).position); 
        targetRotation.x = 0; 
        targetRotation.z = 0; 
        transform.rotation = Quaternion.Slerp(player.rotation, targetRotation, 100f * Time.deltaTime);
    }

    private void ThrowSpear()
    {
        Quaternion spearRot = new Quaternion(transform.rotation.x, 90f, transform.rotation.z,0);
        Debug.Log("Success");
        spearClone = Instantiate(spear, firePoint.position, firePoint.rotation);
        //rbSpear.velocity = Vector3.right*100f;
        StartCoroutine("SpearCooldown");
    }

    private IEnumerator DashCooldown()
    {
        dashPressed = true;
        yield return new WaitForSeconds(dashCooldown);
        dashPressed = false;
    }

    private IEnumerator SpearCooldown()
    {
        canThrow = false;
        yield return new WaitForSeconds(1.5f);
        Destroy(spearClone);
        canThrow = true;
    }
}
