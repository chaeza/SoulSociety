using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WindCyclone : MonoBehaviourPun, SkillMethod
{
    bool skillCool = false;
    AudioSource sound;
 
    public void ResetCooltime()
    {
        skillCool = false;//스킬을 다시 사용 가능하게함
        Debug.Log("스킬쿨끝");
    }

   

    public void SkillFire()
    {
        if (skillCool == false)//스킬 사용 가능이면
        {
            GameObject a = PhotonNetwork.Instantiate("WindCyclone", transform.position, Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.
            a.AddComponent<WindCycloneHit>();//이펙트에 히트 스크립트를 넣습니다.
            a.SendMessage("AttackerName", gameObject.GetPhotonView().ViewID, SendMessageOptions.DontRequireReceiver);//이펙트에 공격자를 지정합니다.
            a.AddComponent<MyPosition>();
            
            a.transform.Rotate(-90f, 0f, 0f);
            a.SendMessage("MyPos", gameObject.transform, SendMessageOptions.DontRequireReceiver);

            sound = a.GetComponent<AudioSource>();
            StartCoroutine(soundCh());
            sound.Play();


            GameMgr.Instance.DestroyTarget(a, 8f);    //지속 시간
            

            skillCool = true;//쿨타임 온 시켜 다시 사용 못하게함
 
            Debug.Log("스킬사용");
            GameMgr.Instance.uIMgr.SkillCooltime(gameObject, 20);//UI매니저에 쿨타임 x초를 보냄
        }
    }

    IEnumerator soundCh()
    {
        yield return new WaitForSeconds(7f);
        sound.Stop();
    }
}
