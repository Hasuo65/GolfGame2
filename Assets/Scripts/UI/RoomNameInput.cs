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
        if (Input.GetKey(KeyCode.Return)||Input.GetKey(KeyCode.KeypadEnter))//その辺をクリックしても実行されるからEnterを押したときだけ実行されるようにする
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom(roomInput.text, new Photon.Realtime.RoomOptions(), Photon.Realtime.TypedLobby.Default);
    }
}
