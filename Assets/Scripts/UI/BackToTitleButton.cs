using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class BackToTitleButton : MonoBehaviour
{
    public void OnClick()
    {
        RoomGameManager.goaledPlayer.Clear();
        RoomGameManager.playersWipedOut.Clear();
        GameManager.gameState = GameManager.GameState.game;
        GameManager.playerAvatars.Clear();
        GameManager.playerScorePanel = null;
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}