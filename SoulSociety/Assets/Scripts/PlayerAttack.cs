using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
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
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.A)
        {
            Debug.Log("����");
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
        hitBox.GetComponent<BoxCollider>().enabled = true;  
        GetComponent<PlayerMove>().MoveStop();
        yield return new WaitForSeconds(1);
        hitBox.GetComponent<BoxCollider>().enabled = false;
        isAttack =true;
    }


}
