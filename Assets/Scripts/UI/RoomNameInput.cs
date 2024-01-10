using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class RoomNameInput : MonoBehaviourPunCallbacks
{
    private TMP_InputField roomInput;

    private void Start()
    {
        roomInput = GetComponent<TMP_InputField>();
    }

    public void OnEnter()
    {
        if (Input.GetKey(KeyCode.Return)||Input.GetKey(KeyCode.KeypadEnter))//���̕ӂ��N���b�N���Ă����s����邩��Enter���������Ƃ��������s�����悤�ɂ���
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom(roomInput.text, new Photon.Realtime.RoomOptions(), Photon.Realtime.TypedLobby.Default);
    }
}
