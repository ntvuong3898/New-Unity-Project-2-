using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckDrg : MonoBehaviour
{
    public Dragon dragon;

    //Start is called before the first frame update
    void Start()
    {
        dragon = gameObject.GetComponentInParent<Dragon>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            dragon.grounded = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            dragon.grounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            dragon.grounded = false;
        }
    }
}
