using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Slash : MonoBehaviourPun, ItemMethod
{
    [SerializeField]
    int itemNum = 0;
    int skillRange = 20;
    bool skillCool = false;
    bool skillClick = false;
    RectTransform myskillRangerect = null;
    GameObject skilla;
    AudioSource sound;

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
            target.x = mousePos.x;            target.y = mousePos.y;
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

            if (hit.collider.tag == "Ground" || hit.collider.tag == "UnGround")
            {
                desiredDir = hit.point;
                desiredDir.y = transform.position.y;
            }
            if (skillCool == false)//???? ???? ????????
            {
                GetComponent<Animator>().SetTrigger("isAttack3");
                GetComponent<PlayerInfo>().Stay(0.5f);
                GameObject b = PhotonNetwork.Instantiate("Slash", transform.position, Quaternion.identity);//???????? ???? ?????????? ??????.
                GameObject a = PhotonNetwork.Instantiate("SlashHitbox", transform.position, Quaternion.identity);//???????? ???? ?????????? ??????.
                b.transform.LookAt(desiredDir);
                a.transform.LookAt(desiredDir);
                a.AddComponent<SlashHitbox>();//???????? ???? ?????????? ????????.
                sound = b.GetComponent<AudioSource>();

                sound.Play();
                a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//???????? ???????? ??????????.
                transform.LookAt(desiredDir);

                GameMgr.Instance.DestroyTarget(a, 1.5f);
                GameMgr.Instance.DestroyTarget(b, 1.5f);
                StartCoroutine(Fire(a));//???? ?????????? ??????
                
            }
        }
    }
    IEnumerator Fire(GameObject skill)//???? ??????????
    {
        for (int i = 0; i < 30; i++)
        {
            skill.transform.Translate(0, 0, 0.45f);
            yield return null;
        }
        GameMgr.Instance.uIMgr.UseItem(itemNum);
        GameMgr.Instance.inventory.RemoveInventory(itemNum);
        Destroy(GetComponent<Slash>());
        yield break;
    }
}
