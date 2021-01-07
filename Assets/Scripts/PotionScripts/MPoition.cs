using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPoition : MonoBehaviour
{
    int mp = 40;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.SendMessageUpwards("TakeMP", mp);
            Destroy(gameObject);
        }
    }
}
