using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleToGameScene : MonoBehaviour
{
    public string user_ID;
    public string session_ID;
    public string bets_ID;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void User_ID(string ID)
    {
        user_ID = ID;
    }
    public void Session_ID(string ID)
    {
        session_ID = ID;
    }
    public void Bets_ID(string ID)
    {
        bets_ID = ID;
    }

}
