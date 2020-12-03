using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static public int Point;
    static public int stageIndex=1;
    static public int Kill;
    static public float UTime;
    public int health = 3;
    static public int Shealth = 3;
    static public Player player;

    static public Image[] UIhealth = new Image[3];
    static public Text UIPoint;
    static public Text UIStage;
    static public string stageStr = "STAGE1";

    static public Button RestartBtn;
    static public Button NextStageBtn;
    static public Button MainBtn;
    static public Button StageChooseBtn;

    public Text UserName;

    static public Text TimerText;
    static public float Timer = 0;

    int finishStage = 3;

    MyBehavior2 my;

    // 다음 스테이지로
    public void NextStage()
    {
        Debug.Log("다음스테이지로!");
        if (stageIndex == 0)
        {
            MainBtn.gameObject.SetActive(true);
        }
        else if (stageIndex == finishStage)
        {
            StageChooseBtn.gameObject.SetActive(true);
        }
        else
        {
            stageIndex++;
            NextStageBtn.gameObject.SetActive(true);
            StageChooseBtn.gameObject.SetActive(true);
        }
        Debug.Log(stageStr);

        Time.timeScale = 0;
        TimerText.text = Timer.ToString("N0") + "s";
        TimerText.gameObject.SetActive(true);
        
    }

    #region OnTriggerEnter2D
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (health > 1)
            {
                collision.attachedRigidbody.velocity = Vector2.zero;
                collision.transform.position = new Vector3(-5, -1, -1);
            }

            Debug.Log("HealthDown()");
            HealthDown();
        }
    }*/
    #endregion 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (health > 1)
            {
                collision.rigidbody.velocity = Vector2.zero;
                collision.transform.position = new Vector3(-5, -1, -1);
            }
            HealthDown();
        }
    }


     public void HealthDown()
    {
        if(health > 1)
        {
            health = --Shealth;
            Debug.Log(health + " : " + Shealth);
            UIhealth[Shealth].color = new Color(1, 0, 0, 0.4f);
        }
        else
        {
            Debug.Log(health + " : " + Shealth);
            UIhealth[0].color = new Color(1, 0, 0, 0.4f);
            player.OnDie();
        }
    }

    static public void SHealthDown()
    {
        --Shealth;
    }

    private void Awake()
    {
        Debug.Log("StageIndex" + stageIndex);

        player = GameObject.Find("Player").GetComponent<Player>();

        UIhealth[0] = GameObject.Find("Health1").GetComponent<Image>();
        UIhealth[1] = GameObject.Find("Health2").GetComponent<Image>();
        UIhealth[2] = GameObject.Find("Health3").GetComponent<Image>();

        UIPoint = GameObject.Find("PointText").GetComponent<Text>();

        UIStage = GameObject.Find("StageText").GetComponent<Text>();

        RestartBtn = GameObject.Find("RestartBtn").GetComponent<Button>();
        RestartBtn.gameObject.SetActive(false);

        NextStageBtn = GameObject.Find("NextStageBtn").GetComponent<Button>();
        NextStageBtn.gameObject.SetActive(false);

        MainBtn = GameObject.Find("MainBtn").GetComponent<Button>();
        MainBtn.gameObject.SetActive(false);

        StageChooseBtn = GameObject.Find("StageChooseBtn").GetComponent<Button>();
        StageChooseBtn.gameObject.SetActive(false);

        TimerText = GameObject.Find("PlayTime").GetComponent<Text>();
        TimerText.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        UserName.text = MyBehavior2.UserID;
        Debug.Log(UserName.text + " : " + MyBehavior2.UserID);

        my = new MyBehavior2();

        Point = 0;
        Kill = 0;
        health = 3;
        Shealth = 3;
    }

    // Update is called once per frame
    void Update()
    {
        UIPoint.text = Point.ToString()+ "p";
        UIStage.text = stageStr;
        health = Shealth;

        // 타이머
        Timer += Time.deltaTime;
    }

    public void Restart()
    {
        if (stageIndex != 0)
            SceneManager.LoadScene("1_Stage0" + stageIndex);
        else
            SceneManager.LoadScene("0_Tutorial");
        
        Time.timeScale = 1;
        Point = 0;
        Kill = 0;
        health = 3;
        Shealth = 3;
    }

    public void NextStageStart()
    {
        SceneManager.LoadScene("1_Stage0" + stageIndex);
        Time.timeScale = 1;
        Point = 0;
        Kill = 0;
        health = 3;
        Shealth = 3;
        stageStr = "STAGE" + stageIndex;
    }

}
