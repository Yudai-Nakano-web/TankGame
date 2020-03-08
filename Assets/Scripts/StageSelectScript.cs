using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectScript : MonoBehaviour{
    private GameObject menuManager;
    private int stage_num; //各ステージの番号を格納

    //各数字にあったステージのボタンをインスペクターから代入
    public GameObject two;
    public GameObject three;
    public GameObject four;
    public GameObject five;
    public GameObject six;
    public GameObject seven;
    public GameObject eight;
    public GameObject nine;
    public GameObject ten;
    
    void Start(){
       menuManager = GameObject.Find("MenuManager");
       stage_num = PlayerPrefs.GetInt ("SCORE");//現状のステージクリア状況を読み込む
    }
    void Update () {
        //stage_numが２以上のとき、ステージ２を解放する。以下同様
        if (stage_num >= 1) {
            two.SetActive (false);
        }

        if (stage_num >= 2) {
            three.SetActive (false);
        }

        if (stage_num >= 3) {
            four.SetActive (false);
        }

        if (stage_num >= 4) {
            five.SetActive (false);
        }
        if (stage_num >= 5) {
            six.SetActive (false);
        }
        if (stage_num >= 6) {
            seven.SetActive (false);
        }
        if (stage_num >= 7) {
            eight.SetActive (false);
        }
        if (stage_num >= 8) {
            nine.SetActive (false);
        }
        if (stage_num >= 9) {
            ten.SetActive (false);
        }

    }

    public void Stage1(){//ボタンのOnclickにインスペクターから代入
        Destroy(menuManager);
        SceneManager.LoadScene("Stage1");
    }
    public void Stage2(){//ボタンのOnclickにインスペクターから代入
        Destroy(menuManager);
        SceneManager.LoadScene("Stage2");
    }
    public void Stage3(){//ボタンのOnclickにインスペクターから代入        
        Destroy(menuManager);
        SceneManager.LoadScene("Stage3");
    }
    public void Stage4(){//ボタンのOnclickにインスペクターから代入
        Destroy(menuManager);
        SceneManager.LoadScene("Stage4");
    }
    public void Stage5(){//ボタンのOnclickにインスペクターから代入
        Destroy(menuManager);
        SceneManager.LoadScene("Stage5");
    }
    public void Stage6(){//ボタンのOnclickにインスペクターから代入
        Destroy(menuManager);
        SceneManager.LoadScene("Stage6");
    }
    public void Stage7(){//ボタンのOnclickにインスペクターから代入
        Destroy(menuManager);
        SceneManager.LoadScene("Stage7");
    }
    public void Stage8(){//ボタンのOnclickにインスペクターから代入
        Destroy(menuManager);
        SceneManager.LoadScene("Stage8");
    }
    public void Stage9(){//ボタンのOnclickにインスペクターから代入
        Destroy(menuManager);
        SceneManager.LoadScene("Stage9");
    }
    public void Stage10(){//ボタンのOnclickにインスペクターから代入
        Destroy(menuManager);
        SceneManager.LoadScene("Stage10");
    }
}
