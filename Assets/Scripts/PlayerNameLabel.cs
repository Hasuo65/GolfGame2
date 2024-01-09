using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

/// <summary>
/// プレイヤーのボールの上にある名前を決めるためのクラス
/// 
/// </summary>
public class PlayerNameLabel : MonoBehaviourPunCallbacks
{
    TextMeshPro nameLabel;

    private void Start()
    {
        nameLabel = GetComponent<TextMeshPro>();
        nameLabel.text = photonView.Owner.NickName;
    }
}
