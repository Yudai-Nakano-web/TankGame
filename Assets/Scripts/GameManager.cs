using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{
   private GameObject[] enemyObjects;//敵の数を格納するための配列

    void Update(){
        enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");//Enemyタグを持つオブジェcウトを格納

        if(enemyObjects.Length == 0){//敵が全滅した時
            PlayerPrefs.SetInt("Stage1Clear",1);//ステージクリアのキーを保存
		}
    }
}
