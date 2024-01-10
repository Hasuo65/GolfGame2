using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// �S�[������
/// !!!���_�֌W�̏�����PlayerController��ONTriggerEnter�ōs��
/// </summary>
public class GoalBehaviour : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform startTransform;//�S�[��������̃v���C���[�����[�v������W
    [SerializeField] private GameManager gameManager;

    [SerializeField] private AudioClip goalSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")//�v���C���[�����ɏ�������
        {
            audioSource.PlayOneShot(goalSound);//����炷
            Player owner = collision.transform.parent.gameObject.GetPhotonView().Owner;//�{�[���̏��L��
            if (owner == PhotonNetwork.LocalPlayer)
            {
                gameManager.roomGameManager.PlayerGoal(owner);
            }
            GameManager.playerAvatars.Remove(collision.gameObject);
            Destroy(collision.transform.parent.gameObject);//�Q�[���I�u�W�F�N�g������
            
        }
    }
}
