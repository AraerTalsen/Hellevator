/*Daniel Greenberg
 * Last Updated: 1/14/20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Health class is for managing enemy health
public class Health : MonoBehaviour
{
    public int maxHealth, currentHealth;
    private ScoringPoints score;
    private SpriteRenderer sr;
    private List<string> IDs = new List<string> { "Follower", "Shooter", "Projectile"};
    private int[] pointEquiv = {200, 400, 50};
    private bool blinking;

    public virtual void Start()
    {
        blinking = false;
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    public virtual void Deteriorate(int subtract)
    {
        currentHealth -= subtract;
        DisplayDamage();
    }

    /*public virtual void Deteriorate(int subtract, Vector2 contact)
    {
        currentHealth -= subtract;
        DisplayDamage();
    }*/

    //Indicates damage
    private void DisplayDamage()
    {
        if(!blinking)
            StartCoroutine("DamageBlink");
        if (currentHealth <= 0)
        {
            string name = gameObject.name;
            name = name.IndexOf("(") > -1 ? name.Substring(0, name.IndexOf("(")) : name;
            score = FindObjectOfType<ScoringPoints>();
            score.IncrementPoints(pointEquiv[IDs.IndexOf(name)]);
            if(name.CompareTo("Projectile") != 0) FollowerSpawner.enemyCount--;
            Destroy(gameObject);
        }
    }

    //Animates health deduction
    public IEnumerator DamageBlink()
    {
        blinking = true;
        sr.color = Color.red;
        yield return new WaitForSeconds(.25f);
        sr.color = Color.white;
        blinking = false;
        StopCoroutine("DamageBlink");
    }
}
