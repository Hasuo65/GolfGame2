using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNameInput : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomInput;

    private void Start()
    {
        roomInput = GetComponent<TMP_InputField>();
    }

    public void OnEnter()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))//その辺をクリックしても実行されるからEnterを押したときだけ実行されるようにする
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
}
