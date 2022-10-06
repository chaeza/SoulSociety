using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Kick : MonoBehaviourPun, ItemMethod
{
    [SerializeField]
    int itemNum = 0;
    int skillRange = 20;
    bool skillCool = false;
    bool skillClick = false;
    RectTransform myskillRangerect = null;
    GameObject skilla;

    Vector3 canSkill;
    private void Start()
    {
        myskillRangerect = GetComponent<PlayerInfo>().myskillRangerect;

        skilla = GetComponent<PlayerInfo>().skilla;
    }
    private void Update()
    {
        if (skillClick == true)
        {

            Vector3 mousePos = Input.mousePosition;

            Vector3 target;
            target.x = mousePos.x;
            target.y = mousePos.y;
            target.z = 0;

            skilla.transform.position = target;

            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, 30f);
            canSkill = hit.point;
            canSkill.y = transform.position.y;
        }
    }
    public void GetItem(int itemnum)
    {
        if (itemNum == 0)
            itemNum = itemnum;
    }

    public void ItemFire()
    {
        if (itemNum == 1 && GameMgr.Instance.playerInput.inputKey == KeyCode.Q) ItemSkill();
        else if (itemNum == 2 && GameMgr.Instance.playerInput.inputKey == KeyCode.W) ItemSkill();
        else if (itemNum == 3 && GameMgr.Instance.playerInput.inputKey == KeyCode.E) ItemSkill();
        else if (itemNum == 4 && GameMgr.Instance.playerInput.inputKey == KeyCode.R) ItemSkill();
    }
    public void ItemSkill()
    {
        if (skillClick == false)
        {
            skilla.SetActive(true);
            myskillRangerect.gameObject.SetActive(true);
            myskillRangerect.sizeDelta = new Vector2(skillRange, skillRange);

            skillClick = true;
        }

        else
        {
            skillClick = false;
            myskillRangerect.gameObject.SetActive(false);
            skilla.SetActive(false);
        }
    }
    public void SkillClick(Vector3 Pos)
    {
        if (skillClick == true)
        {
            skillClick = false;
            myskillRangerect.gameObject.SetActive(false);
            skilla.SetActive(false);
            if (Vector3.Distance(canSkill, transform.position) > skillRange / 2) return;

            RaycastHit hit;
            Vector3 desiredDir = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Pos);
            int mask = 1 << LayerMask.NameToLayer("Terrain");
            Physics.Raycast(Camera.main.ScreenPointToRay(Pos), out hit, 30f, mask);

            Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red, 1f);

            if (hit.collider.tag == "Ground"|| hit.collider.tag == "UnGround")
            {
                desiredDir = hit.point;
                desiredDir.y = transform.position.y;
            }
            if (skillCool == false)//스킬 사용 가능이면
            {
                GetComponent<Animator>().SetTrigger("isKick");
                GetComponent<PlayerInfo>().Stay(0.5f);
                StartCoroutine(Stay(desiredDir, 0.2f));

            }
        }
    }
    IEnumerator Stay(Vector3 desiredDir, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject b = PhotonNetwork.Instantiate("Dash", transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.
        GameObject a = PhotonNetwork.Instantiate("KickHitbox", transform.position, Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.
        b.transform.LookAt(desiredDir);
        a.transform.LookAt(desiredDir);
        a.transform.Translate(0f, 1f, 2f);
        a.AddComponent<KickHitbox>();//이펙트에 히트 스크립트를 넣습니다.
        a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//이펙트에 공격자를 지정합니다.
        transform.LookAt(desiredDir);

        GameMgr.Instance.DestroyTarget(a, 1.5f);
        GameMgr.Instance.DestroyTarget(b, 1.5f);

        GameMgr.Instance.uIMgr.UseItem(itemNum);
        GameMgr.Instance.inventory.RemoveInventory(itemNum);
        Destroy(GetComponent<Kick>());
        yield break;
    }
}
