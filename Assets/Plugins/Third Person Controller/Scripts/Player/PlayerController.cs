using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    Vector2 inputDir;

    [Header("Movements")]
    [SerializeField] float moveSpeed = 5f; // movespeed of the player
    [SerializeField] float moveSpeedSmoothingTime;
    float currentSpeed;
    float moveSpeedVelocity;


    [Header("Rotations")]
    float currentVelocity;
    [SerializeField] float rotationTiming = 0.1f;

    Transform cameraObject;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cameraObject = Camera.main.transform;
        // getting input from axis
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputDir = input.normalized; // normalizing the input


    }

    private void FixedUpdate()
    {
        Rotation();
        Movement();
    }

    private void Rotation()
    {  //if player is pressing any button only then change rotation
        if (inputDir != Vector2.zero)
        {
            //   making rotationon Y axis by finding atan redient and converting atan rediant to degree
            float rotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraObject.transform.eulerAngles.y;

            //smooting the rotations
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation, ref currentVelocity
, rotationTiming);
        }

    }

    private void Movement()
    {
        // target speed
        float targetSpeed = moveSpeed * inputDir.magnitude;

        // current speed of the player
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref moveSpeedVelocity, moveSpeedSmoothingTime);
        //moving the player
        transform.Translate((transform.forward * currentSpeed) * Time.fixedDeltaTime, Space.Self);
    }
}


