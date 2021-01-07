using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    float moveSpeed = 10f;
    public Rigidbody2D rb;
    int damage = 40;
    public GameObject explosion;

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger != true)
        {
            collision.SendMessageUpwards("TakeDamage", damage);
        }

        GameObject ex = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(ex.gameObject, 0.6f);
        Destroy(gameObject);
    }
}
