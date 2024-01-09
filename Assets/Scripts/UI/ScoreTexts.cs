using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class ScoreTexts : MonoBehaviourPunCallbacks
{
    private TMP_Text hitText;
    private TMP_Text scoreText;
    private TMP_Text scoreHit;

    ExitGames.Client.Photon.Hashtable hashTable;

    private void Start() {
        transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = photonView.Owner.NickName;//プレイヤーの名前欄を変える
        hitText = transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
        scoreText = transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>();
        scoreHit = transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>();
    }

    private bool onInstantiate = true;

    public new void OnEnable()
    {
        if (!onInstantiate)
        {
            UpdateUI();
        }
        onInstantiate = false;
    }

    private float updateInterval;

    private void Update()
    {
        updateInterval += Time.deltaTime;
        if(updateInterval >= 2)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        hashTable = photonView.Owner.CustomProperties;
        hitText.text = hashTable["hit"].ToString();
        scoreText.text = hashTable["score"].ToString();
    }
}