using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI�������ꂽ�Ƃ��ɉ������A�N�e�B�u/��A�N�e�B�u�ɂ���
/// </summary>
public class UIToggleButton : MonoBehaviour
{
    [SerializeField] private GameObject UI;

    public void OnClick()
    {
        if (UI.activeSelf)
        {
            UI.SetActive(false);
        }
        else
        {
            UI.SetActive(true);
        }
    }
}
