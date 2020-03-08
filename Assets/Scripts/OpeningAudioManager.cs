using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningAudioManager : MonoBehaviour{
    public bool DontDestroyEnabled = true;//シーン遷移をしてもBGMが終わらない様に管理する

    void Start () {
        if (DontDestroyEnabled) {// Sceneを遷移してもオブジェクトが消えないようにする
            DontDestroyOnLoad (this);
        }
    }
    public void StageSelect(){//インスペクターのonClickから代入
        SceneManager.LoadScene("StageSelect");
    }
}
