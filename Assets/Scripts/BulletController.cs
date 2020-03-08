using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour{

    //移動に関する変数
    public float speed;//速さ　インスペクターから入れる
    private Vector3 moveVector;//弾丸のVector3を格納
    private Rigidbody rb;//Rigidbodyコンポーネントを格納

    //跳ね返る時に使う変数
    private Vector3 objNomalVector = Vector3.zero;//法線ベクトル
    private int reflectCounter;//跳ね返る回数を数える
    public int reflectLimit;//跳ね返る回数を制限
    private Vector3 collisionPoint;//衝突地点の位置情報を格納

    //shotCounter関連の変数
    private GameObject player;//playerを格納
    private PlayerController playerControllerScript;//canonにアタッチされているスクリプトを代入

    //効果音関係の変数
    public AudioClip shellReflectSE;//跳ね返る時の音
    AudioSource audioSource;//コンポーネントを格納
    
    //エフェクト関係の変数
    public GameObject bulletExplosion;//砲弾の爆発エフェクト　インスペクターから入れる
    public GameObject tankExplosion;//戦車の爆発エフェクト　インスペクターから入れる

    //発射されてからの時間を測定するタイマー
    private float timer;

    void Start(){
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        audioSource = GetComponent<AudioSource>();//コンポーネントを格納

        player = GameObject.Find("Player");
        playerControllerScript = player.GetComponent<PlayerController>();//スクリプトを格納
    }
    void Update(){
        moveVector = rb.velocity;
        timer += Time.deltaTime;//発射されてからの時間を測定開始
    }
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Wall"){
            if(timer < 0.08){//発射されてすぐに壁と衝突した場合、弾を破壊
                playerControllerScript.DecreaseShotCount();
                
                Destroy(this.gameObject);

                GameObject effect = Instantiate(bulletExplosion,transform.position,Quaternion.identity);//砲弾の爆発エフェクトを生成
                Destroy(effect,1.5f);//再生した後削除
            }
            reflectCounter ++;//跳ね返った回数を数える
            if(reflectCounter > reflectLimit){//跳ね返る上限を決める
                //canonControllerScript.shotCounter --;
                playerControllerScript.DecreaseShotCount();
                
                Destroy(this.gameObject);

                GameObject effect = Instantiate(bulletExplosion,transform.position,Quaternion.identity);//砲弾の爆発エフェクトを生成
                Destroy(effect,1.5f);//再生した後削除
            }
            audioSource.PlayOneShot(shellReflectSE);//効果音再生
            objNomalVector = collision.contacts[0].normal;//法線ベクトル
            Vector3 reflectVec = Vector3.Reflect(moveVector,objNomalVector);//反射後のベクトル
            rb.velocity = reflectVec;//velocityを変化

            this.transform.rotation = Quaternion.LookRotation(reflectVec);//衝突後の進行方向に向きを変える
        }
        if(collision.gameObject.tag == "Bullet"){//別の弾と衝突した時
            playerControllerScript.DecreaseShotCount();

            //両方破壊
            Destroy(this.gameObject);
            Destroy(collision.gameObject);

            GameObject effect = Instantiate(bulletExplosion,transform.position,Quaternion.identity);//砲弾の爆発エフェクトを生成
            Destroy(effect,1.5f);//再生した後削除
        }
        if(collision.gameObject.tag == "Enemy"){
            playerControllerScript.DecreaseShotCount();

            //両方破壊
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
            GameObject effect = Instantiate(tankExplosion,transform.position,Quaternion.identity);//戦車の爆発エフェクトを生成
            Destroy(effect,1.5f);//再生した後削除
        }
        if(collision.gameObject.tag == "Player"){
            playerControllerScript.DecreaseShotCount();

            //両方破壊
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
            GameObject effect = Instantiate(tankExplosion,transform.position,Quaternion.identity);//戦車の爆発エフェクトを生成
            Destroy(effect,1.5f);//再生した後削除
        }
    }
}
