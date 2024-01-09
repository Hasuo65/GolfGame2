using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// ゴール判定
/// !!!得点関係の処理はPlayerControllerのONTriggerEnterで行う
/// </summary>
public class GoalBehaviour : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform startTransform;//ゴールした後のプレイヤーがワープする座標
    [SerializeField] private RoomGameManager roomGameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")//プレイヤーだけに処理する
        {
            roomGameManager.clearedPlayer.Add(collision.gameObject.GetPhotonView());
            Destroy(collision);
        }
    }
}
