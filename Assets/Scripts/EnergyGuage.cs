using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergyGuage : MonoBehaviour
{
    public int maxEnergy;
    public float longWait, shortWait;
    public TextMeshProUGUI energyGuage;
    public static bool empty, firing;
    private bool isRecharging;
    private int currentEnergy;    

    // Start is called before the first frame update
    void Start()
    {
        energyGuage.text = "Energy: " + currentEnergy + "/" + maxEnergy;
        currentEnergy = maxEnergy;
        empty = false;
        isRecharging = false;
        firing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnergy < maxEnergy && !isRecharging && !firing) StartCoroutine("Recharge");
        else if (firing) StopCoroutine("Recharge");
    }

    public void DepleteEnergy()
    {
        currentEnergy--;
        if (currentEnergy <= 0) empty = true;
        energyGuage.text = "Energy: " + currentEnergy + "/" + maxEnergy;
    }

    private IEnumerator Recharge()
    {
        if(empty)
        {
            currentEnergy = 0;
            energyGuage.text = "Energy: " + currentEnergy + "/" + maxEnergy;
            yield return new WaitForSeconds(longWait);
            empty = false;            
        }

        for(; currentEnergy <= maxEnergy; currentEnergy++)
        {
            energyGuage.text = "Energy: " + currentEnergy + "/" + maxEnergy;
            yield return new WaitForSeconds(shortWait);
        }
        StopCoroutine("Recharge");
    }
}
