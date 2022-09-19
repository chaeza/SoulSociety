using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //이 스크립트는 플레이어에게 
    public KeyCode inputKey { get; set; }
    void Update()
    {
<<<<<<< HEAD
        //마우스 포지션값 (x,y)를 읽어옴 
        if (Input.GetKeyDown(KeyCode.Mouse1))
            GameMgr.Instance.playerMove.Move(Input.mousePosition);

        if (Input.GetKeyUp(KeyCode.A)) GameMgr.Instance.playerAttack.Attack();

        if (Input.GetKeyUp(KeyCode.D)) SendMessage("SkillFire", SendMessageOptions.DontRequireReceiver);

        if (Input.GetKeyUp(KeyCode.S)) GameMgr.Instance.playerMove.moveStop();
=======
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
        Debug.Log(inputKey);
        
>>>>>>> main
    }
}
