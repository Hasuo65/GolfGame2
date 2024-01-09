using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// �J���������E�ɓ��������߂̃N���X
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private float stageWidth;//�J�����𓮂����[��X���W
    private Scrollbar sb;
    private void Start()
    {
        sb = GetComponent<Scrollbar>();
    }

    public void OnUpdate()//Scrollbar��OnValueChanged�ɐݒ肷��
    {
        float cameraPosition = sb.value * stageWidth * 2 + -stageWidth;
        Camera.main.transform.position = new Vector3(cameraPosition, 0, -10);
    }
}