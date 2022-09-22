using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerAttack : MonoBehaviourPun
{
    [SerializeField] GameObject hitBox = null;

    Animator myAnimator;
    bool isAttack=true;


    private void Start()
    {
        myAnimator =GetComponent<Animator>();
    }

    private void Update()
    {
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
    }
    public void Attack()
    {
        //��� 
        if (isAttack == true)
        {
            Debug.Log("����");
            isAttack = false;
            //��� ���� ���� 
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
            //�ڷ�ƾ���� ������ ����
           StartCoroutine(AttackDelay());
        }
        //����
        //���� �Լ� 
        
    }
    //��Ÿ ������ 
    IEnumerator AttackDelay()
    {
        hitBox.SetActive(true); 
        GetComponent<PlayerMove>().MoveStop();
        yield return new WaitForSeconds(1);
        hitBox.SetActive(false);
        isAttack =true;
    }


}
