using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookat : MonoBehaviour
{
    GameObject target;
    Transform mini;

    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindWithTag("mainPlayer");
        mini = target.transform;
        offset = transform.position - mini.position;
        Vector3 mapPos = mini.position + offset;
        transform.position = Vector3.Lerp(transform.position, mapPos, Time.deltaTime);
        transform.rotation = Quaternion.identity;
    }
}
