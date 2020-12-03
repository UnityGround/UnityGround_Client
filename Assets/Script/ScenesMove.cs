using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesMove : MonoBehaviour
{
    public AudioClip _playBgm;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = _playBgm;
        audioSource.Play();
    }

    private void OnDestroy()
    {
        audioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Scene 이동 메서드
    // Tutorial
    public void GoTutorial()
    {
        SceneManager.LoadScene("0_Tutorial");
        GameManager.stageIndex = 0;
    }

    // Main
    public void GoMain()
    {
        SceneManager.LoadScene("0_Lobby");
        GameManager.stageIndex = 0;
        Time.timeScale = 1;
    }

    // Stage Choose
    public void GoStageChoose()
    {
        SceneManager.LoadScene("0_StageChoose");
        Time.timeScale = 1;
    }

    public void GoRankAll()
    {
        SceneManager.LoadScene("2_RankAll");
    }
    public void GoRankUser()
    {
        SceneManager.LoadScene("2_RankUser");
    }

    #region 스테이지 고르기
    public void GoStage1()
    {
        SceneManager.LoadScene("1_Stage01");
        GameManager.stageIndex = 1;
    }
    public void GoStage2()
    {
        SceneManager.LoadScene("1_Stage02");
        GameManager.stageIndex = 2;
    }

    public void GoStage3()
    {
        SceneManager.LoadScene("1_Stage03");
        GameManager.stageIndex = 3;
    }
    #endregion

    #endregion
}
