using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// �Q�[���̃V�X�e���ɂ��ẴN���X
/// </summary>
public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform spawnPosition;//�v���C���[�̃X�^�[�g���W
    public static List<GameObject> playerAvatars = new List<GameObject>();//�v���C���[�̃{�[���̃��X�g
    [SerializeField] private GameObject nameInput;//�v���C���[����InputField�̃I�u�W�F�N�g
    [System.NonSerialized] public string avatarName;//�v���C���[���I�������A�o�^�[�̖��O
    [SerializeField] private GameObject readyButton;//Ready�̃{�^��

    [SerializeField] public GameObject playerScorePanelObject;//�v���C���[�̃X�R�A��\������p�l�����A�^�b�`����    Start��playerScorePanel�Ɋ��蓖�Ă�
    public static GameObject playerScorePanel;

    [SerializeField] private Scrollbar cameraSliderx;
    [SerializeField] private Scrollbar cameraSlidery;

    [SerializeField] private CountDown countDown;
    [SerializeField] private int timeLimit;

    [SerializeField] private GameObject playerList;

    [System.NonSerialized]public RoomGameManager roomGameManager;//RoomGameNanager�̃C���X�^���X

    [SerializeField] private GameObject resultScreen;
    private GameObject[] lastPlayers;

    public enum GameState {
        game,//���ۂ̃Q�[����
        interval,//��Q����u����
        gameOver//�Q�[���I�[�o�[
    }
    public static GameState gameState = GameState.game;

    public override void OnJoinedRoom()//���[���ɓ�������Ă΂��
    {
        PhotonNetwork.LocalPlayer.NickName = nameInput.GetComponent<TMP_InputField>().text;//�j�b�N�l�[����ύX

        playerScorePanel = playerScorePanelObject;//playerScorePanel�̏�����

        nameInput.transform.parent.gameObject.SetActive(false);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject("RoomGameManager", Vector2.zero, Quaternion.identity);//�����ROomGameManager�����

            //���[���̃J�X�^���v���p�e�B�����
            ExitGames.Client.Photon.Hashtable roomHashTable = new ExitGames.Client.Photon.Hashtable();
            roomHashTable["r"] = 0;//�������ł��Ă���v���C���[�̐l��[Ready]
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomHashTable);
        }

        //Pun��hashtable�̒ǉ�
        ExitGames.Client.Photon.Hashtable hashTable = new ExitGames.Client.Photon.Hashtable();
        hashTable["score"] = 0;
        hashTable["hit"] = 0;
        hashTable["wiped"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashTable);

        PhotonNetwork.Instantiate("PlayerScoreDisplay", new Vector2(0, 0), Quaternion.identity);//�X�R�A�{�[�h�ɒǉ�

        readyButton.SetActive(true);
        
    }

    public void SetRoomGameManager(RoomGameManager roomGameManager)//���O������̂ŃC���X�^���X�����ꂽ�Ƃ��Ɏw�肷��
    {
        Debug.Log("SetRoomManager");
        this.roomGameManager = roomGameManager;
    }


    public void OnSelectedPlayer()
    {
        GameObject player = PhotonNetwork.Instantiate(avatarName, spawnPosition.position, Quaternion.identity);//�v���C���[�̃A�o�^�[���C���X�^���X��
        playerAvatars.Add(player);
        
        playerList.SetActive(false);
    }

    private bool hasRoundEnd;//OnEndRound�̑��삪���Ȃ��悤�ɂ��邽�߂�bool

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
        countDown.StartCountDown(timeLimit, this);//�J�E���g�_�E�����n�߂�
        timeLimit -= 5;
        hasRoundEnd = false;
    }

    public void GameOverScreen()
    {
        cameraSliderx.value = 0;
        cameraSlidery.value = 0;
        resultScreen.SetActive(true);
        List<Player> wonPlayers = new List<Player>();//����������̂Ń��X�g�ɂ���
        int leastHit = 1000;
        foreach (GameObject player in lastPlayers)
        {
            Player owner = player.GetPhotonView().Owner;
            var hashTable = owner.CustomProperties;
            if((int)hashTable["hit"] < leastHit)//���X�g�ɒǉ�����
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