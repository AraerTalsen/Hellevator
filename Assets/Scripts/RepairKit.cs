using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairKit : Item
{
    public override void UseItem()
    {
        ElevatorHealth eh = FindObjectOfType<ElevatorHealth>();
        eh.RepairElevator(5);
    }
}
