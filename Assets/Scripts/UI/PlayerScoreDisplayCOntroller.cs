using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̖��O�֌W�̃N���X
/// </summary>
public class PlayerScoreDisplayCOntroller : MonoBehaviour
{
    void Start()
    {
        //transform.SetParent(GameObject.Find("PlayerListUI").transform);/// ScorePanel����A�N�e�B�u���Ɛl�C����Ȃ��̂�GameManager��playerScorePanel���g��
        transform.SetParent(GameManager.playerScorePanel.transform);
    }
}
