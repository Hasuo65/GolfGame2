using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

/// <summary>
/// プレーヤーの操作関係
/// </summary>
public class PlayerController : MonoBehaviourPunCallbacks
{
    Vector2 initialVec;//クリックした時の座標
    Vector2 fireVec;//発射するベクトル
    [SerializeField] private GameObject arrowImage;
    [SerializeField] private GameObject initialPoint;//initialVecに置く点のオブジェクト
    private GameObject ip;//initialPointのインスタンス

    [System.NonSerialized]public bool IsOnDragZone = false;//ViewZoneCOntrollerで操作する

    private bool isStop = false;//ボールが打った後止まっているかのbool
    [SerializeField] private int maxHitTime;
    private int hitTime = 0;

    private Rigidbody2D rb;//アバターのRigidBody

    private SpriteRenderer sprite;//アバターのSpriteRenderer
    private GameObject ball;//ボールのオブジェクト

    [SerializeField] private bool simulateTrail;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ball = transform.GetChild(0).transform.gameObject;
        sprite = ball.GetComponent<SpriteRenderer>();
        hitTime = maxHitTime;//最初は打つことができないようにする
    }

    private void Update()
    {
        CheckStopped();
        if (IsOnDragZone&& hitTime < maxHitTime)
        {
            if (Input.GetMouseButton(0))
            {
                DuringClick();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnClickExit();
            }
        }
    }

    private bool firstStop = true;//止まった時に一回だけ呼ぶためのbool
    private float stopTimer;//止まった後の時間

    private void CheckStopped()//止まるまで色を変えないようにする
    {
        if(rb.velocity.magnitude <= 0.03f)
        {
            if (!isStop)//ボールを打った後カウントする
            {
                stopTimer += Time.deltaTime;
            }
            if (firstStop && stopTimer >= 1)
            {
                //---ここに止まった時の処理を書く
                sprite.color = Color.white;
                hitTime = 0;
                //----
                firstStop = false;
                isStop = true;
                stopTimer = 0;
            }
        }
        else
        {
            sprite.color = Color.red;
        }
    }

    private bool clickFirst = true;//一番最初のフレームを判定するbool

    private GameObject arrowObjectInstance;//矢印のインスタンス

    public void DuringClick()
    {
        if (clickFirst)//初めのフレームはこれが呼び出される
        {
            initialVec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            arrowObjectInstance = Instantiate(arrowImage, (Vector2)transform.position, Quaternion.FromToRotation(initialVec, fireVec) * Quaternion.Euler(0, 0, 90));
            ip = Instantiate(initialPoint, initialVec, Quaternion.identity);
            clickFirst = false;
            if (simulateTrail)
            {
                BeginSimulation();
            }
            return;
        }

        //次のフレームから呼び出される
        fireVec = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - initialVec;
        arrowObjectInstance.transform.position = transform.position;//矢印の操作
        arrowObjectInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, fireVec.normalized) * Quaternion.Euler(0, 0, -90);//矢印の操作
        arrowObjectInstance.transform.localScale = new Vector2(0.3f, 0.1f) * fireVec.magnitude;//矢印の操作
        if (simulateTrail)
        {
            DuringSimulation();
        }
    }

    public void OnClickExit()//ドラッグをやめたら
    {
        Destroy(arrowObjectInstance);
        Destroy(ip);
        fireVec = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - initialVec;
        OwnerShoot(fireVec);
        photonView.RPC(nameof(Shoot), RpcTarget.Others, fireVec, (Vector2)transform.position);
    }

    private void OwnerShoot(Vector2 force)//ボールの所有者の処理
    {
        rb.AddForce(force * -50);
        ExitGames.Client.Photon.Hashtable hashTable = PhotonNetwork.LocalPlayer.CustomProperties;
        hashTable["hit"] = (int)hashTable["hit"] + 1;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashTable);
        hitTime++;
        isStop = false;
        firstStop = true;
        clickFirst = true;
    }

    [PunRPC]//ほかの人の処理
            //PunRPCについて https://zenn.dev/o8que/books/bdcb9af27bdd7d/viewer/2e3520
    private void Shoot(Vector2 force, Vector2 position)
    {
        rb.AddForce(force * -50);
        transform.position = position;//ラグを防ぐ
    }

    private void OnTriggerEnter2D(Collider2D collision)//ゴール判定はプレイヤーごとに行う
    {
        if (collision.tag == "Goal"&&photonView.Owner == PhotonNetwork.LocalPlayer)
        {
            ExitGames.Client.Photon.Hashtable hashTable = photonView.Owner.CustomProperties;
            hashTable["score"] = (int)hashTable["score"] + 1;
            photonView.Owner.SetCustomProperties(hashTable);
        }
    }


    //unity 2dでの弾道予測 https://www.matatabi-ux.com/entry/2018/11/08/100000

    /// <summary>
    /// 運動軌跡
    /// </summary>
    [SerializeField]
    private LineRenderer simulationLine = null;

    /// <summary>
    /// 最大付与力量
    /// </summary>
    private const float MaxMagnitude = 2f;

    /// <summary>
    /// 発射方向の力
    /// </summary>
    private Vector3 currentForce = Vector3.zero;

    /// <summary>
    /// メインカメラ
    /// </summary>
    private Camera mainCamera = null;

    /// <summary>
    /// メインカメラ座標
    /// </summary>
    private Transform mainCameraTransform = null;

    /// <summary>
    /// ドラッグ開始点
    /// </summary>
    //private Vector2 dragStart = Vector2.zero;

    /// <summary>
    /// ボール位置
    /// </summary>
    private Vector2 currentPosition = Vector2.zero;

    /// <summary>
    /// 固定フレームウェイト
    /// </summary>
    private static float DeltaTime;

    /// <summary>
    /// 固定フレーム待ち時間
    /// </summary>
    private static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Awake()
    {
/*        mainCamera = Camera.main;
        mainCameraTransform = mainCamera.transform;*/
        DeltaTime = Time.fixedDeltaTime;
    }

    /// <summary>
    /// マウス座標をワールド座標に変換して取得
    /// </summary>
    /// <returns></returns>
/*    private Vector3 GetMousePosition()
    {
        // マウスから取得できないZ座標を補完する
        var position = Input.mousePosition;
        position.z = mainCameraTransform.position.z;
        position = mainCamera.ScreenToWorldPoint(position);
        position.z = 0;

        return position;
    }*/

    /// <summary>
    /// ドラック開始イベントハンドラ
    /// </summary>
    public void BeginSimulation()
    {
        currentPosition = rb.position;
    }

    /// <summary>
    /// ドラッグ中イベントハンドラ
    /// </summary>
    public void DuringSimulation()
    {
        //var position = GetMousePosition();

/*        currentForce = position - dragStart;
        if (currentForce.magnitude > MaxMagnitude * MaxMagnitude)
        {
            currentForce *= MaxMagnitude / currentForce.magnitude;
        }*/

        StartCoroutine(Simulation());
    }

    /// <summary>
    /// 軌跡を予測して描画するコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator Simulation()
    {

        // 自動的な物理運動を停止させる
        Physics.simulationMode = SimulationMode.Script;

        var points = new List<Vector2> { currentPosition };
        rb.AddForce(fireVec*-50,ForceMode2D.Impulse);

        // 運動の軌跡をシミュレーションして記録する
        for (var i = 1; i < 16; i++)
        {
            Physics.Simulate(1);
            points.Add(transform.TransformPoint(transform.position));
            Debug.Log(transform.TransformPoint(transform.position));
        }

        // もとの位置に戻す
        rb.velocity = Vector2.zero;
        transform.position = currentPosition;

        // 予測地点をつないで軌跡を描画
        simulationLine.positionCount = points.Count;
        List<Vector3> vec3List = new List<Vector3>();
        foreach(Vector2 point in points)
        {
            vec3List.Add(point);
        }
        simulationLine.SetPositions(vec3List.ToArray());

        Physics.simulationMode = SimulationMode.FixedUpdate;

        yield return WaitForFixedUpdate;
    }

    /*
        /// <summary>
        /// ドラッグ終了イベントハンドラ
        /// </summary>
        public void OnShoot()
        {
            Flip(fireVec * 6f);
        }

        /// <summary>
        /// ボールをはじく
        /// </summary>
        /// <param name="force"></param>*/
/*    public void Flip(Vector3 force)
    {
        // 瞬間的に力を加えてはじく
        this.rb.AddForce(force, ForceMode.Impulse);
    }*/
}