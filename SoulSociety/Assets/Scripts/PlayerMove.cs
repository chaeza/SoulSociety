using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    bool isMove = true;

    public void Move(Vector3 mousePos)
    {
        // 움직임 애니메이션
        // 사운드
        // 실제 움직임 (포지션 변경)
        // 코루틴으로 실행 ]

        Debug.Log("됌");
        StartCoroutine(MoveCorutine());


    }

    IEnumerator MoveCorutine()
    {
        while(true)
        {
            Debug.Log("가는중");
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
