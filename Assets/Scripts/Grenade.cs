using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Item
{
    public float delay, radius, tossStrength;
    public GameObject explosion;
    private Rigidbody2D rb;
    private Animator anim;

    public override void UseItem()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartCoroutine("ThrowGrenade");
    }

    private IEnumerator ThrowGrenade()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.velocity = mousePos.normalized * tossStrength;
        yield return new WaitForSeconds(delay);
        Explode();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Explode();
    }

    private void Explode()
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, radius);

        for(int i = 0; i < col.Length; i++)
        {
            if (col[i].CompareTag("Enemy")) col[i].GetComponent<Health>().Deteriorate(3);
            else if (col[i].CompareTag("Block"))
            {
                col[i].GetComponent<Block>().Deteriorate(3);
            }
        }
        GameObject instance = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
