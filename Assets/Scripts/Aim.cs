/*Daniel Greenberg
 * Last Updated: 1/14/20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//The Aim class is for determining where the laser goes and what it hits
public class Aim : MonoBehaviour
{
    public GameObject projectile, cannon;
    private Vector2 mousePos, anchorPos, direction;
    private Collider2D previous, current;
    public float speedMod;
    private bool pause;
    private LaserDictionary ld;

    private void Start()
    {
        ld = FindObjectOfType<LaserDictionary>();
        anchorPos = transform.position;        
        pause = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!pause) AimDrillLaser();       
    }

    //Inhibit laser fire (intended for if mining expedition is paused or ended)
    public void TogglePause(bool state)
    {
        pause = state;
    }

    //Checks mouse location and draws raycast in that direction
    private void AimDrillLaser()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int angle = 180 + (int)CalculateAngle(mousePos, Vector2.zero);
        cannon.transform.rotation = Quaternion.Euler(0, 0, angle);

        /*if (Vector2.Distance(mousePos, anchorPos) >= 5)
        {
            transform.localPosition = (anchorPos + mousePos).normalized * 5;
        }
        else
        {
            transform.position = mousePos;
        }*/
        transform.position = mousePos;

        if (Input.GetKey(KeyCode.Mouse0)/* && !EnergyGuage.empty*/)
        {
            ld.UseLaser(anchorPos, mousePos, speedMod, true);
            //EnergyGuage.firing = true;
        }
        else
        {
            ld.UseLaser(anchorPos, mousePos, speedMod, false);
            //EnergyGuage.firing = false;
        }
    }

    public float CalculateAngle(Vector2 current, Vector2 anchor)
    {
        float a, b, angle;
        a = current.x - anchor.x;
        b = current.y - anchor.y;
        angle = Mathf.Atan(b / a);
        angle *= Mathf.Rad2Deg;

        Vector2 thisPos = transform.position;
        if (current.x <= anchor.x && current.y > anchor.y)
        {
            angle += 180;
        }
        else if (current.x < anchor.x && current.y <= anchor.y)
        {
            angle += 180;
        }
        else if (current.x >= anchor.x && current.y < anchor.y)
        {
            angle += 360;
        }

        angle = NormalizeAngle360(angle);
        return angle;
    }

    public float NormalizeAngle360(float angle)
    {
        if (angle > 360)
            return angle % 360;
        else if (angle < 0)
            return angle % 360 + 360;
        else
            return angle;
    }
}
