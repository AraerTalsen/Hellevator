using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private GameObject focus = null;
    private List<Vector2> objPoints = new List<Vector2>(), hitPoints = new List<Vector2>();    
    private bool locked = false, tetherless = false;
    private Vector2 objPoint, hitPoint;

    public Node(GameObject f) { focus = f; }
    public Node(GameObject f, bool t) { focus = f; tetherless = t; }

    public void Add(Vector2 obj, Vector2 hit)
    {
        if(!locked)
        {
            objPoints.Add(obj);
            hitPoints.Add(hit);
        }
    }

    public void FindShortestTether()
    {
        locked = true;
        float shortest = 100;
        int shortestIndex = -1;
        
        for(int i = 0; i < objPoints.Count; i ++)
        {
            if(Vector2.Distance(objPoints[i], hitPoints[i]) < shortest)
            {
                shortest = Vector2.Distance(objPoints[i], hitPoints[i]);
                shortestIndex = i;
            }
        }
        objPoint = objPoints[shortestIndex];
        hitPoint = hitPoints[shortestIndex];
    }

    public GameObject GetFocus() { return focus; }

    public Vector2 GetObjPoint() { return objPoint; }

    public Vector2 GetHitPoint() { return hitPoint; }

    public bool IsTetherless()
    {
        return tetherless || objPoints.Count == 0;
    }
}
