using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{
    //�÷��̾� �̵��ӵ�
    [SerializeField] float moveSpeed = 1;
    PlayerInfo playerInfo;
    Animator myAnimator;
    //�̵� ���� ���� 
    bool isMove = false;

    Vector3 moveTarget;
    //�̵� ��ǥ ����
    Vector3 desiredDir;
    //��ǥ ���������� ���� ���� 
    Vector3 targetDir;

    private void Start()
    {
        playerInfo = GetComponent<PlayerInfo>();
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        //���� ����, �÷��̾� ĳ���� ���, ����, ����� �÷��̾��� ��� Update ��� x
        if (GameMgr.Instance.endGame == true) return;
        if (playerInfo.playerState == state.Die) return;
        if (playerInfo.playerState == state.Stun) return;
        if (photonView.IsMine == false) return;

        //��ų ���� UI
        if (Input.mousePosition.x > 85 && Input.mousePosition.x < 170 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance) GameMgr.Instance.uIMgr.OnExplantionSkill(true);
        else GameMgr.Instance.uIMgr.OnExplantionSkill(false);

        //������ ���� UI
        if (Input.mousePosition.x > 680 && Input.mousePosition.x < 760 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance.inventory.InvetoryCount(1) == false) GameMgr.Instance.uIMgr.OnExplantionItem(1, GameMgr.Instance.inventory.GetInventory(1));
        else if (Input.mousePosition.x > 845 && Input.mousePosition.x < 915 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance.inventory.InvetoryCount(2) == false) GameMgr.Instance.uIMgr.OnExplantionItem(2, GameMgr.Instance.inventory.GetInventory(2));
        else if (Input.mousePosition.x > 1005 && Input.mousePosition.x < 1080 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance.inventory.InvetoryCount(3) == false) GameMgr.Instance.uIMgr.OnExplantionItem(3, GameMgr.Instance.inventory.GetInventory(3));
        else if (Input.mousePosition.x > 1160 && Input.mousePosition.x < 1240 && Input.mousePosition.y > 25 && Input.mousePosition.y < 105 && GameMgr.Instance.inventory.InvetoryCount(4) == false) GameMgr.Instance.uIMgr.OnExplantionItem(4, GameMgr.Instance.inventory.GetInventory(4));
        else GameMgr.Instance.uIMgr.OnExplantionItem(5, 0);

        //���콺 ��Ŭ���Է��� ������ Move�Լ��� �̵� ���� ���� 
        if (Input.mousePosition != null && GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse1)
        {
            Move(Input.mousePosition);
        }

        if (GameMgr.Instance.playerInput.inputKey == KeyCode.S)
        {
            MoveStop();
        }
        if (isMove == true)
        {
            //��ǥ�������� �̵�
            if (Vector3.Distance(desiredDir, transform.position) > 0.1f)
            {
                myAnimator.SetBool("isMove", true);

                gameObject.GetComponent<Rigidbody>().velocity = targetDir.normalized * moveSpeed;
            }
            //��ǥ�������� �����ϸ� ����
            else
                MoveStop();
        }
    }

    //��Ŭ�� ���� ���� ���� ã�� �Լ� 
    public void Move(Vector3 mousePos)
    {
        // ������ �ִϸ��̼�
        // ����
        // ���� ������ (������ ����)

        //����ĳ��Ʈ�� ���� �΋H�� ���� ������ �������� ���� ���� 
        RaycastHit hit;
        //���콺 ������ ���� ScreenPointToRay�� ��ȯ�ؼ� ����ĳ��Ʈ�� ������ ��ġ�� ray�� ���ͷ� �־��ش�. ����׷� ����ĳ��Ʈ�� ��������
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        //��Ʈ ������ ���� ���̾� Terrain���̾� ����ũ�� mask���� ��ȯ 
        int mask = 1 << LayerMask.NameToLayer("Terrain");
        //mask�� ���̾�� ���� ���� �浹�ϴ� ����ĳ��Ʈ�� �߻��Ѵ�. 
        Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 20f, mask);
        //����ĳ��Ʈ�� ���� ���� ����� 
        Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red, 5f);

        //�΋H�� ���� �±װ� �׶��� �� ��츸 �̵� �ϰ� �ϱ� ����
        if (hit.collider.tag == "Ground")
        {
            //����ĳ��Ʈ �浹��ġ�� ���� �����ѵ� �÷��̾�� Y���� �����ϰ� ����� X,Z���󿡼��� ���⺤�͸� ã�Ƴ���. 
            desiredDir = hit.point;
            desiredDir.y = transform.position.y;
            targetDir = desiredDir - transform.position;
            transform.rotation = Quaternion.LookRotation(targetDir);
            //Debug.Log(targetDir.ToString());


            //������ �� �ִ� ���·� ��ȯ���ش�. 
            isMove = true;
        }
    }
    // �������� ���ߴ� �Լ�
    public void MoveStop()
    {
        //������ ���¿� velocity���� 0���� ����� �������� ���´�. 
        myAnimator.SetBool("isMove", false);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        isMove = false;
        //Debug.Log(isMove.ToString());
    }
}
