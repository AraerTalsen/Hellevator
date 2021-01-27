/*Daniel Greenberg
 * Last Updated: 1/14/20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Projectile class is the behaviour of the Projectile template
public class Projectile : MonoBehaviour
{
    public float speed;
    private Vector2 lastPoint;
    private Rigidbody2D rb;
    private CircleCollider2D cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = (lastPoint - (Vector2)transform.position).normalized * speed;
    }

    //Sets projectile's trigger to equal the input state
    public void ToggleCollider(bool state)
    {
        cc.isTrigger = state;
    }

    //Sets projectile's origin so it knows the direction to move away from
    public void SetOrigin(Vector2 origin)
    {
        lastPoint = origin;
    }

    //If projectile collides with the elevator, cause damage to elevator and destroy projectile
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.CompareTo("ElevatorOutline") == 0)
        {
            collision.gameObject.GetComponent<ElevatorHealth>().Deteriorate(1);
            Destroy(gameObject);
        }
    }
}
