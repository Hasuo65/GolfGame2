using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewModeToggleButton : MonoBehaviour
{
    public CameraController scrollbarx;
    public CameraControllerY scrollbary;

    private Toggle toggle;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
    }

    public void OnValueChanged()
    {
        PlayerController.scrollBarMode = toggle.isOn;
        if (toggle.isOn)
        {
            scrollbarx.OnUpdate();
            scrollbary.OnUpdate();
        }
    }
}
