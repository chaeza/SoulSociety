using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInput : MonoBehaviour
{
    //�� ��ũ��Ʈ�� �÷��̾�� 
    public KeyCode inputKey { get; set; } = KeyCode.Alpha0;
    void Update()
    {
        

        if (Input.GetKey(KeyCode.Mouse1)) inputKey = KeyCode.Mouse1;
        else if (Input.GetKeyDown(KeyCode.Q)) inputKey = KeyCode.Q;
        else if (Input.GetKeyDown(KeyCode.W)) inputKey = KeyCode.W;
        else if (Input.GetKeyDown(KeyCode.E)) inputKey = KeyCode.E;
        else if (Input.GetKeyDown(KeyCode.R)) inputKey = KeyCode.R;
        else if (Input.GetKeyDown(KeyCode.A)) inputKey = KeyCode.A;
        else if (Input.GetKeyDown(KeyCode.S)) inputKey = KeyCode.S;
        else if (Input.GetKeyDown(KeyCode.D)) inputKey = KeyCode.D;
        else if (Input.GetKeyDown(KeyCode.F)) inputKey = KeyCode.F;
        else if (Input.GetKeyDown(KeyCode.Tab)) inputKey = KeyCode.Tab;
        else inputKey = KeyCode.Alpha0;
        if (Input.GetKeyDown(KeyCode.Alpha1)) inputKey = KeyCode.Alpha1;// GameMgr.Instance.randomItem.GetRandomitem(gameObject);

    //    Debug.Log(inputKey);
        
    }
}
