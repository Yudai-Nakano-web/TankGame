using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour{
    //追跡するAIに関する変数
    private NavMeshAgent enemy;
    public GameObject target;//playerをインスペクター上からいれる
    public bool following;//敵によって追跡するか変えられる

    //移動に関する変数
    [SerializeField] float posrange;//徘徊中に設定する範囲
    private Vector3 randompos;//追跡する対象 徘徊の時に使用
    public bool move;//戦車が動くかどうか決定する

    //追跡するかどうかを決定する距離情報を格納
    [SerializeField] float changeTargetDistance;//ターゲットを変える距離をインスペクターからいれる　２乗した値と比較するので注意
    private float targetDistance;//ターゲットとの距離を２乗した値を格納 処理を少しでも軽くするため

    //徘徊中に使う変数
    private float timer;//時間を計測
    [SerializeField] float changeTargetInterval;//徘徊する目的地を変更する頻度
    private bool fixedRandomPos;//徘徊中にvoid UpdateでrandomPosを毎フレーム処理できない様にする

    //大砲の発射を管理する変数
    public GameObject canon;//インスペクターからCanonをいれる
    private CanonController canonController;//CanonについてるスクリプトCanonControllerを格納
    public int fireDistance;//止まっている敵がplayerに発砲する距離
    //移動している時に出る音を出すための変数
    AudioSource audioSource;
    private bool engine = false;//Realize状態で１回のみ処理される様にする

    //playerがやられた時、動かない様にする
    private GameObject[] playerObjects;

    Vector3 GetRandomPosition(Vector3 currentpos){//徘徊するときの目的地を取得させる関数　引数にthis.transform.positionを入れて使う
        return new Vector3(Random.Range(-posrange + currentpos.x, posrange + currentpos.x), 0, Random.Range(-posrange + currentpos.z, posrange + currentpos.z));
    }


    void NotRealize(){//徘徊中に処理させたい
        randompos = GetRandomPosition(transform.position);//目的地をランダムに設定
        enemy.destination = randompos;//徘徊用の目的地を設定
        if(engine == false){
            audioSource.Play();
            engine = true;
        }
    }
    void Realize(){//追跡中に処理
        enemy.destination  = target.transform.position;//targetを目的地に設定
        canonController.Fire();
        if(engine == false){//Realize中１度だけ処理させる
            audioSource.Play();
            engine = true;
        }
    }
    void Start () {
        enemy  = gameObject.GetComponent<NavMeshAgent>();//NavMeshのコンポーネントを取得
        canonController = canon.GetComponent<CanonController>();//スクリプトを取得
        audioSource = GetComponent<AudioSource>();//AudioSourceコンポーネントを取得

        fixedRandomPos = false;
    }

    void Update () {
        if(Time.timeScale == 0){//一時停止中などは、音を止める
            audioSource.Stop();
        }

        targetDistance = Vector3.SqrMagnitude(transform.position - target.transform.position);//ターゲットとの距離を２乗した値を格納
        timer += Time.deltaTime;//タイマーを秒単位でカウント
        if(timer > changeTargetInterval){//一定時間で目的地を更新できる様にする
            fixedRandomPos = false;
            timer = 0;//timerを元に戻す
        }
        if(move){//動く性質を持っている場合
            if(following){//敵が追跡する性質を持つ行動パターンを持っている場合
                if(targetDistance < changeTargetDistance){//ターゲットとの距離が近くなった場合
                    Realize();//追跡する
                }else if(fixedRandomPos == false){
                    engine = false;
                    fixedRandomPos = true;
                    NotRealize();//徘徊する
                }
            }else{//どんな敵も徘徊する
                fixedRandomPos = true;
                NotRealize();//徘徊する
                if(targetDistance < fireDistance){//射程範囲内に入ったら
                    canonController.Fire();
                }
            }
        }else{//動かない敵
            if(targetDistance < fireDistance){//射程範囲内に入ったら
                canonController.Fire();
            }
        }
    }
}
