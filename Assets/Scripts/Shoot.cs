/*Daniel Greenberg
 * Last Updated: 1/14/20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Shooter class is the behavior of the Shooter enemy template
public class Shoot : MonoBehaviour
{
    public float range, chargeSpeed,shotSpeed;
    public GameObject  projectile;
    private GameObject target;
    private bool isShooting;
    private DynamicAspectRatio dar;
    private float mod;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<ElevatorHealth>().gameObject;
        dar = FindObjectOfType<DynamicAspectRatio>();

        mod = dar.GetModifier();
        range *= mod;
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > Layer.heightBorders.y + 2) Destroy(gameObject);

        LookForTarget();
    }

    //Searches scene for target (target intended to be the target)
    private void LookForTarget()
    {
        if (Vector2.Distance(target.transform.position, transform.position) <= range && !isShooting)
        {
            StartCoroutine("ShootProjectile");
        }
    }

    //Spawns a projectile and sets its trajctory
    private IEnumerator ShootProjectile()
    {
        isShooting = true;
        yield return new WaitForSeconds(chargeSpeed);        
        Vector2 spawnPos = transform.position + (target.transform.position - transform.position).normalized * mod;
        GameObject instance = Instantiate(projectile, spawnPos, Quaternion.identity);
        instance.transform.localScale *= mod;
        projectile.GetComponent<Projectile>().SetOrigin(transform.position);
        isShooting = false;
        StopCoroutine("ShootProjectile");
    }
}
