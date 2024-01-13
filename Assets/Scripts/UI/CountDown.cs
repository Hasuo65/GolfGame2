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

    private Coroutine coroutine;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    private bool isFirstRound = true;

    public void StartCountDown(int timeLimit,GameManager gameManager)
    {
        if (!isFirstRound)
        {
            StopCoroutine(coroutine);
        }
        this.gameManager = gameManager;//ゲームマネージャーの初期化
        coroutine = StartCoroutine(Down(timeLimit));
        isFirstRound = false;
        Debug.Log("StartCountDown");
    }

    private IEnumerator Down(int timeLimit)
    {
        count = timeLimit;
        while (true)
        {
            count--;
            text.text = count.ToString("00");
            if(count <= 0)
            {
                gameManager.OnEndRound();
                yield break;
            }
            yield return new WaitForSeconds(1);
        }
    }
}
