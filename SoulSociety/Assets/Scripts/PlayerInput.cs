using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //이 스크립트는 플레이어에게 
    public KeyCode inputKey { get; set; }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1)) inputKey = KeyCode.Mouse1;
        else if (Input.GetKeyDown(KeyCode.Q)) inputKey = KeyCode.Q;
        else if (Input.GetKeyDown(KeyCode.W)) inputKey = KeyCode.W;
        else if (Input.GetKeyDown(KeyCode.E)) inputKey = KeyCode.E;
        else if (Input.GetKeyDown(KeyCode.R)) inputKey = KeyCode.R;
        else if (Input.GetKeyDown(KeyCode.A)) inputKey = KeyCode.A;
        else if (Input.GetKeyDown(KeyCode.S)) inputKey = KeyCode.S;
        else if (Input.GetKeyDown(KeyCode.D)) inputKey = KeyCode.D;
        else if (Input.GetKeyDown(KeyCode.F)) inputKey = KeyCode.F;
        else inputKey = KeyCode.Alpha0;
        if (Input.GetKeyDown(KeyCode.Alpha1)) GameMgr.Instance.randomItem.GetRandomitem(gameObject);

        Debug.Log(inputKey);
        
    }
}
