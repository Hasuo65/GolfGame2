using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

/// <summary>
/// �Q�[���̃V�X�e���ɂ��ẴN���X
/// </summary>
public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform spawnPosition;//�v���C���[�̃X�^�[�g���W
    public static List<GameObject> playerAvatars = new List<GameObject>();//�v���C���[�̃{�[���̃��X�g
    [SerializeField] private GameObject nameInput;//�v���C���[����InputField�̃I�u�W�F�N�g
    [System.NonSerialized] public string avatarName;//�v���C���[���I�������A�o�^�[�̖��O

    [SerializeField] public GameObject playerScorePanelObject;//�v���C���[�̃X�R�A��\������p�l�����A�^�b�`����    Start��playerScorePanel�Ɋ��蓖�Ă�
    public static GameObject playerScorePanel;

    [SerializeField] private CountDown countDown;
    [SerializeField] private int timeLimit;

    [SerializeField] private GameObject playerList;

    public enum GameState {
        game,//���ۂ̃Q�[����
        prop//��Q����u����
    }

/*    public void OnStart()//PlayerNameInput��TextMeshPro-InputField��On End Edit�ɐݒ�
    {
        PhotonNetwork.LocalPlayer.NickName = nameInput.GetComponent<TMP_InputField>().text;//�j�b�N�l�[����ύX

        playerScorePanel = playerScorePanelObject;//playerScorePanel�̏�����

        nameInput.transform.parent.gameObject.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }*/

/*    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);
    }*/

    public override void OnJoinedRoom()//���[���ɓ�������Ă΂��
    {
        PhotonNetwork.LocalPlayer.NickName = nameInput.GetComponent<TMP_InputField>().text;//�j�b�N�l�[����ύX

        playerScorePanel = playerScorePanelObject;//playerScorePanel�̏�����

        nameInput.transform.parent.gameObject.SetActive(false);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject("RoomGameManager", Vector2.zero, Quaternion.identity).GetComponent<RoomGameManager>();//�����ROomGameManager�����
        }
        playerList.SetActive(true);
    }

    public void OnSelectedPlayer()
    {
        //Pun��hashtable�̒ǉ�
        ExitGames.Client.Photon.Hashtable hashTable = new ExitGames.Client.Photon.Hashtable();
        hashTable["score"] = 0;
        hashTable["hit"] = 0;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashTable);

        GameObject player = PhotonNetwork.Instantiate(avatarName, spawnPosition.position, Quaternion.identity);//�v���C���[�̃A�o�^�[���C���X�^���X��
        playerAvatars.Add(player);
        PhotonNetwork.Instantiate("PlayerScoreDisplay", new Vector2(0, 0), Quaternion.identity);//�X�R�A�{�[�h�ɒǉ�
        countDown.StartCountDown(timeLimit, this);
        playerList.SetActive(false);
    }

    private bool hasRoundEnd;//OnROundEnd�̑��삪���Ȃ��悤�ɂ��邽�߂�bool

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