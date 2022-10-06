using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using Photon.Pun;
public class HpBarInfo : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI nickname = null;
    [SerializeField] Slider Hpbar = null;

    Transform cam = null;



    private void Start()
    {
        //카메라
        cam = Camera.main.transform;

    }
    //이름을 출력
    public void SetName(string name)
    {
        nickname.text = name;
    }


    //hp 출력
    public void SetHP(float curHP, float maxHP)
    {
        Hpbar.value = curHP / maxHP;
    }

    private void Update()
    {
       
        //카메라 보기
            transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        
    }

}
