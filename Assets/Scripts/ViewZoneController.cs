using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

/// <summary>
/// Input.GetMouseだとカメラ動かすUIで反応するので専用の透明なUIを作ってこれを張る
/// </summary>
public class ViewZoneController : MonoBehaviourPunCallbacks,IPointerDownHandler,IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if(GameManager.gameState == GameManager.GameState.game)
        {
            foreach (GameObject player in GameManager.playerAvatars)
            {
                if (player.GetPhotonView().IsMine)
                {
                    player.GetComponent<PlayerController>().IsOnDragZone = true;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(GameManager.gameState == GameManager.GameState.game)
        {
            StartCoroutine(PointerUp());
        }
    }

    public IEnumerator PointerUp()
    {
        yield return null;
        foreach (GameObject player in GameManager.playerAvatars)
        {
            if (player.GetPhotonView().IsMine)
            {
                player.GetComponent<PlayerController>().IsOnDragZone = false;
            }
        }
    }
}