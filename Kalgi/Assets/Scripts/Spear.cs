using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public PlayerMovement playerMovement;
    private Rigidbody rbSpear;

    private Transform firePoint;
    public float spearSpeed;
    
    void Start()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        firePoint = playerMovement.firePoint;
        rbSpear = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        //rbSpear.velocity = firePoint.forward * spearSpeed;
        rbSpear.AddForce(firePoint.forward.normalized * spearSpeed * Time.deltaTime, ForceMode.Impulse);
    }
}
