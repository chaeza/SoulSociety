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
        //ī�޶�
        cam = Camera.main.transform;

    }
    //�̸��� ���
    public void SetName(string name)
    {
        nickname.text = name;
    }


    //hp ���
    public void SetHP(float curHP, float maxHP)
    {
        Hpbar.value = curHP / maxHP;
    }

    private void Update()
    {
       
        //ī�޶� ����
            transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        
    }

}
