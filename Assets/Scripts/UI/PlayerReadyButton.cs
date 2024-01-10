using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

/// <summary>
/// �}�b�`�̏��߂Ƀv���C���[�������ł��Ă��邩���m�F
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
