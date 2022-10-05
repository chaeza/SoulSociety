using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerAttack : MonoBehaviourPun
{
    Animator myAnimator;
    PlayerInfo playerInfo;
    bool isAttack = true;
    AudioSource sound;
    private void Start()
    {
        sound = GetComponent<AudioSource>();
        myAnimator =GetComponent<Animator>();
        playerInfo = GetComponent<PlayerInfo>();
    }

    private void Update()
    {
        if (playerInfo.stay == true) return;
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
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.D) SendMessage("SkillFire", SendMessageOptions.DontRequireReceiver);
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.F) SendMessage("DashFire", SendMessageOptions.DontRequireReceiver);

        if (GameMgr.Instance.playerInput.inputKey == KeyCode.LeftShift) SendMessage("BlueSoul", SendMessageOptions.DontRequireReceiver);
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
                    {
                        myAnimator.SetTrigger("isAttack1");
                        break;
                    }
                case 1:
                    {
                        myAnimator.SetTrigger("isAttack2");
                        break;
                    }
                case 2:
                    {
                        myAnimator.SetTrigger("isAttack3");
                        break;
                    }
            }
            //코루틴으로 딜레이 생성
          //  StartCoroutine(AttackDelay());

            photonView.StartCoroutine(AttackDelay(motionNum));
        }
        //사운드
        //전달 함수 
        
    }
    //평타 딜레이 
    IEnumerator AttackDelay(int a)
    {
        Debug.Log("공격");
        //hitBox.SetActive(true) ;
        yield return new WaitForSeconds(0.2f);
        GameObject eff = PhotonNetwork.Instantiate("BasicAttackEff", transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        sound.Play();
        eff.AddComponent<BasicAttackHitbox>();
        eff.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//이펙트에 공격자를 지정합니다.
        eff.SendMessage("AttackerDamage",GetComponent<PlayerInfo>().basicAttackDamage, SendMessageOptions.DontRequireReceiver);//이펙트에 공격력를 지정합니다.
        GameMgr.Instance.DestroyTarget(eff ,0.5f);
        if(a==0||a==1) eff.transform.Rotate(0, 0, -45);
        GetComponent<PlayerMove>().MoveStop();
        GetComponent<PlayerMove>().donMove = true;
        yield return new WaitForSeconds(0.5f);
        GetComponent<PlayerMove>().donMove = false;
        yield return new WaitForSeconds(0.3f);
        //hitBox.SetActive(false) ;  
        isAttack = true;
    }


}
