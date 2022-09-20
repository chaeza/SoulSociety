using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New ResourceData", menuName = "ScriptableObjects/ResourceData", order = 5)]
public class ResourceData : ScriptableObject
{
    [Header("������ ������ ���")]
    public Image ItemIcon1 = null;
    public Image ItemIcon2 = null;
    public Image ItemIcon3 = null;
    public Image ItemIcon4 = null;
}

