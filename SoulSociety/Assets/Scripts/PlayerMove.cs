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
            
            
            Debug.Log("마우스클릭");
        }
    }
    public void Move(Vector3 mousePos)
    {
        // 움직임 애니메이션
        // 사운드
        // 실제 움직임 (포지션 변경)
        // 코루틴으로 실행 ]
        Coroutine move=null;//이동 코루틴 선언
        if (isMove == true)//이동하던 코루틴을 멈춘다.
            StopCoroutine(move);
            move = StartCoroutine(MoveCorutine());//지정한 곳으로 이동하는 코루틴 시작.


    }

    IEnumerator MoveCorutine()
    {
        isMove = true;
        while(true)
        {
            Debug.Log("가는중");
            yield return null;
            if (isMove == false) break;//중간에 moveStop으로 멈출시 코루틴 정지
        }
        isMove = false;
        yield break;
    }

    public void moveStop()
    {
        isMove = false;
    }

}
