using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;



public class Loading : MonoBehaviourPunCallbacks
{
    public Slider slider;
    public string SceneName;

    private float time;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
       photonView.StartCoroutine(LoadAsynSceneCoroutine());
    }

    IEnumerator LoadAsynSceneCoroutine()
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName);

        operation.allowSceneActivation = false;



        while (!operation.isDone)
        {

            time = +Time.time;

            slider.value = time / 10f;

            if (time > 10)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

    }

}
