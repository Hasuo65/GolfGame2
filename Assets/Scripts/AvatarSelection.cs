using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSelection : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject avatar;//選ぶアバターの名前
    public void OnClick()
    {
        gameManager.avatarName = avatar.name;
        gameManager.OnSelectedPlayer();
    }
}
