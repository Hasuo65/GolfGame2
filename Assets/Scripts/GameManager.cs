using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

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

    [SerializeField] private Scrollbar cameraSliderx;
    [SerializeField] private Scrollbar cameraSlidery;

    [SerializeField] private CountDown countDown;
    [SerializeField] private int timeLimit;

    [SerializeField] private GameObject playerList;

    [System.NonSerialized]public RoomGameManager roomGameManager;//RoomGameNanagerのインスタンス

    [SerializeField] private GameObject resultScreen;
    private GameObject[] lastPlayers;

    public enum GameState {
        game,//実際のゲーム時
        interval,//障害物を置く時
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
        hashTable["wiped"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashTable);

        PhotonNetwork.Instantiate("PlayerScoreDisplay", new Vector2(0, 0), Quaternion.identity);//スコアボードに追加

        readyButton.SetActive(true);
        
    }

    public void SetRoomGameManager(RoomGameManager roomGameManager)//ラグがあるのでインスタンス化されたときに指定する
    {
        Debug.Log("SetRoomManager");
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
            lastPlayers = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in lastPlayers)
            {
                if (!RoomGameManager.goaledPlayer.Contains(player.GetPhotonView().Owner))
                {
                    RoomGameManager.playersWipedOut.Add(player.GetPhotonView().Owner);
                    Destroy(player);
                }
            }
            if (RoomGameManager.playersWipedOut.Contains(PhotonNetwork.LocalPlayer))
            {
                gameState = GameState.gameOver;
            }
            playerAvatars.Clear();
            if(RoomGameManager.playersWipedOut.Count >= PhotonNetwork.PlayerList.Length)
            {
                GameOverScreen();
                return;
            }
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
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length != 0)
        {
            foreach (GameObject obj in players)
            {
                Destroy(obj);
            }
        }
        Debug.Log("OnStartRound");
        RoomGameManager.goaledPlayer.Clear();
        if(gameState == GameState.game){
            readyButton.SetActive(false);
            playerList.SetActive(true);
        }
        countDown.StartCountDown(timeLimit, this);//カウントダウンを始める
        timeLimit -= 5;
        hasRoundEnd = false;
    }

    public void GameOverScreen()
    {
        cameraSliderx.value = 0;
        cameraSlidery.value = 0;
        resultScreen.SetActive(true);
        List<Player> wonPlayers = new List<Player>();//同着があるのでリストにする
        int leastHit = 1000;
        foreach (GameObject player in lastPlayers)
        {
            Player owner = player.GetPhotonView().Owner;
            var hashTable = owner.CustomProperties;
            if((int)hashTable["hit"] < leastHit)//リストに追加する
            {
                wonPlayers.Clear();
                wonPlayers.Add(owner);
                leastHit = (int)hashTable["hit"];
            }
            else if((int)hashTable["hit"] == leastHit)
            {
                wonPlayers.Add(owner);
            }
        }
        foreach(Player wonPlayer in wonPlayers)
        {
            resultScreen.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>().text += wonPlayer.NickName + "\n";
        }
    }
}