using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

/// <summary>
/// Input.GetMouse‚¾‚ÆƒJƒƒ‰“®‚©‚·UI‚Å”½‰‚·‚é‚Ì‚Åê—p‚Ì“§–¾‚ÈUI‚ğì‚Á‚Ä‚±‚ê‚ğ’£‚é
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