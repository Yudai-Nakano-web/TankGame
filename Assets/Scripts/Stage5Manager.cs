using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage5Manager : MonoBehaviour{
   private GameObject[] enemyObjects;//敵の数を格納するための配列
   private GameObject[] playerObjects;//playerの数を数えるための変数
   public GameObject clearMenu;//クリア時のメニューをインスペクターから入れる
   public GameObject gameOverMenu;//ゲームオーバー時の画面をインスペクターから入れる
   public GameObject joyStick;//joystickをインスペクターから格納
   public　GameObject startUI;//ゲーム開始時の透明なボタン
   public GameObject stopMenu;//一時停止時の画面
   public GameObject pauseButton;//pauseボタンをアタッチ
   public Text tankCounter;
   public AudioClip clearSound;//クリア時に流れるBGM
   private AudioSource audioSource;
   private int stage_num;
    
    void Start(){
        tankCounter.text = 5.ToString();
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 0;
    }
    void Update(){
        enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");//Enemyタグを持つオブジェクトを格納
        playerObjects = GameObject.FindGameObjectsWithTag("Player");

        tankCounter.text = enemyObjects.Length.ToString();

        if(enemyObjects.Length == 0){//敵が全滅した時
            stage_num = PlayerPrefs.GetInt("SCORE");
            if(stage_num < 5){
                PlayerPrefs.SetInt ("SCORE", 5);//ステージセレクトのUIに関わる
                PlayerPrefs.Save ();
            }

            Destroy(joyStick);
            Destroy(pauseButton);

            clearMenu.gameObject.SetActive(true);//クリアー画面を表示する
            Time.timeScale = 0;
		}
        if(playerObjects.Length == 0){//playerがやられた時
            Destroy(joyStick);
            Destroy(pauseButton);
            gameOverMenu.gameObject.SetActive(true);//ゲームオーバー画面を表示する
            Time.timeScale = 0;
		}
    }
    public void Pause(){//stopボタンにインスペクター上からアタッチ
        Time.timeScale = 0;
        stopMenu.SetActive(true);//一時停止画面を表示する
        joyStick.SetActive(false);//joystickを一旦消す
    }
    public void Continue(){
        Time.timeScale = 1;
        stopMenu.SetActive(false);//一時停止画面を非表示する
        joyStick.SetActive(true);//joystickを一旦消す
    }
    public void tapToStart(){//インスペクター上からアタッチ
        Time.timeScale = 1;//ゲームが動く様になる
        startUI.SetActive(false);//非表示にする
    }
    public void Nextstage(){//nextボタンにインスペクターからアタッチ
        Debug.Log("Time.timeScale = " + Time.timeScale);
        SceneManager.LoadScene("Stage6");
    }
    public void Retry(){//retryボタンにインスペクターからアタッチ
        Debug.Log("Time.timeScale = " + Time.timeScale);
        SceneManager.LoadScene("Stage5");
    }
    public void Exit(){//exitボタンにインスペクターからアタッチ
        Debug.Log("Time.timeScale = " + Time.timeScale);
        SceneManager.LoadScene("StartScene");
    }
    void Clear(){
        audioSource.PlayOneShot(clearSound);
    }
}