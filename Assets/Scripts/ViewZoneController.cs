using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

/// <summary>
/// Input.GetMouse���ƃJ����������UI�Ŕ�������̂Ő�p�̓�����UI������Ă���𒣂�
/// </summary>
public class ViewZoneController : MonoBehaviourPunCallbacks,IPointerDownHandler,IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        foreach(GameObject player in GameManager.playerAvatars)
        {
            if (player.GetPhotonView().IsMine)
            {
                player.GetComponent<PlayerController>().IsOnDragZone = true;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StartCoroutine(PointerUp());
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