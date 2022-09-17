using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //이 스크립트는 플레이어에게 



    // Start is called before the first frame update
    void Start()
    {
        //GameMgr.Instance.myRandomSkill.GetRandomSkill(gameObject);
        GameMgr.Instance.randomSkill.GetRandomSkill(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //마우스 포지션값 (x,y)를 읽어옴 
        if (Input.GetKeyDown(KeyCode.Mouse1))
            GameMgr.Instance.playerMove.Move(Input.mousePosition);

        if (Input.GetKeyUp(KeyCode.A)) GameMgr.Instance.playerAttack.Attack();

        if (Input.GetKeyUp(KeyCode.D)) SendMessage("SkillFire", SendMessageOptions.DontRequireReceiver);

        if (Input.GetKeyUp(KeyCode.S)) GameMgr.Instance.playerMove.moveStop();
    }
}
