using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

/// <summary>
/// ゲームのシステムについてのクラス
/// </summary>
public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform spawnPosition;//プレイヤーのスタート座標
    public static List<GameObject> playerAvatars = new List<GameObject>();//プレイヤーのボールのリスト
    [SerializeField] private GameObject nameInput;//プレイヤー名のInputFieldのオブジェクト
    [System.NonSerialized] public string avatarName;//プレイヤーが選択したアバターの名前
    [SerializeField] private GameObject readyButton;//Readyのボタン

    [SerializeField] public GameObject playerScorePanelObject;//プレイヤーのスコアを表示するパネルをアタッチする    StartでplayerScorePanelに割り当てる
    public static GameObject playerScorePanel;

    [SerializeField] private CountDown countDown;
    [SerializeField] private int timeLimit;

    [SerializeField] private GameObject playerList;

    [System.NonSerialized]public RoomGameManager roomGameManager;//RoomGameNanagerのインスタンス

    public enum GameState {
        game,//実際のゲーム時
        prop,//障害物を置く時
        gameOver//ゲームオーバー
    }
    public static GameState gameState = GameState.game;

    public override void OnJoinedRoom()//ルームに入ったら呼ばれる
    {
        PhotonNetwork.LocalPlayer.NickName = nameInput.GetComponent<TMP_InputField>().text;//ニックネームを変更

        playerScorePanel = playerScorePanelObject;//playerScorePanelの初期化

        nameInput.transform.parent.gameObject.SetActive(false);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject("RoomGameManager", Vector2.zero, Quaternion.identity);//一つだけROomGameManagerを作る

            //ルームのカスタムプロパティを作る
            ExitGames.Client.Photon.Hashtable roomHashTable = new ExitGames.Client.Photon.Hashtable();
            roomHashTable["r"] = 0;//準備ができているプレイヤーの人数[Ready]
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomHashTable);
        }

        //Punのhashtableの追加
        ExitGames.Client.Photon.Hashtable hashTable = new ExitGames.Client.Photon.Hashtable();
        hashTable["score"] = 0;
        hashTable["hit"] = 0;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashTable);

        PhotonNetwork.Instantiate("PlayerScoreDisplay", new Vector2(0, 0), Quaternion.identity);//スコアボードに追加

        readyButton.SetActive(true);
        
    }

    public void SetRoomGameManager(RoomGameManager roomGameManager)//ラグがあるのでインスタンス化されたときに指定する
    {
        Debug.Log("SetROomManager");
        this.roomGameManager = roomGameManager;
    }


    public void OnSelectedPlayer()
    {
        GameObject player = PhotonNetwork.Instantiate(avatarName, spawnPosition.position, Quaternion.identity);//プレイヤーのアバターをインスタンス化
        playerAvatars.Add(player);
        
        playerList.SetActive(false);
    }

    private bool hasRoundEnd;//OnEndRoundの操作が被らないようにするためのbool

    public void OnEndRound()
    {
        if (!hasRoundEnd)
        {
            foreach (GameObject player in playerAvatars)
            {
                if (!RoomGameManager.goaledPlayer.Contains(player.GetPhotonView().Owner))
                {
                    Destroy(player);
                    RoomGameManager.playersWipedOut.Add(player.GetPhotonView().Owner);
                    gameState = GameState.gameOver;
                }
            }
            playerAvatars.Clear();
            if(gameState == GameState.game)
            {
                readyButton.SetActive(true);
                readyButton.GetComponent<PlayerReadyButton>().ResetButton();
            }
            hasRoundEnd = true;
        }
    }

    public void OnStartRound()
    {
        RoomGameManager.goaledPlayer.Clear();
        if (RoomGameManager.playersWipedOut.Contains(PhotonNetwork.LocalPlayer))
        {
            Debug.Log("Wiped");
            gameState = GameState.gameOver;
        }
        else
        {
            readyButton.SetActive(false);
            playerList.SetActive(true);
        }
        countDown.StartCountDown(timeLimit, this);//カウントダウンを始める
        hasRoundEnd = false;
    }
}