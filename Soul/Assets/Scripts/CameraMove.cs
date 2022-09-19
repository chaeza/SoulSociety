using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] float distanceFromPlayerZ = 3f;
    [SerializeField] float distanceFromPlayerY = 15f;
    [SerializeField] float cameraSpeed = 0.1f;
    [SerializeField] Transform playerPos;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        transform.position = playerPos.position - Vector3.forward * distanceFromPlayerZ + Vector3.up * distanceFromPlayerY;
        transform.LookAt(playerPos.position + Vector3.up * 2);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            transform.position = playerPos.position - Vector3.forward * distanceFromPlayerZ + Vector3.up * distanceFromPlayerY;
            transform.LookAt(playerPos.position + Vector3.up * 2);
        }
        //if(Input.mousePosition.y > 1080)
        Debug.Log(Input.mousePosition);
        if (Input.mousePosition.x >= 1916.86 && transform.position.x <= 50)
        {
            transform.position = transform.position + Vector3.right * cameraSpeed;
        }
        if (Input.mousePosition.x <= 0.88 && transform.position.x >= -50)
        {
            transform.position = transform.position - Vector3.right * cameraSpeed;
        }
        if (Input.mousePosition.y >= 1080 && transform.position.z <= 50)
        {
            transform.position = transform.position + Vector3.forward * cameraSpeed;
        }
        if (Input.mousePosition.y <= 2.26 && transform.position.z >= -50)
        {
            transform.position = transform.position - Vector3.forward * cameraSpeed;
        }
    }
}
