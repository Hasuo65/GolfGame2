using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

/// <summary>
/// �v���C���[�̃{�[���̏�ɂ��閼�O�����߂邽�߂̃N���X
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
