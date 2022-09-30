using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10;
    [SerializeField] private float sprintFactor = 5;
    [SerializeField] private float scrollSpeed = 30;

    private float xRotation = 0;
    private float yRotation = 0;

    // Update is called once per frame
    void Update()
    {
        Movement();
        Scrolling();
    }

    void Movement()
    {
        Vector3 speed = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) speed += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) speed -= Vector3.forward;
        if (Input.GetKey(KeyCode.D)) speed += Vector3.right;
        if (Input.GetKey(KeyCode.A)) speed -= Vector3.right;

        float sprintMultiplier = Input.GetKey(KeyCode.LeftShift) ? sprintFactor : 1;

        transform.Translate(speed.normalized * movementSpeed * sprintMultiplier * Time.deltaTime);
    }

    void Scrolling()
    {
        float horizontalScroll = Input.GetAxis("Mouse X") * scrollSpeed;
        float verticalScroll = Input.GetAxis("Mouse Y") * scrollSpeed;

        yRotation += horizontalScroll;
        xRotation -= verticalScroll;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
 
        this.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
    }
}
