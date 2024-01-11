using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


/// <summary>
/// ルーム全体の管理を行うクラス
/// </summary>
public class RoomGameManager : MonoBehaviourPunCallbacks
{
    public static List<Player> goaledPlayer = new List<Player>();//ゴールしたプレイヤーのリスト
    public static List<Player> playersWipedOut = new List<Player>();//残ったプレイヤーのリスト
    private GameManager gameManager;

    private void Start()
    {
        GameManager gameManager;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.SetRoomGameManager(this);
        this.gameManager = gameManager;
    }

    public void PlayerGoal(Player goalPlayer)
    {
        goaledPlayer.Add(goalPlayer);
        Debug.Log("PlayerLength:"+PhotonNetwork.PlayerList.Length);
        Debug.Log("GoalPlayerCount:"+goaledPlayer.Count);
        if (goaledPlayer.Count >= PhotonNetwork.PlayerList.Length-playersWipedOut.Count)
        {
            ExitGames.Client.Photon.Hashtable roomHashTable = PhotonNetwork.CurrentRoom.CustomProperties;
            roomHashTable["r"] = 0;//リセット
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomHashTable);
            gameManager.OnEndRound();
        }
    }


    //残っているプレイヤーが全員準備できたら始める
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        foreach (var prop in propertiesThatChanged)
        {
            Debug.Log("Key:" + prop.Key.ToString() + "Value:" + (int)prop.Value + "Player:" + PhotonNetwork.PlayerList.Length.ToString());
            if ((string)prop.Key == "r" && (int)prop.Value >= PhotonNetwork.PlayerList.Length-playersWipedOut.Count)
            {
                Debug.Log("!!!!StartRound!!!!! Key:" + prop.Key.ToString() + "Value:" + (int)prop.Value + "Player:" + PhotonNetwork.PlayerList.Length.ToString());
                gameManager.OnStartRound();
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameManager gameManager;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.SetRoomGameManager(this);
        this.gameManager = gameManager;
    }
}