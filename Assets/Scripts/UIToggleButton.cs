using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UIが押されたときに何かをアクティブ/非アクティブにする
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
