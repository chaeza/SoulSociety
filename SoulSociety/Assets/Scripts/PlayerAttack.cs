using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private void Update()
    {
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.A) Debug.Log("공격");
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.F) SendMessage("SkillFire", SendMessageOptions.DontRequireReceiver);
    }
    public void Attack()
    {
        //모션 
        //사운드
        //전달 함수 
        //코루틴으로 딜레이 생성
    }
}
