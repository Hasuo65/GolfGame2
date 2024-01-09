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

    [SerializeField] public GameObject playerScorePanelObject;//プレイヤーのスコアを表示するパネルをアタッチする    StartでplayerScorePanelに割り当てる
    public static GameObject playerScorePanel;

    [SerializeField] private CountDown countDown;
    [SerializeField] private int timeLimit;

    [SerializeField] private GameObject playerList;

    public enum GameState {
        game,//実際のゲーム時
        prop//障害物を置く時
    }

/*    public void OnStart()//PlayerNameInputのTextMeshPro-InputFieldのOn End Editに設定
    {
        PhotonNetwork.LocalPlayer.NickName = nameInput.GetComponent<TMP_InputField>().text;//ニックネームを変更

        playerScorePanel = playerScorePanelObject;//playerScorePanelの初期化

        nameInput.transform.parent.gameObject.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }*/

/*    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);
    }*/

    public override void OnJoinedRoom()//ルームに入ったら呼ばれる
    {
        PhotonNetwork.LocalPlayer.NickName = nameInput.GetComponent<TMP_InputField>().text;//ニックネームを変更

        playerScorePanel = playerScorePanelObject;//playerScorePanelの初期化

        nameInput.transform.parent.gameObject.SetActive(false);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject("RoomGameManager", Vector2.zero, Quaternion.identity).GetComponent<RoomGameManager>();//一つだけROomGameManagerを作る
        }
        playerList.SetActive(true);
    }

    public void OnSelectedPlayer()
    {
        //Punのhashtableの追加
        ExitGames.Client.Photon.Hashtable hashTable = new ExitGames.Client.Photon.Hashtable();
        hashTable["score"] = 0;
        hashTable["hit"] = 0;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashTable);

        GameObject player = PhotonNetwork.Instantiate(avatarName, spawnPosition.position, Quaternion.identity);//プレイヤーのアバターをインスタンス化
        playerAvatars.Add(player);
        PhotonNetwork.Instantiate("PlayerScoreDisplay", new Vector2(0, 0), Quaternion.identity);//スコアボードに追加
        countDown.StartCountDown(timeLimit, this);
        playerList.SetActive(false);
    }

    private bool hasRoundEnd;//OnROundEndの操作が被らないようにするためのbool

    public void OnEndRound()
    {
        if (!hasRoundEnd)
        {

            hasRoundEnd = true;
        }
    }

    public void OnStartRound()
    {
        hasRoundEnd = false;
    }
}