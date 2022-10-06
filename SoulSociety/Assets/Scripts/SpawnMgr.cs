using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class SpawnMgr : MonoBehaviourPun
{
    // 소환할 Object
    GameObject[] groundCh = null;     //그라운드태그 모을 장소
    bool[] groundNum = new bool[1000];    //해당 그라운드에 있는 아이템 위치 번호
    public GameObject itemPrefab;     //생성될 아이템 프리펩
    public GameObject soulPrefab;     //생성될 소울 프리펩
    public GameObject[] gameObjects;

    int ran;   //아이템 랜덤 위치
    int ran2;  //소울 랜덤 위치
    int ran3;  //새로운 아이템 랜덤 위치
    int ran4;  //새로운 소울 랜덤 위치

    Queue<GameObject> queue = new Queue<GameObject>();    //풀 저장할 큐

    PhotonView Master;

    private void Awake()
    {
        groundCh = GameObject.FindGameObjectsWithTag("Ground");        //그라운드 태그인것 모두 찾아 배열에 넣다
    }


    //생성할때  //풀링전에 생성을 한다 (매니저)
    [PunRPC]
    [System.Obsolete]
    public void ItemInit()     //아이템 x개 생성
    {
        for (int i = 0; i < 49; i++)
        {
            ran = Random.Range(0, groundCh.Length);
            while (groundNum[ran] == true)
            {
                ran = Random.Range(0, groundCh.Length);
            }
            groundNum[ran] = true;
            GameObject obj = PhotonNetwork.InstantiateSceneObject("ItemBox", groundCh[ran].transform.position, Quaternion.identity);
            obj.SendMessage("MyNum", ran, SendMessageOptions.DontRequireReceiver);
        }
    }
    [PunRPC]
    [System.Obsolete]
    public void SoulInit()    //소울 x개 생성
    {
        for (int i = 0; i < 4; i++)
        {
            ran2 = Random.Range(0, groundCh.Length);
            while(groundNum[ran2]==true)
            {
                ran2 = Random.Range(0, groundCh.Length);
            }
            groundNum[ran2] = true;
            GameObject obj = PhotonNetwork.InstantiateSceneObject("BlueSoul", groundCh[ran2].transform.position + new Vector3(0f, 1.8f, 0f), Quaternion.identity);
            obj.SendMessage("MyNum", ran2, SendMessageOptions.DontRequireReceiver);

        }
    }


    [PunRPC]
    //풀에서 내보낼때
    public void ItemGet(int groundNum)   //플레이어가 박스에 닿으면
    {
   

        GameObject obj;

        //있다면 큐에서 빼내서 쓴다 
        obj = queue.Dequeue();

        obj.transform.position = groundCh[groundNum].transform.position;
        obj.SendMessage("MyNum", groundNum, SendMessageOptions.DontRequireReceiver);
        obj.gameObject.SetActive(true);

    }
    [PunRPC]
    public void SoulGet(int groundNum)  //소울에 닿으면
    {

        GameObject obj;

        //있다면 큐에서 빼내서 쓴다 
        obj = queue.Dequeue();

        obj.transform.position = groundCh[groundNum].transform.position + new Vector3(0f, 1.8f, 0f);
        obj.SendMessage("MyNum", groundNum, SendMessageOptions.DontRequireReceiver);
        obj.gameObject.SetActive(true);

    }

    [PunRPC]
    public void FindItemInPool(int ViewID2,int num)
    {
        GameObject obj = null;
        
        PhotonView[] allPoton = FindObjectsOfType<PhotonView>();
        for (int i = 0; i < allPoton.Length; i++)
        {
            if (allPoton[i].ViewID == ViewID2)
            {
                obj = allPoton[i].gameObject;
            }
        }
        //photonView.RPC("Relase", RpcTarget.All, obj);
        groundNum[num] = false;
        Relase(obj);
    }

    [PunRPC]
    public void FindSoulInPool(int ViewID2, int num)
    {
        GameObject obj = null;
        PhotonView[] allPoton = FindObjectsOfType<PhotonView>();
        for (int i = 0; i < allPoton.Length; i++)
        {
            if (allPoton[i].ViewID == ViewID2)
            {
                obj = allPoton[i].gameObject;
            }
        }
        // photonView.RPC("SoulRelase", RpcTarget.All, obj);
        groundNum[num] = false;

        SoulRelase(obj);
    }


    //아이템
    //풀에 들어올때

    public void Relase(GameObject obj)
    {   //큐로 다시 보낸다
        obj.gameObject.SetActive(false);   //플레이어에 닿으면 false시키고 큐에 저장
        queue.Enqueue(obj);

        StartCoroutine(TenSec());    //x초 코루틴 실행
    }

    //영혼 오브젝트 풀

    public void SoulRelase(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        queue.Enqueue(obj);

        StartCoroutine(Minute());
    }


    IEnumerator TenSec()
    {
        yield return new WaitForSeconds(10f);
       
        if (PhotonNetwork.IsMasterClient)
        {
            ran3 = Random.Range(0, groundCh.Length);
            while (groundNum[ran3] == true)
            {
                ran3 = Random.Range(0, groundCh.Length);
            }
            groundNum[ran3] = true;

            photonView.RPC("ItemGet", RpcTarget.All, ran3);
        }
    }

    IEnumerator Minute()
    {
        yield return new WaitForSeconds(10f);
     
        if (PhotonNetwork.IsMasterClient)
        {
            ran4 = Random.Range(0, groundCh.Length);
            while (groundNum[ran4] == true)
            {
                ran4 = Random.Range(0, groundCh.Length);
            }
            groundNum[ran4] = true;

            photonView.RPC("SoulGet", RpcTarget.All, ran4);
        }

    }
}
