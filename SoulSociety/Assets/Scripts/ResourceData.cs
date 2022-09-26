using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New ResourceData", menuName = "ScriptableObjects/ResourceData", order = 5)]
public class ResourceData : ScriptableObject
{
    [Header("스킬 이펙트 목록")]
    public GameObject effobj = null;
    public GameObject stoneField = null;
    public GameObject spearCrash = null;
    public GameObject swordRain = null;
    public GameObject duelRoom = null;
    public int hp = 10;
}

