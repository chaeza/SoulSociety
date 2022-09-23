using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using Photon.Pun;
public class HpBarInfo : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI txtName = null;
    [SerializeField] Slider sliHP = null;

    [SerializeField] GameObject UILookat = null;
    Transform playerPos = null;

    Vector3 lookat;
    
    
    public void StartPlayerPos(Transform player)
    {
        playerPos = player;

    }


    private void Start()
    {
        transform.Rotate(180, 0, 180);
        transform.Translate(0, 0, 2);

    }
    //이름을 출력
    public void SetName(string name)
    {
        Debug.Log("내가등장");
        txtName.text = name;
    }

    //hp 출력
    public void SetHP(float curHP, float maxHP)
    {
        sliHP.value = curHP / maxHP;
    }

    private void Update()
    {
        transform.position=playerPos.position + Vector3.up * 2+Vector3.forward*2 ;
    }

}
