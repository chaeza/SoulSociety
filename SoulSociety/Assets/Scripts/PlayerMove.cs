using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    bool isMove = true;

    public void Move(Vector3 mousePos)
    {
        // ������ �ִϸ��̼�
        // ����
        // ���� ������ (������ ����)
        // �ڷ�ƾ���� ���� ]

        Debug.Log("��");
        StartCoroutine(MoveCorutine());


    }

    IEnumerator MoveCorutine()
    {
        while(true)
        {
            Debug.Log("������");
            yield return null;
            if (isMove == false) break;
        }
        isMove = true;
        yield break;
    }

    public void moveStop()
    {
        isMove = false;
    }

}
