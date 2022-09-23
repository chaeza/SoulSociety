using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerAttack : MonoBehaviourPun
{
    [SerializeField] GameObject hitBox = null;
    Animator myAnimator;
    PlayerInfo playerInfo;
    bool isAttack = true;

    private void Start()
    {
        myAnimator =GetComponent<Animator>();
        playerInfo = GetComponent<PlayerInfo>();
    }

    private void Update()
    {
        if (GameMgr.Instance.endGame == true) return;
        if (playerInfo.playerState == state.Die) return;
        if (playerInfo.playerState == state.Stun) return;
        if (photonView.IsMine == false) return;
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.A)
        {
            Attack();
        }
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.Q) SendMessage("ItemFire", SendMessageOptions.DontRequireReceiver);
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.W) SendMessage("ItemFire", SendMessageOptions.DontRequireReceiver);
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.E) SendMessage("ItemFire", SendMessageOptions.DontRequireReceiver);
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.R) SendMessage("ItemFire", SendMessageOptions.DontRequireReceiver);
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.F) SendMessage("SkillFire", SendMessageOptions.DontRequireReceiver);
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.Alpha1) GameMgr.Instance.randomItem.GetRandomitem(gameObject);
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.Alpha2) gameObject.SendMessage("BlueSoul", SendMessageOptions.DontRequireReceiver);
    }
    [PunRPC]
    public void Attack()
    {
        //모션 
        if (isAttack==true)
        {
            isAttack = false;
            //모션 랜덤 설정 
            int motionNum = Random.Range(0, 3);
            switch (motionNum)
            {
                case 0:
                    myAnimator.SetTrigger("isAttack1");
                    break;
                case 1:
                    myAnimator.SetTrigger("isAttack2");
                    break;
                case 2:
                    myAnimator.SetTrigger("isAttack3");
                    break;
            }
            //코루틴으로 딜레이 생성
          //  StartCoroutine(AttackDelay());

            photonView.StartCoroutine(AttackDelay());
        }
        //사운드
        //전달 함수 
        
    }
    //평타 딜레이 
    IEnumerator AttackDelay()
    {
        Debug.Log("공격");
        //hitBox.SetActive(true) ;  
        hitBox.GetComponentInChildren<BoxCollider>().enabled = true;
        GetComponent<PlayerMove>().MoveStop();
        yield return new WaitForSeconds(1);
        //hitBox.SetActive(false) ;  
        hitBox.GetComponentInChildren<BoxCollider>().enabled =false;
        isAttack = true;
    }


}
