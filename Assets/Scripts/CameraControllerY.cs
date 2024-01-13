using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControllerY : MonoBehaviour
{
    private Scrollbar sb;

    [System.NonSerialized] public float cameraPosition;

    private void Start()
    {
        sb = GetComponent<Scrollbar>();
    }

    public void OnUpdate()//ScrollbarÇÃOnValueChangedÇ…ê›íËÇ∑ÇÈ
    {
        cameraPosition = sb.value *10;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, cameraPosition, -10); ;
    }
}
