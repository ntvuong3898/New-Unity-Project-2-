using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckK : MonoBehaviour
{
    public Knight knight;

    //Start is called before the first frame update
    void Start()
    {
        knight = gameObject.GetComponentInParent<Knight>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            knight.grounded = true;
            knight.cast = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            knight.grounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            knight.grounded = false;
        }
    }
}
