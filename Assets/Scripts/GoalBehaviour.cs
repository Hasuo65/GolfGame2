using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// �S�[������
/// !!!���_�֌W�̏�����PlayerController��ONTriggerEnter�ōs��
/// </summary>
public class GoalBehaviour : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform startTransform;//�S�[��������̃v���C���[�����[�v������W
    [SerializeField] private RoomGameManager roomGameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")//�v���C���[�����ɏ�������
        {
            roomGameManager.clearedPlayer.Add(collision.gameObject.GetPhotonView());
            Destroy(collision);
        }
    }
}
