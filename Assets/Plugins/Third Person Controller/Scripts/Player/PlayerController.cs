using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f; // movespeed of the player
   
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // getting input from axis
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized; // normalizing the input

        //if player is pressing any button only then change rotation
        if (inputDir != Vector2.zero)
            //   making rotation   on Y axis         by finding atan redient         and converting atan rediant to degree
            transform.eulerAngles = Vector3.up * Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;

        //moving the player
        transform.Translate(((moveSpeed * inputDir.magnitude) * transform.forward) * Time.deltaTime, Space.World);
    }
}
