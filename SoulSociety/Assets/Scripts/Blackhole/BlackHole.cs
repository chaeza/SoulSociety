using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField] GameObject[] blackHole = null;
    float time = 0;
    int ran;
   public int zero;


    private void Start()
    {
        ran = Random.Range(0, 9);     //���� ���� �迭�� �ִ� ���� ���ش�
    }

    private void Update()
    {
        if (zero != 7)                //��Ȧ�� 7���� ������ �������
            time += Time.deltaTime;   

        if (time >= 20)
        {
            zero++;                          //7�� �Ӽ��ְ� ī��Ʈ����
            blackHole[ran].SetActive(true);  //��Ȧ ���ֱ�

            time = 0;

            ran++;                          //��Ȧ ������ �� ���� �ִ°� ������ ����

            if(ran == 8)                    //���� 9�� �ȴٸ� 0���� ������
            {
                ran = 0;
            }
        }
    }
}

