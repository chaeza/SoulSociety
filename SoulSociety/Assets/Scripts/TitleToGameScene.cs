using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TitleToGameScene : MonoBehaviour
{ 
    public string user_ID;
    public string session_ID;
    public string bets_ID;

    public string[] all_Session_ID = new string[4];
    List<string> all_Sessions = new List<string>();


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
