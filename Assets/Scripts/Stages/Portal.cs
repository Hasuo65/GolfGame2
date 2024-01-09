using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform oppositeSide;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.parent.position = oppositeSide.transform.position;
    }
}
