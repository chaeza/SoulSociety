using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GameMgr.Instance.myRandomSkill.GetRandomSkill(gameObject);
        GameMgr.Instance.randomSkill.GetRandomSkill(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.A)) GameMgr.Instance.PlayerAttack.Attack();
        //if (Input.GetMouseButtonDown(0)) GameMgr.Instance.PlayerMove.Move();
        if (Input.GetKeyUp(KeyCode.D)) SendMessage("SkillFire", SendMessageOptions.DontRequireReceiver);
    }
}
