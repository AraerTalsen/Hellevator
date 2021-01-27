/*Daniel Greenberg
 * Last Updated: 1/14/20
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

//The ElevatorHealth class is a child class of Health specifically for the elevator
public class ElevatorHealth : Health
{
    public Vector2[] contactPoints;
    public TextMeshProUGUI tmp;
    public Image healthGuage, healthJuice;
    public GameManager game;
    public float waitTime;
    private bool waiting, visible, hit;
    private float resistance, lightLevel, energyUse;
    private List<GameObject> projectiles;
    private Color gColor, jColor;    
    private Vector2 healthBarPos;

    // Start is called before the first frame update
    public override void Start()
    {
        projectiles = new List<GameObject>();
        waiting = false;
        waitTime = 1.0f;
        base.Start();
        tmp.text = "Health: " + currentHealth;
        healthJuice.fillAmount = currentHealth / (float)maxHealth;
        resistance = 1.0f;
        lightLevel = 1.0f;
        energyUse = 1.0f;
        gColor = healthGuage.color;
        jColor = healthJuice.color;
        healthBarPos = contactPoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        Suicide();
    }

    //Decrements elevator health
    public override void Deteriorate(int subtract)
    {
        hit = true;
        currentHealth -= subtract;
        if (currentHealth <= 0) game.GameOver();

        if (!visible) StartCoroutine("FadeIn");
        tmp.text = "Health: " + currentHealth;
        healthJuice.fillAmount = currentHealth / (float)maxHealth;
        //Contact(contact);
    }

    //Sets elevator back to maxHealth
    public void RepairElevator(int amount)
    {
        currentHealth = amount + currentHealth > maxHealth ? maxHealth : currentHealth + amount;
        tmp.text = "Health: " + currentHealth;
        healthJuice.fillAmount = currentHealth / (float)maxHealth;
    }

    //Kills elevator for debugging
    private void Suicide()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            Deteriorate(maxHealth);
        }
    }

    private IEnumerator FadeIn()
    {
        visible = true;
        gColor = healthGuage.color;
        if (gColor.a == 0) healthGuage.rectTransform.position = healthBarPos;
        for(int i = (int)(gColor.a * 100 / 4); i < 25; i++)
        {
            healthGuage.color = new Color(gColor.r, gColor.g, gColor.b, i * 4 / 100f);
            healthJuice.color = new Color(jColor.r, jColor.g, jColor.b, i * 4 / 100f);
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine("FadeOut");
    }

    public void Contact(Vector2 pos)
    {
        float closest = 100;

        foreach (Vector2 p in contactPoints)
        {
            if (Vector2.Distance(pos, p) < closest)
            {
                healthBarPos = p;
                closest = Vector2.Distance(pos, p);
            }
        }
    }

    private IEnumerator FadeOut()
    {
        hit = false;
        for (int i = 0; i < 5; i++)
        {
            if (hit)
            {
                StartCoroutine("FadeIn");
                StopCoroutine("FadeOut");
            }
            yield return new WaitForSeconds(.6f);
        }
        for (int i = 25; i >= 0; i--)
        {
            if(hit)
            {
                StartCoroutine("FadeIn");
                StopCoroutine("FadeOut");
            }
            healthGuage.color = new Color(gColor.r, gColor.g, gColor.b, i * 4 / 100f);
            healthJuice.color = new Color(jColor.r, jColor.g, jColor.b, i * 4 / 100f);
            yield return new WaitForEndOfFrame();
        }
        visible = false;
    }


    public void SetResistance(float newVal)
    {
        resistance -= newVal;
    }

    public void SetLightLevel(float newVal)
    {
        lightLevel += newVal;
    }

    public void SetEnergyUse(float newVal)
    {
        energyUse -= newVal;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
