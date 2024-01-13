using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// カメラを左右に動かすためのクラス
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private float stageWidth;//カメラを動かす端のX座標
    private Scrollbar sb;

    [System.NonSerialized] public float cameraPosition;

    private void Start()
    {
        sb = GetComponent<Scrollbar>();
    }

    public void OnUpdate()//ScrollbarのOnValueChangedに設定する
    {
        cameraPosition = sb.value * stageWidth * 2 + -stageWidth;
        Camera.main.transform.position = new Vector3(cameraPosition, Camera.main.transform.position.y, -10);
    }
}
