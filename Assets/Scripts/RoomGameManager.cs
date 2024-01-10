using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


/// <summary>
/// ���[���S�̂̊Ǘ����s���N���X
/// </summary>
public class RoomGameManager : MonoBehaviourPunCallbacks
{
    public static List<Player> goaledPlayer = new List<Player>();//�S�[�������v���C���[�̃��X�g
    public static List<Player> playersWipedOut = new List<Player>();//�c�����v���C���[�̃��X�g
    private List<GameManager> gameManagers = new List<GameManager>();

    private void Start()
    {
        GameManager gameManager;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManagers.Add(gameManager);
        gameManager.SetRoomGameManager(this);
    }

    public void PlayerGoal(Player goalPlayer)
    {
        goaledPlayer.Add(goalPlayer);
        Debug.Log(PhotonNetwork.PlayerList.Length);
        Debug.Log(goaledPlayer.Count);
        if (goaledPlayer.Count >= PhotonNetwork.PlayerList.Length)
        {
            foreach(GameManager gm in gameManagers)
            {
                gm.OnEndRound();
            }
            ExitGames.Client.Photon.Hashtable roomHashTable = PhotonNetwork.CurrentRoom.CustomProperties;
            roomHashTable["r"] = 0;//���Z�b�g
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomHashTable);
        }
    }


    //�c���Ă���v���C���[���S�������ł�����n�߂�
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        foreach (var prop in propertiesThatChanged)
        {
            Debug.Log("Key:" + prop.Key.ToString() + "Value:" + (int)prop.Value + "Player:" + PhotonNetwork.PlayerList.Length.ToString());
            if ((string)prop.Key == "r" && (int)prop.Value == PhotonNetwork.PlayerList.Length)
            {
                foreach(GameManager gm in gameManagers)
                {
                    Debug.Log("GMS;" + gameManagers.Count);
                    Debug.Log("StartRound");
                    gm.OnStartRound();
                }
            }
        }
    }
}