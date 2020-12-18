using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System;

public class MyBehavior2 : MonoBehaviour
{
    public Text title;

    public InputField inputLoginID;
    public InputField inputLoginPW;

    public InputField inputSignID;
    public InputField inputSignPW;

    static public string UserID;
    static public string UserPW;

    public Text idRankText;
    public Text RankText;

    int MaxUser;

    String BASE_URL = "http://54.158.205.18/";


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startBtn()
    {
        StartCoroutine(PostLogin());
    }

    public void startLogin()
    {
        UserID = inputLoginID.text;
        UserPW = inputLoginPW.text;
        StartCoroutine(PostLogin());
    }

   public void startSignUP()
    {
        UserID = inputSignID.text;
        UserPW = inputSignPW.text;
        Debug.Log("userid: " + UserID + " PW: " + UserPW);
        StartCoroutine(PostSignup());
    }

    public void startInfoAdd()
    {
        StartCoroutine(PostInfo());

    }

    public void startRankScore()
    {
        StartCoroutine(GetUserRankScore());
    }

    public void startRankKill()
    {
        StartCoroutine(GetUserRankKill());
    }

    public void startUserRankScore()
    {
        StartCoroutine(PostUserScore());
    }

    public void startUserRankKill()
    {
        StartCoroutine(PostUserKill());
    }

    IEnumerator PostLogin()
    {
        WWWForm form = new WWWForm();
        form.AddField("userid", UserID);
        form.AddField("passwd", UserPW);

        UnityWebRequest www = UnityWebRequest.Post(BASE_URL+"login", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            string str = www.downloadHandler.text;

            JObject jo = JObject.Parse(str);

            Debug.Log(jo["status"]);

            int status = Int32.Parse((string)jo["status"]);

            if(status == 200)
            {
                ScenesMove sm = new ScenesMove();
                sm.GoStageChoose();
            }
            else
            {
                inputLoginID.text = "";
                inputLoginPW.text = "";

            }
        }
        
    }

    IEnumerator PostSignup()
    {
        WWWForm form = new WWWForm();
        form.AddField("userid", UserID);
        form.AddField("passwd", UserPW);

        Debug.Log("userid: " + UserID + " PW: " + UserPW);

        UnityWebRequest www = UnityWebRequest.Post(BASE_URL+"register", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            string body = www.downloadHandler.text;

            JObject jo = JObject.Parse(body);

            Debug.Log(jo);


            string oFlag = jo.SelectToken("psss").ToString();

            if (oFlag.Equals("False"))
            {
                inputSignID.text = "";
                inputSignPW.text = "";

                title.text = "중복입니다";
                Invoke("setTitle('Sign up')", 5);
            }
        }
    }

    void setTitle(string title)
    {
        this.title.text = title;
    }

    IEnumerator PostInfo()
    {
        WWWForm form = new WWWForm();
        form.AddField("userid", UserID);
        form.AddField("user_score", GameManager.Point);
        form.AddField("user_kill", GameManager.Kill);
        Debug.Log(UserID + " " + GameManager.Point + " " + GameManager.Kill);

        UnityWebRequest www = UnityWebRequest.Post(BASE_URL+"infoadd", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }

    IEnumerator PostUserScore()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest www = UnityWebRequest.Post(BASE_URL + "score", form);
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

            string Rank = www.downloadHandler.text;

            JObject jo = JObject.Parse(Rank);

            RankText.text = "";

            Debug.Log(jo);

            for (int i = 0; i < 1; i++)
            {
                RankText.text += jo["results"][i]["score"].ToString() + "\n";
            }
        }
    }

    IEnumerator PostUserKill()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest www = UnityWebRequest.Post(BASE_URL + "user_kill", form);
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

            string Rank = www.downloadHandler.text;

            JObject jo = JObject.Parse(Rank);

            RankText.text = "";

            Debug.Log(jo);

            for (int i = 0; i < 1; i++)
            {
                RankText.text += jo["results"][i]["kill"].ToString() + "\n";
            }
        }
    }

    IEnumerator GetUserRankScore()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest www = UnityWebRequest.Get(BASE_URL + "user_rank_score");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //받아오기

            string Rank = www.downloadHandler.text;

            JObject jo = JObject.Parse(Rank);

            Debug.Log(jo);

            idRankText.text = "";
            RankText.text = "";

            MaxUser = Int32.Parse(jo["count"].ToString());
            
            for(int i=0; i< MaxUser; i++)
            {
                idRankText.text += jo["results"][i]["userid"].ToString() + "\n";
                RankText.text += jo["results"][i]["user_score"].ToString()+ "\n";
            }
        }
    }

    IEnumerator GetUserRankKill()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest www = UnityWebRequest.Get(BASE_URL + "user_rank_kill");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string Rank = www.downloadHandler.text;

            JObject jo = JObject.Parse(Rank);

            idRankText.text = "";
            RankText.text = "";
            MaxUser = Int32.Parse(jo["count"].ToString());


            for (int i = 0; i < MaxUser; i++)
            {
                idRankText.text += jo["results"][i]["userid"].ToString() + "\n";
                RankText.text += jo["results"][i]["user_kill"].ToString() + "\n";
            }
        }
    }
}
