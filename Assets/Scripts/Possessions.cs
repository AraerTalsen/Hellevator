using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Possessions : MonoBehaviour
{
    private Dictionary<string, float> stats;
    private float[] capValues = { 3.0f, 2.0f, 1.0f };
    private Dictionary<string, bool> powerups;
    private Dictionary<string, bool> tools;
    private List<string> inventory = new List<string>();
    private int head = 0;
    private int capacity = 9;
    private ElevatorHealth elevator;

    // Start is called before the first frame update
    void Start()
    {
        elevator = FindObjectOfType<ElevatorHealth>();
        stats = new Dictionary<string, float>
        {
            { "lightLevel", 1.0f},
            { "resistance", 0},
            { "energyUse", 0}
        };

        powerups = new Dictionary<string, bool>
        {
            { "gemini", false},
            { "default", true},
            { "contagion", false}
        };

        tools = new Dictionary<string, bool>
        {
            { "leftTop", false},
            { "rightTop", false},
            { "leftBot", false}
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToInventory(string item)
    {
        if(!CheckInventoryFull())
        {
            if (inventory.Count == 0) FindObjectOfType<ActivateItems>().SetFirstItem(item);
            inventory.Add(item);
            ActivateItems.items.Add(item);
        }        
    }

    public void RemoveFromInventory(int index)
    {
        if (index == -1) inventory.Clear();
        else inventory.RemoveAt(index);
    }

    public bool CheckInventoryFull()
    {
        return inventory.Count >= capacity;
    }

    public void IncreaseStat(string whichStat, float amount)
    {
        stats[whichStat] += amount;
        switch(whichStat)
        {
            case "lightLevel":
            {
                elevator.SetLightLevel(stats[whichStat]);
                break;
            }
            case "resistance":
            {
                elevator.SetResistance(stats[whichStat]);
                break;
            }
            case "energyUse":
            {
                elevator.SetEnergyUse(stats[whichStat]);
                break;
            }
        }
    }

    public bool[] GetStatsIsCap()
    {
        float[] statsArray = stats.Values.ToArray();
        bool[] isCap = new bool[stats.Count];

        for (int i = 0; i < stats.Count; i++) isCap[i] = statsArray[i] >= capValues[i];
        return isCap;
    }

    public void PurchasePowerup(string whichPower)
    {
        powerups[whichPower] = true;
    }

    public bool[] GetPowerups()
    {
        return powerups.Values.ToArray();
    }

    public void PurchaseTool(string whichTool)
    {
        tools[whichTool] = true;
    }
}
