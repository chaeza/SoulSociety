using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    bool isMove = false;

    private void Update()
    {
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse1)
        {
            
            
            Debug.Log("���콺Ŭ��");
        }
    }
    public void Move(Vector3 mousePos)
    {
        // ������ �ִϸ��̼�
        // ����
        // ���� ������ (������ ����)
        // �ڷ�ƾ���� ���� ]
        Coroutine move=null;//�̵� �ڷ�ƾ ����
        if (isMove == true)//�̵��ϴ� �ڷ�ƾ�� �����.
            StopCoroutine(move);
            move = StartCoroutine(MoveCorutine());//������ ������ �̵��ϴ� �ڷ�ƾ ����.


    }

    IEnumerator MoveCorutine()
    {
        isMove = true;
        while(true)
        {
            Debug.Log("������");
            yield return null;
            if (isMove == false) break;//�߰��� moveStop���� ����� �ڷ�ƾ ����
        }
        isMove = false;
        yield break;
    }

    public void moveStop()
    {
        isMove = false;
    }

}
