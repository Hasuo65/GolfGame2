using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの名前関係のクラス
/// </summary>
public class PlayerScoreDisplayCOntroller : MonoBehaviour
{
    void Start()
    {
        //transform.SetParent(GameObject.Find("PlayerListUI").transform);/// ScorePanelが非アクティブだと人気されないのでGameManagerのplayerScorePanelを使う
        transform.SetParent(GameManager.playerScorePanel.transform);
    }
}
