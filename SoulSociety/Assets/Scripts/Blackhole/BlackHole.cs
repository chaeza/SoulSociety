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
        ran = Random.Range(0, 9);     //랜덤 으로 배열에 있는 것을 켜준다
    }

    private void Update()
    {
        if (zero != 7)                //블랙홀이 7번만 켜지게 만들었다
            time += Time.deltaTime;   

        if (time >= 20)
        {
            zero++;                          //7번 켤수있게 카운트해줌
            blackHole[ran].SetActive(true);  //블랙홀 켜주기

            time = 0;

            ran++;                          //블랙홀 랜덤에 그 옆에 있는게 켜지는 로직

            if(ran == 8)                    //만약 9가 된다면 0에서 켜진다
            {
                ran = 0;
            }
        }
    }
}

