using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesChange : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Next());
    }

    IEnumerator Next()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);  // 1 ¹øÂ° ¾À ·Îµå
    }


}
