/*Daniel Greenberg
 * Last Updated: 1/14/20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Follower class is the behavior for the Follower enemy template
public class Follower : MonoBehaviour
{
    public GameObject windGust;
    private GameObject target;
    public float range, chargeTime, dashDistance, moveSpeed, restTime;
    private bool isAttacking, isResting, triggerRest;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private DynamicAspectRatio dar;
    private float mod;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        target = FindObjectOfType<ElevatorHealth>().gameObject;
        dar = FindObjectOfType<DynamicAspectRatio>();
        rb = GetComponent<Rigidbody2D>();
        mod = dar.GetModifier();
        isAttacking = false;
        isResting = false;
        triggerRest = false;
        range *= mod;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > Layer.heightBorders.y + 2) Destroy(gameObject);

        if (triggerRest) StartCoroutine("Rest");

        if(!isResting) LookForTarget();
        else transform.position = new Vector2(transform.position.x, transform.position.y + GameManager.speed);

    }

    //Searches for the target to be followed (intended to be the elevator)
    private void LookForTarget()
    {        
        if (Vector2.Distance(target.transform.position, transform.position) <= range)
        {
            Pursue();
            if (Vector2.Distance(target.transform.position, transform.position) <= dashDistance && !isAttacking)
                StartCoroutine("Dash");
        }
    }

    //Moves the follower towards the target
    private void Pursue()
    {
        if (transform.parent != null) transform.parent = null;

        bool close = Vector2.Distance(target.transform.position, transform.position) <= dashDistance;

        if(close)
        {
            rb.velocity = Vector2.zero;
            rb.transform.position = (target.transform.position + transform.position).normalized * dashDistance;
        }
        else rb.velocity = (target.transform.position - transform.position) * moveSpeed;       
    }

    //Attack where the follower charges up and then dashes through the target, dealing damage
    private IEnumerator Dash()
    {
        Vector2 last = transform.position;
        isAttacking = true;
        yield return new WaitForSeconds(chargeTime);
        transform.position = -transform.position;        
        yield return new WaitForEndOfFrame();
        Vector2 spawnPos = last + ((Vector2)target.transform.position - last).normalized * mod;
        GameObject instance = Instantiate(windGust, spawnPos, Quaternion.identity);
        instance.transform.localScale *= mod;
        instance.GetComponent<Projectile>().SetOrigin(transform.position);
        isAttacking = false;
        triggerRest = true;
        StopCoroutine("Dash");
    }

    private IEnumerator Rest()
    {
        triggerRest = false;
        isResting = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(restTime);
        isResting = false;
        StopCoroutine("Rest");
    }
}
