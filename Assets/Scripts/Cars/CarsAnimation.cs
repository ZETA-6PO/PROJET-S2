using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsAnimation : MonoBehaviour
{
    [Header("Car settings")] 
    public float accelerationFactor = 30.0f;
    public float turnFactor = 3.5f;
    
    // Local variables
    private float accelerationInput = 0;
    private float steeringInput = 0;
    private float rotationAngle = 0;

    private Rigidbody2D carRigidbody2D;

    private void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ApplyEngineForce()
    {
        //create a force for the engine
        Vector2 engineForceVector = transform.up * (accelerationInput * accelerationFactor);
        
        //Apply forces and pushes the car forward
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        //update the rotation angle based on input
        rotationAngle -= steeringInput * turnFactor;
        
        //Apply steering by rotating the car object
        carRigidbody2D.MoveRotation(rotationAngle);
    }
    private void FixedUpdate()
    {
        ApplyEngineForce();
        ApplySteering();
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }
}
