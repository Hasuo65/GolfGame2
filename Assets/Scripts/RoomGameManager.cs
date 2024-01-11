using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


/// <summary>
/// ���[���S�̂̊Ǘ����s���N���X
/// </summary>
public class RoomGameManager : MonoBehaviourPunCallbacks
{
    public static List<Player> goaledPlayer = new List<Player>();//�S�[�������v���C���[�̃��X�g
    public static List<Player> playersWipedOut = new List<Player>();//�c�����v���C���[�̃��X�g
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
            roomHashTable["r"] = 0;//���Z�b�g
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomHashTable);
            gameManager.OnEndRound();
        }
    }


    //�c���Ă���v���C���[���S�������ł�����n�߂�
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