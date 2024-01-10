using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

/// <summary>
/// マッチの初めにプレイヤーが準備できているかを確認
/// </summary>
public class PlayerReadyButton : MonoBehaviourPunCallbacks
{
    private Image image;

    [System.NonSerialized] public bool isPlayerReady = false;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void ResetButton()
    {
        isPlayerReady = false;
        image.color = Color.white;
    }

    public void OnClick()
    {
        var roomHashtable = PhotonNetwork.CurrentRoom.CustomProperties;
        if (isPlayerReady)
        {
            roomHashtable["r"] = (int)roomHashtable["r"] - 1;
            image.color = Color.white;
            isPlayerReady = false;
        }
        else
        {
            roomHashtable["r"] = (int)roomHashtable["r"] + 1;
            image.color = Color.green;
            isPlayerReady = true;
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomHashtable);
    }
}
