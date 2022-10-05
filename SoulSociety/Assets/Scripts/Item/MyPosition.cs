using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPosition : MonoBehaviour
{
    Transform myPos = null;
    float yPos = 0;

    void MyPos(Transform pos)
    {
        myPos = pos;
    }
    void YPos(float y)
    {
        yPos = y;
    }
    void Update()
    {
        transform.position = myPos.transform.position;
        if(yPos!=0) transform.Translate(0f,yPos,0f);
    }
}
