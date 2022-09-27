using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //이 스크립트는 플레이어에게 
    public KeyCode inputKey { get; private set; }
    void Update()
    {
    
        if (Input.GetKey(KeyCode.Mouse1)) inputKey = KeyCode.Mouse1;
        else if (Input.GetKey(KeyCode.Mouse0)) inputKey = KeyCode.Mouse0;
        else if (Input.GetKeyDown(KeyCode.Q)) inputKey = KeyCode.Q;
        else if (Input.GetKeyDown(KeyCode.W)) inputKey = KeyCode.W;
        else if (Input.GetKeyDown(KeyCode.E)) inputKey = KeyCode.E;
        else if (Input.GetKeyDown(KeyCode.R)) inputKey = KeyCode.R;
        else if (Input.GetKeyDown(KeyCode.A)) inputKey = KeyCode.A;
        else if (Input.GetKeyDown(KeyCode.S)) inputKey = KeyCode.S;
        else if (Input.GetKeyDown(KeyCode.D)) inputKey = KeyCode.D;
        else if (Input.GetKeyDown(KeyCode.F)) inputKey = KeyCode.F;
        else if (Input.GetKey(KeyCode.Tab)) inputKey = KeyCode.Tab;
        else if (Input.GetKey(KeyCode.Tab)) inputKey = KeyCode.Escape;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) inputKey = KeyCode.Alpha2;
        else if (Input.GetKeyDown(KeyCode.Alpha1)) inputKey = KeyCode.Alpha1;
        else inputKey = KeyCode.Alpha0;

    //    Debug.Log(inputKey);
        
    }
}
