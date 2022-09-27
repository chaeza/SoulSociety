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


    //Transform playerPos = null;
    [SerializeField] Transform player;



    /*public void StartPlayerPos(Transform player)
    {
        playerPos = player;

    }*/

    Transform cam;
    private void Start()
    {
        /*transform.Rotate(180, 0, 180);
        transform.Translate(0, 0, 2);*/
        cam = Camera.main.transform;
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
        //transform.position=playerPos.position + Vector3.up * 2+Vector3.forward*2 ;
        //transform.position = player.position;
        this.transform.position = new Vector3(this.GetComponentInParent<GameObject>().transform.position.x + 3f, this.GetComponentInParent<GameObject>().transform.position.y, this.GetComponentInParent<GameObject>().transform.position.z);
        transform.LookAt(transform.position+new Vector3(0,0,2) + cam.rotation * Vector3.forward, cam.rotation * Vector3.up + new Vector3(0, 2, 2));
    }

}
