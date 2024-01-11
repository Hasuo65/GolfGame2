using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    private TMP_Text text;

    private int count = 0;

    private GameManager gameManager;

    private GameObject timeUpScreen;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void StartCountDown(int timeLimit,GameManager gameManager)
    {
        this.gameManager = gameManager;//ゲームマネージャーの初期化
        count = timeLimit;
        StartCoroutine(Down());
    }

    private IEnumerator Down()
    {
        bool w = true;
        while (w)
        {
            yield return new WaitForSeconds(1);
            count--;
            text.text = count.ToString("00");
            if(count <= 0)
            {
                gameManager.OnEndRound();
                w = false;
            }
        }
    }
}
