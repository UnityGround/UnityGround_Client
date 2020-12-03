using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MyBehavior : MonoBehaviour
{

    public InputField inputID;
    public InputField inputPW;

    static public string UserID;
    static public string UserPW;

    public void Login()
    {
        UserID = inputID.text;
        UserPW = inputPW.text;
        Debug.Log("UserID: " + UserID+" UserPW: "+UserPW);
        StartCoroutine(PostLogin());
        //StartCoroutine(Upload());
        SceneManager.LoadScene("0_StageChoose");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region 회원가입
    IEnumerator PostRegister()
    {
        // 폼  생성 보내기
        WWWForm form = new WWWForm();
        form.AddField("userid", UserID);
        form.AddField("passwd", UserPW);

        UnityWebRequest www = UnityWebRequest.Post("http://10.53.68.252:3000/head", form);
        www.SetRequestHeader("userid", UserID);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //받아오기
            Debug.Log(www.downloadHandler.text);


        }
    }
    #endregion

    IEnumerator Upload()
    {
        // 폼  생성 보내기
        WWWForm form = new WWWForm();
        form.AddField("score", GameManager.Point);
        form.AddField("user_kill", GameManager.Kill);
        form.AddField("time_user", GameManager.UTime.ToString());
        Debug.Log(GameManager.Point + " "+ GameManager.Kill + " " + GameManager.UTime);

        UnityWebRequest www = UnityWebRequest.Post("http://10.53.68.252:3000/head", form);
        www.SetRequestHeader("userid", UserID);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //받아오기
            Debug.Log(www.downloadHandler.text);

            
        }
    }

    #region 로그인
    IEnumerator PostLogin()
    {
        WWWForm form = new WWWForm();
        form.AddField("userid", UserID);
        form.AddField("passwd", UserPW);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:3000/login", form);
        www.SetRequestHeader("userid", inputID.text);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //받아오기
            Debug.Log("aa");        }
    }
    #endregion

    #region Info추가
    IEnumerator PostInfoadd()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest www = UnityWebRequest.Post("http://10.53.68.252:3000/login", form);
        form.AddField("user_score", GameManager.Point);
        form.AddField("user_kill", GameManager.Kill);
        www.SetRequestHeader("userid", UserID);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //받아오기
            Debug.Log(www.downloadHandler.text);
        }
    }
    #endregion

    #region 유저 별 총 스코어
    IEnumerator PostScore()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest www = UnityWebRequest.Post("http://10.53.68.252:3000/login", form);
        www.SetRequestHeader("userid", UserID);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //받아오기
            Debug.Log(www.downloadHandler.text);
        }
    }
    #endregion

    #region 유저 별 총 킬 수
    IEnumerator PostUserKill()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest www = UnityWebRequest.Post("http://10.53.68.252:3000/login", form);
        www.SetRequestHeader("userid", UserID);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //받아오기
            Debug.Log(www.downloadHandler.text);
        }
    }
    #endregion

    #region 유저 별 순위 (스코어)
    IEnumerator GetUserRankScore()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/user_rank_score");
        Debug.Log("받았니?");
        yield return www.SendWebRequest();
        Debug.Log("받았냐구?");

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Debug.Log("에러?");
        }
        else
        {
            //받아오기
            Debug.Log(www.downloadHandler.text);
            Debug.Log("좀 받아라");
        }
    }
    #endregion

    #region 유저 별 순위 (킬)
    IEnumerator GetUserRankKill()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest www = UnityWebRequest.Get("http://10.53.68.252:3000/user_rank_score");
        Debug.Log("받았니?");
        yield return www.SendWebRequest();
        Debug.Log("받았냐구?");

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Debug.Log("에러?");
        }
        else
        {
            //받아오기
            Debug.Log(www.downloadHandler.text);
            Debug.Log("좀 받아라");
        }
    }
    #endregion


}
