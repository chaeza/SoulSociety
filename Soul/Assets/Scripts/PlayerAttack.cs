using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private void Update()
    {
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.A) Debug.Log("����");
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.F) SendMessage("SkillFire", SendMessageOptions.DontRequireReceiver);
    }
    public void Attack()
    {
        //��� 
        //����
        //���� �Լ� 
        //�ڷ�ƾ���� ������ ����
    }
}
