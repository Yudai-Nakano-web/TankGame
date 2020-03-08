using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour{
   //移動に関する変数
    public float speed;//速さ　インスペクターから入れる
    private Vector3 moveVector;//弾丸のVector3を格納
    private Rigidbody rb;//Rigidbodyコンポーネントを格納

    //跳ね返る時に使う変数
    private Vector3 objNomalVector = Vector3.zero;//法線ベクトル
    private int reflectCounter;//跳ね返る回数を数える
    public int reflectLimit;//跳ね返る回数を制限
    private Vector3 collisionPoint;//衝突地点の位置情報を格納

    //効果音関係の変数
    public AudioClip shellReflect;//跳ね返る音
    AudioSource audioSource;//コンポーネント格納

    //爆発のエフェクト
    public GameObject bulletExplosion;//砲弾の爆発エフェクト　インスペクターから入れる
    public GameObject tankExplosion;//戦車の爆発エフェクト　インスペクターから入れる
    
    void Start(){
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        audioSource = GetComponent<AudioSource>();//コンポーネント取得
    }
    void Update(){
        moveVector = rb.velocity;
    }
    void OnCollisionEnter(Collision collision){//衝突判定
        if(collision.gameObject.tag == "Wall"){
            reflectCounter ++;//跳ね返った回数を数える
            if(reflectCounter > reflectLimit){//跳ね返る上限を決める
                Destroy(this.gameObject);

                GameObject effect = Instantiate(bulletExplosion,transform.position,Quaternion.identity);//砲弾の爆発エフェクトを生成
                Destroy(effect,1.5f);//再生した後削除
            }
            audioSource.PlayOneShot(shellReflect);
            objNomalVector = collision.contacts[0].normal;//法線ベクトル
            Vector3 reflectVec = Vector3.Reflect(moveVector,objNomalVector);//反射後のベクトル
            rb.velocity = reflectVec;//velocityを変化

            this.transform.rotation = Quaternion.LookRotation(reflectVec);//衝突後の進行方向に向きを変える

            foreach (ContactPoint point in collision.contacts){
            collisionPoint = point.point;//衝突地点を格納
            }
        }
        if(collision.gameObject.tag == "Bullet"){//別の弾と衝突した時
            //両方破壊
            Destroy(this.gameObject);
            Destroy(collision.gameObject);

            GameObject effect = Instantiate(bulletExplosion,transform.position,Quaternion.identity);//砲弾の爆発エフェクトを生成
            Destroy(effect,1.5f);//再生した後削除
        }
        if(collision.gameObject.tag == "Enemy"){
            //両方破壊
            Destroy(this.gameObject);
            Destroy(collision.gameObject);

            GameObject effect = Instantiate(tankExplosion,transform.position,Quaternion.identity);//戦車の爆発エフェクトを生成
            Destroy(effect,1.5f);//再生した後削除
        }
        if(collision.gameObject.tag == "Player"){
            //両方破壊
            Destroy(this.gameObject);
            Destroy(collision.gameObject);

            GameObject effect = Instantiate(tankExplosion,transform.position,Quaternion.identity);//戦車の爆発エフェクトを生成
            Destroy(effect,1.5f);//再生した後削除
        }
    }
}
