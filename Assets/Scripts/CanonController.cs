using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonController : MonoBehaviour{
    [SerializeField] GameObject player;//Playerを格納
    private Transform p_Transform;//playerの位置情報を格納
    [SerializeField] bool enemyCanon;//敵の大砲
    
    //砲塔の回転に関する変数
    [SerializeField] GameObject e_Pivot;//enemyの大砲が回転する軸　インスペクターからいれる

    //弾関係の宣言
    [SerializeField] EnemyBulletController enemyBulletPrefab;//生成する弾をインスペクターから代入する
    private float shotTimer;//全戦車の弾の発射頻度を管理するタイマー
    [SerializeField] float e_shotInterval;//弾の発射頻度

    //効果音関係の変数
    public AudioClip fireSound;
    AudioSource audioSource;

    void Start(){
        audioSource = GetComponent<AudioSource>();//AudioSourceコンポーネントを取得
    }

    void Update(){
        if(Time.timeScale == 0) return;//一時停止中は発射できない様にする
        p_Transform = player.transform;//playerの位置情報

    }

    public void Fire(){//敵の大砲の発射を制御する関数　EnemyControllerで使う
        if(enemyCanon){
            e_Pivot.transform.rotation = Quaternion.LookRotation(p_Transform.position - e_Pivot.transform.position);//playerの方向を向く

            shotTimer += Time.deltaTime;//タイマーを設定
            if(shotTimer > e_shotInterval){//一定間隔で弾を発射
                Instantiate(enemyBulletPrefab,this.transform.position + this.transform.forward * 0.75f,this.transform.rotation);//弾を生成
                audioSource.PlayOneShot(fireSound);//発射音を鳴らす
                shotTimer = 0;
            }
        }
    }
}
