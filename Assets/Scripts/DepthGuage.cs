using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DepthGuage : MonoBehaviour
{
    public TextMeshProUGUI depthGuage;
    private static float depth = 0, unit = 0;
    //private static bool trigger = false;
    private static int threshold = 20;
    private static bool called = false;

    // Start is called before the first frame update
    void Start()
    {
        depthGuage.text = "Depth: 0m";
    }

    // Update is called once per frame
    void Update()
    {
        //if(trigger)
        //{
            depthGuage.text = "Depth: " + depth + "m";
            //trigger = false;
        //}
    }

    public static void Deeper(float descent)
    {
        depth += descent;
        if (depth >= 10.0f)
        {
            if (!called)
            {
                FollowerSpawner.waiting = false;
                called = true;
            }
            //unit -= 1.0f;
            //depth++;
            if (depth % 10 == 0 && FollowerSpawner.spawnRate <= FollowerSpawner.maxWeight) FollowerSpawner.spawnRate++;
            if (depth % threshold == 0 && FollowerSpawner.spawnDelay >= 1)
            {
                FollowerSpawner.spawnDelay--;
                FollowerSpawner.enemyCap *= 2;
                threshold *= 2;
            }
        }
        //trigger = true;
    }

    public static void ZeroOut()
    {
        depth = 0;
        unit = 0;
        //trigger = true;
    }
}
