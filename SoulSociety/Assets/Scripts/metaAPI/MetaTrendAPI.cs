using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using TMPro;

public class MetaTrendAPI : MonoBehaviour
{
	[Header("[등록된 프로젝트에서 획득가능한 API 키]")]
	[SerializeField] private string API_KEY = "";

	[Header("[Betting Backend Base URL]")]
	[SerializeField] private string fullAppsProductionURL = "https://odin-api.browseosiris.com";
	[SerializeField] private string fullAppsStagingURL = "https://odin-api-sat.browseosiris.com";
	[SerializeField] private string selectedBettingID;
	[SerializeField] private TMP_InputField txtInputField;

	private Res_BettingSetting resBettingSetting = null;
	private Res_UserProfile resUserProfile = null;
	private Res_UserSessionID resUserSessionID = null;

	
	private string GetBaseURL()
	{
		//when you are on Production Level
		//return FullAppsProductionURL;

		//when you are on Staging Level 스테이징 단계(개발)라면
		return fullAppsStagingURL;
	}

	//---------------
	// GetUserProfile 
	public void OnClickGetUserProfile()
	{
		StartCoroutine(ProcessRequestGetUserInfo());
	}
	IEnumerator ProcessRequestGetUserInfo()
	{
		// 유저 정보
		yield return RequestGetUserInfo((response) =>
		{
			if (response != null)
			{
				resUserProfile = response;
			}
		});
	}
	delegate void ResCallback_GetUserInfo(Res_UserProfile response);
	IEnumerator RequestGetUserInfo(ResCallback_GetUserInfo callback)
    {
		// get user profile
		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8546/api/getuserprofile");
		yield return www.SendWebRequest();
		Debug.Log(www.downloadHandler.text);
		txtInputField.text = www.downloadHandler.text;
		Res_UserProfile res_getUserProfile = JsonUtility.FromJson<Res_UserProfile>(www.downloadHandler.text);
		callback(res_getUserProfile);
	}

	//---------------
	// Get Session ID
	public void OnClick_GetSessionID()
	{
		StartCoroutine(ProcessRequestGetSessionID());
	}
	IEnumerator ProcessRequestGetSessionID()
	{
		// 유저 정보
		yield return requestGetSessionID((response) =>
		{
			if (response != null)
			{
				Debug.Log("## " + response.ToString());
				resUserSessionID = response;
			}
		});
	}
	delegate void ResCallback_GetSessionID(Res_UserSessionID response);
	IEnumerator requestGetSessionID(ResCallback_GetSessionID callback)
	{
		// get session id
		UnityWebRequest www = UnityWebRequest.Get("http://localhost:8546/api/getsessionid");
		yield return www.SendWebRequest();
		Debug.Log(www.downloadHandler.text);
		txtInputField.text = www.downloadHandler.text;
		Res_UserSessionID res_getSessionID = JsonUtility.FromJson<Res_UserSessionID>(www.downloadHandler.text);
		callback(res_getSessionID);
	}

	//---------------
	// 베팅관련 셋팅 정보를 얻어오기
	public void OnClick_Settings()
	{
		StartCoroutine(ProcessRequestSettings());
	}
	IEnumerator ProcessRequestSettings()
	{
		yield return RequestSettings((response) =>
		{
			if (response != null)
			{
				Debug.Log("## Settings : " + response.ToString());
				resBettingSetting = response;
				Debug.Log(resBettingSetting);
			}
		});
	}
	delegate void ResCallback_Settings(Res_BettingSetting response);
	IEnumerator RequestSettings(ResCallback_Settings callback)
	{
		string url = GetBaseURL() + "/v1/betting/settings";


		UnityWebRequest www = UnityWebRequest.Get(url);
		www.SetRequestHeader("api-key", API_KEY);
		yield return www.SendWebRequest();
		Debug.Log(www.downloadHandler.text);
		txtInputField.text = www.downloadHandler.text;
		Res_BettingSetting res = JsonUtility.FromJson<Res_BettingSetting>(www.downloadHandler.text);
		callback(res);
		//UnityWebRequest www = new UnityWebRequest(URL);
	}

	//---------------
	// Zera Check Balance
	public void OnClick_ZeraBalance()
	{
		StartCoroutine(ProcessRequestZeraBalance());
	}
	IEnumerator ProcessRequestZeraBalance()
	{
		yield return RequestZeraBalance(resUserSessionID.sessionId, (response) =>
		{
			if (response != null)
			{
				Debug.Log("## Response Zera Balance : " + response.ToString());
			}
		});
	}
	delegate void ResCallback_BalanceInfo(Res_ZeraBalance response);
	IEnumerator RequestZeraBalance(string sessionID, ResCallback_BalanceInfo callback)
	{
		string url = GetBaseURL() + ("/v1/betting/" + "zera" + "/balance/" + sessionID);

		UnityWebRequest www = UnityWebRequest.Get(url);
		www.SetRequestHeader("api-key", API_KEY);
		yield return www.SendWebRequest();
		Debug.Log(www.downloadHandler.text);
		txtInputField.text = www.downloadHandler.text;
		Res_ZeraBalance res = JsonUtility.FromJson<Res_ZeraBalance>(www.downloadHandler.text);
		callback(res);
		//UnityWebRequest www = new UnityWebRequest(URL);
	}

	//---------------
	// ZERA 베팅
	public void OnClick_Betting_Zera()
	{
		StartCoroutine(ProcessRequestBetting_Zera());
	}
	IEnumerator ProcessRequestBetting_Zera()
	{
		Res_Initialize resBettingPlaceBet = null;
		Req_Initialize reqBettingPlaceBet = new Req_Initialize();
		reqBettingPlaceBet.players_session_id = new string[] { resUserSessionID.sessionId };
		reqBettingPlaceBet.bet_id = selectedBettingID;// resSettigns.data.bets[0]._id;
		yield return RequestCoinPlaceBet(reqBettingPlaceBet, (response) =>
		{
			if (response != null)
			{
				Debug.Log("## CoinPlaceBet : " + response.message);
				resBettingPlaceBet = response;
			}
		});
	}
	delegate void ResCallback_BettingPlaceBet(Res_Initialize response);
	IEnumerator RequestCoinPlaceBet(Req_Initialize req, ResCallback_BettingPlaceBet callback)
	{
		string url = GetBaseURL() + "/v1/betting/" + "zera" + "/place-bet";

		string reqJsonData = JsonUtility.ToJson(req);
		Debug.Log(reqJsonData);


		UnityWebRequest www = UnityWebRequest.Post(url, reqJsonData);
		byte[] buff = System.Text.Encoding.UTF8.GetBytes(reqJsonData);
		www.uploadHandler = new UploadHandlerRaw(buff);
		www.SetRequestHeader("api-key", API_KEY);
		www.SetRequestHeader("Content-Type", "application/json");
		yield return www.SendWebRequest();

		Debug.Log(www.downloadHandler.text);
		txtInputField.text = www.downloadHandler.text;
		Res_Initialize res = JsonUtility.FromJson<Res_Initialize>(www.downloadHandler.text);
		callback(res);
	}

	//---------------
	// ZERA 베팅-승자
	public void OnClick_Betting_Zera_DeclareWinner()
	{
		StartCoroutine(ProcessRequestBetting_Zera_DeclareWinner());
	}
	IEnumerator ProcessRequestBetting_Zera_DeclareWinner()
	{
		Res_BettingWinner resBettingDeclareWinner = null;
		Req_BettingWinner reqBettingDeclareWinner = new Req_BettingWinner();
		reqBettingDeclareWinner.betting_id = selectedBettingID;// resSettigns.data.bets[0]._id;
		reqBettingDeclareWinner.winner_player_id = resUserProfile.userProfile._id;
		yield return RequestCoinDeclareWinner(reqBettingDeclareWinner, (response) =>
		{
			if (response != null)
			{
				Debug.Log("## CoinDeclareWinner : " + response.message);
				resBettingDeclareWinner = response;
			}
		});
	}
	delegate void ResCallback_BettingDeclareWinner(Res_BettingWinner response);
	IEnumerator RequestCoinDeclareWinner(Req_BettingWinner req, ResCallback_BettingDeclareWinner callback)
	{
		string url = GetBaseURL() + "/v1/betting/" + "zera" + "/declare-winner";

		string reqJsonData = JsonUtility.ToJson(req);
		Debug.Log(reqJsonData);


		UnityWebRequest www = UnityWebRequest.Post(url, reqJsonData);
		byte[] buff = System.Text.Encoding.UTF8.GetBytes(reqJsonData);
		www.uploadHandler = new UploadHandlerRaw(buff);
		www.SetRequestHeader("api-key", API_KEY);
		www.SetRequestHeader("Content-Type", "application/json");
		yield return www.SendWebRequest();

		Debug.Log(www.downloadHandler.text);
		txtInputField.text = www.downloadHandler.text;
		Res_BettingWinner res = JsonUtility.FromJson<Res_BettingWinner>(www.downloadHandler.text);
		callback(res);
	}

	//---------------
	// 베팅금액 반환
	public void OnClick_Betting_Zera_Disconnect()
	{
		StartCoroutine(ProcessRequestBetting_Zera_Disconnect());
	}
	IEnumerator ProcessRequestBetting_Zera_Disconnect()
	{
		Res_BettingDisconnect resBettingDisconnect = null;
		Req_BettingDisconnect reqBettingDisconnect = new Req_BettingDisconnect();
		reqBettingDisconnect.betting_id = selectedBettingID;// resSettigns.data.bets[1]._id;
		yield return RequestCoinDisconnect(reqBettingDisconnect, (response) =>
		{
			if (response != null)
			{
				Debug.Log("## CoinDisconnect : " + response.message);
				resBettingDisconnect = response;
			}
		});
	}
	delegate void ResCallback_BettingDisconnect(Res_BettingDisconnect response);
	IEnumerator RequestCoinDisconnect(Req_BettingDisconnect req, ResCallback_BettingDisconnect callback)
	{
		string url = GetBaseURL() + "/v1/betting/" + "zera" + "/disconnect";

		string reqJsonData = JsonUtility.ToJson(req);
		Debug.Log(reqJsonData);


		UnityWebRequest www = UnityWebRequest.Post(url, reqJsonData);
		byte[] buff = System.Text.Encoding.UTF8.GetBytes(reqJsonData);
		www.uploadHandler = new UploadHandlerRaw(buff);
		www.SetRequestHeader("api-key", API_KEY);
		www.SetRequestHeader("Content-Type", "application/json");
		yield return www.SendWebRequest();

		Debug.Log(www.downloadHandler.text);
		txtInputField.text = www.downloadHandler.text;
		Res_BettingDisconnect res = JsonUtility.FromJson<Res_BettingDisconnect>(www.downloadHandler.text);
		callback(res);
	}
}
