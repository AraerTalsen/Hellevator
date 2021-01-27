using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LaserDictionary : MonoBehaviour
{
    public GameObject myLine1, myLine2;
    private GameObject[] lines = new GameObject[5];
    private Node[] current = new Node [6], previous = new Node[6];
    private int capacity = 6, currentLine = 0;
    private bool ready = false;
    private float[] timers = new float[6];
    private int tail, rot, rotIncrement;
    private bool full = false, single = false;
    private LaserType selectedLaser;
    private LaserType[] laserTypes;// = { DefaultLaser, GeminiLaser, ContagionLaser};
    private Tunnel tunnel;
    private EnergyGuage eg;

    // Start is called before the first frame update

    delegate void LaserType(Vector2 anchorPos, Vector2 mousePos, float rateOfFire, bool active);

    void Start()
    {
        tunnel = FindObjectOfType<Tunnel>();
        eg = FindObjectOfType<EnergyGuage>();
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = Instantiate(myLine1);
            lines[i].transform.SetParent(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UseLaser(Vector2 anchorPos, Vector2 mousePos, float rateOfFire, bool active)
    {
        if (selectedLaser == null) selectedLaser = DefaultLaser;

        selectedLaser(anchorPos, mousePos, rateOfFire, active);
    }

    public void Directory(int index)
    {
        laserTypes = new LaserType[3] { DefaultLaser, GeminiLaser, ContagionLaser};

        selectedLaser = laserTypes[index];
    }    

    public void DefaultLaser(Vector2 anchorPos, Vector2 mousePos, float rateOfFire, bool active)
    {
        if (active)
        {
            RaycastHit2D hit = Physics2D.Raycast(anchorPos, mousePos);
            previous[0] = current[0];
            //current[0] = hit.collider == null ? null : hit.collider.gameObject;
            GameObject g = hit.collider == null ? null : hit.collider.gameObject;
            current[0] = new Node(g);

            if (current[0].GetFocus() != null)
            {
                DrawLine(anchorPos, hit.point, myLine1);
                GameObject prev = previous[0] != null ? previous[0].GetFocus() : null;
                GameObject curr = current[0] != null ? current[0].GetFocus() : null;
                if (prev == current[0].GetFocus())
                {
                    if (timers[0] > 0) Fire(0);
                }
                else
                {
                    timers[0] = rateOfFire;
                    Fire(0);
                }
            }
            else
            {
                DrawLine(Vector2.zero, Vector2.zero, myLine1);
            }
            GameManager.speed = .01f;
        }
        else
        {
            DrawLine(Vector2.zero, Vector2.zero, myLine1);
            GameManager.speed = .03f;
        }
    }

    //delegate void Gemini(Vector2 anchorPos, Vector2 mousePos, float rateOfFire);

    public void GeminiLaser(Vector2 anchorPos, Vector2 mousePos, float rateOfFire, bool active)
    {
        if (active)
        {
            RaycastHit2D hit1 = Physics2D.Raycast(anchorPos, mousePos);
            RaycastHit2D hit2 = Physics2D.Raycast(anchorPos, -mousePos);

            previous[0] = current[0];
            previous[1] = current[1];
            GameObject g0 = hit1.collider == null ? null : hit1.collider.gameObject;
            GameObject g1 = hit2.collider == null ? null : hit2.collider.gameObject;
            current[0] = new Node(g0);
            current[1] = new Node(g1);

            if (current[0].GetFocus() != null || current[1].GetFocus() != null)
            {
                DrawLine(hit1.point, hit2.point, myLine1);
                //DrawLine(anchorPos, hit2.transform.position, myLine2);
                for(int i = 0; i < 2; i++)
                {
                    GameObject prev = previous[i] != null ? previous[i].GetFocus() : null;
                    GameObject curr = current[i] != null ? current[i].GetFocus() : null;
                    if (prev == curr)
                    {
                        if (timers[i] > 0) Fire(i);
                    }
                    else
                    {
                        timers[i] = rateOfFire;
                        Fire(i);
                    }
                }
            }
            else
            {
                //StopCoroutine("Fire");
                DrawLine(Vector2.zero, Vector2.zero, myLine1);
                //DrawLine(Vector2.zero, Vector2.zero, myLine2);
            }
            GameManager.speed = .01f;
        }
        else
        {
            DrawLine(Vector2.zero, Vector2.zero, myLine1);
            GameManager.speed = .03f;
            //DrawLine(Vector2.zero, Vector2.zero, myLine2);
        }
    }

    public void ContagionLaser(Vector2 anchorPos, Vector2 mousePos, float rateOfFire, bool active)
    {
        if (active)
        {
            DestroyLines();
            rot = 0;
            rotIncrement = 10;
            tail = 0;           
            RaycastHit2D hit = Physics2D.Raycast(anchorPos, mousePos);
            GameObject hitObj = hit.collider.gameObject;
            DrawLine(anchorPos, hit.point, myLine1);
            previous[0] = current[0];
            current[0] = new Node(hitObj, true);
            if (hitObj.CompareTag("Enemy"))
            {
                for (int i = 0; i <= tail; i++)
                {
                    CheckForNeighbors(current[i]);
                }
            }
            else single = true;
            if (single) tail++;
            for (int i = 0; i < tail; i++)
            {
                GameObject prev = previous[i] != null ? previous[i].GetFocus() : null;
                GameObject curr = current[i] != null ? current[i].GetFocus() : null;
                if (prev == curr)
                {
                    Fire(i);
                }
                else
                {
                    if (i == 0 && !current[0].GetFocus().CompareTag("Enemy")) DestroyLines();

                    timers[i] = rateOfFire;
                    Fire(i);
                   
                }
            }
            single = false;
            GameManager.speed = .01f;
        }
        else
        {
            DrawLine(Vector2.zero, Vector2.zero, myLine1);
            DestroyLines();
            GameManager.speed = .03f;
        }
    }

    //If raycast hits a tile or enemy, deteriorate its health
    public void Fire(int index)
    {
        timers[index] -= Time.deltaTime;
        if (timers[index] <= 0)
        {
            if (current[index].GetFocus().CompareTag("Enemy"))
                current[index].GetFocus().GetComponent<Health>().Deteriorate(1);
            else if (current[index].GetFocus().CompareTag("Block"))
                current[index].GetFocus().GetComponent<Block>().Deteriorate(1);
            current[index] = null;
            eg.DepleteEnergy();
        }
    }

    //UI to represent laser
    private void DrawLine(Vector2 start, Vector2 end, GameObject myLine)
    {
        LineRenderer lr = myLine.GetComponent<LineRenderer>();        
        myLine.transform.position = start;
        lr.widthMultiplier = .1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.endColor = Color.red;
        lr.startColor = Color.red;
    }

    private void CheckForNeighbors(Node n)
    {
        if (!n.IsTetherless())
        {
            n.FindShortestTether();
            DrawLine(n.GetObjPoint(), n.GetHitPoint(), lines[currentLine]);            
            currentLine = (currentLine + 1) % (lines.Length - 1);
        }
        Vector2 dir, objPoint;
        GameObject o = n.GetFocus();
        int deltaTail = tail;
        CircleCollider2D cc = o != null ? o.GetComponent<CircleCollider2D>() : null;
        if(cc != null)
        {           
            float radius = cc.radius + .01f, range = 5.0f - radius, unitDegree;
            objPoint = o.transform.position;
            for (int i = 0; i < 360 / rotIncrement; i++)
            {                
                unitDegree = rot * Mathf.PI / 180;
                objPoint = new Vector2(radius * Mathf.Cos(unitDegree) + objPoint.x, radius * Mathf.Sin(unitDegree) + objPoint.y);
                dir = new Vector2(Mathf.Cos(unitDegree), Mathf.Sin(unitDegree));
                RaycastHit2D hit = Physics2D.Raycast(objPoint, dir);
                dir += objPoint;
                rot += rotIncrement;                               
                GameObject newO = hit.collider != null ? hit.collider.gameObject : null;
                
                if (newO != null && newO.CompareTag("Enemy") && range >= Vector2.Distance(hit.point, objPoint) && Find(newO) < 0)
                {
                    if (tail < current.Length)
                    {
                        tail++;
                        previous[tail] = current[tail];
                        current[tail] = new Node(newO);
                    }
                    else break;
                }
                else if(newO != null && newO.CompareTag("Enemy") && range >= Vector2.Distance(hit.point, objPoint))
                {
                    int index = Find(newO);
                    if (index >= 0) current[index].Add(objPoint, hit.point);
                }
                objPoint = o.transform.position;
            }
        }
        single = deltaTail == tail;
        rot = 0;
    }

    private int Find(GameObject o)
    {
        for(int i = 0; i <= tail; i++)
        {
            if (current[i].GetFocus() == o) return i;
        }
        return -1;
    }

    private void DestroyLines()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            if(lines[i] != null) DrawLine(Vector2.zero, Vector2.zero, lines[i]);
        }
    }

    private void DrawInColor(Vector2 start, Vector2 end, int i)
    {
        switch(i)
        {
            case 0:
                {
                    Debug.DrawRay(start, end, Color.yellow);
                    break;
                }
            case 1:
                {
                    Debug.DrawRay(start, end, Color.green);
                    break;
                }
            case 2:
                {
                    Debug.DrawRay(start, end, Color.blue);
                    break;
                }
            case 3:
                {
                    Debug.DrawRay(start, end, Color.white);
                    break;
                }
        }
    }
}
