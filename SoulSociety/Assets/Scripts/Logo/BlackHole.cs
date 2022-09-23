using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField] GameObject[] blackHole = null;
    float time = 0;
    int ran;
    int zero;


    private void Start()
    {
        int ran = Random.Range(0, 9);

        Debug.Log(ran);
    }


    private void Update()
    {
        if (zero != 7)
            time += Time.deltaTime;

          // Debug.Log(time);
        

        if (time >= 20)
        {

            zero++;
            blackHole[ran].SetActive(true);

            time = 0;

            ran++;

            if (ran == -1)
            {
                ran = 8;
            }

            if (ran == 9)
            {
                ran = 1;
            }
        }




    }

    IEnumerator ActiveTrue()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            Debug.Log("5ÃÊ Áö³²~");
            blackHole[ran].SetActive(true);
            // yield break;
        }


    }
}

