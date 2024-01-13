using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// ゴール判定
/// !!!得点関係の処理はPlayerControllerのONTriggerEnterで行う
/// </summary>
public class GoalBehaviour : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform startTransform;//ゴールした後のプレイヤーがワープする座標
    [SerializeField] private GameManager gameManager;

    [SerializeField] private AudioClip goalSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.parent.gameObject.tag == "Player")//プレイヤーだけに処理する
        {
            audioSource.PlayOneShot(goalSound);//音を鳴らす
            PhotonView owner = collision.transform.parent.gameObject.GetPhotonView();//ボールの所有者
            if (owner.IsMine)
            {
                PlayerController.scrollBarMode = true;
            }
            gameManager.roomGameManager.PlayerGoal(owner.Owner);
            collision.transform.parent.position = new Vector2(0,-10);
            
        }
    }
}
