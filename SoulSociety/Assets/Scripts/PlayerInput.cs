using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //�� ��ũ��Ʈ�� �÷��̾�� 



    // Start is called before the first frame update
    void Start()
    {
        //GameMgr.Instance.myRandomSkill.GetRandomSkill(gameObject);
        GameMgr.Instance.randomSkill.GetRandomSkill(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //���콺 �����ǰ� (x,y)�� �о�� 
        if (Input.GetKeyDown(KeyCode.Mouse1))
            GameMgr.Instance.playerMove.Move(Input.mousePosition);

        if (Input.GetKeyUp(KeyCode.A)) GameMgr.Instance.playerAttack.Attack();

        if (Input.GetKeyUp(KeyCode.D)) SendMessage("SkillFire", SendMessageOptions.DontRequireReceiver);

        if (Input.GetKeyUp(KeyCode.S)) GameMgr.Instance.playerMove.moveStop();
    }
}
