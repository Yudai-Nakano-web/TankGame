using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    public float moveSpeed;//playerの速さ
    float moveX;//x成分の大きさ
    float moveZ;//z成分の大きさ
    private Rigidbody rb;
    public Joystick joystick;//Joystickスクリプト自体を格納
    private bool engine = false;//エンジン音を鳴らすかどうかを管理
    AudioSource audioSource;

    //大砲の発射音
    public AudioClip fireSound;

    //大砲関係の変数
    private GameObject canon;//大砲のオブジェクトを格納する
    private GameObject canonPivot;//砲塔のオブジェクトを格納する

    //弾関係の宣言
    [SerializeField] BulletController playerBulletPrefab;//生成する弾をインスペクターから代入する
    //弾の発射を制限するための変数
    private int shotCounter = 0;//打った数を数える
    public int shotLimit;//発射できる数を制限
    private bool destroy = false;//発射した弾が破壊されたかどうか検知する真偽値

    //タッチ数を数える変数
    private int touchCounter;

    //タッチされた場所によって、タッチの種類を振り分ける
    private Touch canonTouch;//大砲の操作をするタッチ
    private Touch joystickTouch;//joystickの操作をするタッチ

    void Start(){
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        canonPivot = transform.Find("p_Pivot").gameObject;//プレイヤーの子要素である、砲塔を取得
        canon = transform.Find("p_Pivot/PlayerCanon").gameObject;//プレイヤーの孫要素である、大砲を取得
    }
    void Update (){
        if(Time.timeScale == 0) return;//一時停止中は発射できない様にする

        Touch[] touches = Input.touches;//タッチ情報を格納する配列
        for (int i = 0; i < touches.Length; i++){//タッチしている指の数だけ下記振り分け処理を繰り返す
            if(125< touches[i].position.x && touches[i].position.x < 475 && 125 < touches[i].position.y && touches[i].position.y < 475){//ゲームパッド内の範囲をタッチした時
                joystickTouch = touches[i];//joysticktouchに振り分ける
            }else{
                canonTouch = touches[i];//canonTouchに振り分ける
            }
            if(i == 1) break;
        }
        Debug.Log("canonTouch.position ="+ canonTouch.position);
        Debug.Log("joystickTouch.position ="+ joystickTouch.position);

        if(canonTouch.phase == TouchPhase.Began){//タッチされた時だけ処理
            //大砲の向きに関わる部分
            Vector2 screenPos = Camera.main.WorldToScreenPoint( canonPivot.transform.position );//砲塔のスクリーン座標を格納
            Vector2 direction = canonTouch.position - screenPos;//playerからマウスの場所へのベクトル
            float angle = GetAim( Vector3.zero, direction );//上記のベクトルの原点から見た角度
            canonPivot.transform.rotation = Quaternion.AngleAxis(90-angle,Vector3.up);//大砲の根本を回転

            if(shotCounter == shotLimit) return;
            shotCounter ++;
            Debug.Log(shotCounter);
            if(shotCounter > shotLimit) return;//弾数を制限

            Instantiate(playerBulletPrefab,canon.transform.position + canon.transform.forward * 0.75f,canonPivot.transform.rotation);//弾を生成
            canon.gameObject.GetComponent<AudioSource>().PlayOneShot(fireSound);//canon自体に発射音を出させる
        }

        Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.forward * joystick.Vertical);//joystickの動かす向き

        if (moveVector != Vector3.zero){//joystickに何らかの入力がある場合
            
            transform.rotation = Quaternion.LookRotation(moveVector);//joystickの傾いた向きをオブジェクトが向く
            
            moveX = moveVector.x * moveSpeed;
            moveZ = moveVector.z * moveSpeed;
            rb.velocity = new Vector3(moveX,0,moveZ);

            if(engine == false){//ゲームパッド入力中に１度だけ処理させる
                audioSource.Play();//移動音
                engine = true;
            }
        
        }else{
            rb.velocity = Vector3.zero;//ゲームパッドへの入力がない時に慣性で動かない様にする
            audioSource.Stop();//移動している時だけ音を出す
            engine = false;
        }
        if(Time.timeScale == 0){
            audioSource.Stop();//ゲーム終了時音を止める
            Debug.Log("STOP");
        }
    }
    public float GetAim( Vector2 from, Vector2 to ){//2点間の差角を度単位で返す関数
        float dx = to.x - from.x;//x座標の差分
        float dy = to.y - from.y;//y座標の差分
        float rad = Mathf.Atan2(dy, dx);//逆関数で差角のラジアンを求める
        return rad * Mathf.Rad2Deg;//ラジアン単位ではなく、度単位で返す
    }
    public void DecreaseShotCount(){//弾が破壊されたか検知
        shotCounter --;
    }
}
