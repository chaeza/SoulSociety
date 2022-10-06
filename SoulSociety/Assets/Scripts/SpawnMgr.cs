using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class SpawnMgr : MonoBehaviourPun
{
    // ��ȯ�� Object
    GameObject[] groundCh = null;     //�׶����±� ���� ���
    bool[] groundNum = new bool[1000];    //�ش� �׶��忡 �ִ� ������ ��ġ ��ȣ
    public GameObject itemPrefab;     //������ ������ ������
    public GameObject soulPrefab;     //������ �ҿ� ������
    public GameObject[] gameObjects;

    int ran;   //������ ���� ��ġ
    int ran2;  //�ҿ� ���� ��ġ
    int ran3;  //���ο� ������ ���� ��ġ
    int ran4;  //���ο� �ҿ� ���� ��ġ

    Queue<GameObject> queue = new Queue<GameObject>();    //Ǯ ������ ť

    PhotonView Master;

    private void Awake()
    {
        groundCh = GameObject.FindGameObjectsWithTag("Ground");        //�׶��� �±��ΰ� ��� ã�� �迭�� �ִ�
    }


    //�����Ҷ�  //Ǯ������ ������ �Ѵ� (�Ŵ���)
    [PunRPC]
    [System.Obsolete]
    public void ItemInit()     //������ x�� ����
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
    public void SoulInit()    //�ҿ� x�� ����
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
    //Ǯ���� ��������
    public void ItemGet(int groundNum)   //�÷��̾ �ڽ��� ������
    {
   

        GameObject obj;

        //�ִٸ� ť���� ������ ���� 
        obj = queue.Dequeue();

        obj.transform.position = groundCh[groundNum].transform.position;
        obj.SendMessage("MyNum", groundNum, SendMessageOptions.DontRequireReceiver);
        obj.gameObject.SetActive(true);

    }
    [PunRPC]
    public void SoulGet(int groundNum)  //�ҿ￡ ������
    {

        GameObject obj;

        //�ִٸ� ť���� ������ ���� 
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


    //������
    //Ǯ�� ���ö�

    public void Relase(GameObject obj)
    {   //ť�� �ٽ� ������
        obj.gameObject.SetActive(false);   //�÷��̾ ������ false��Ű�� ť�� ����
        queue.Enqueue(obj);

        StartCoroutine(TenSec());    //x�� �ڷ�ƾ ����
    }

    //��ȥ ������Ʈ Ǯ

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
