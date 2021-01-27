/* Daniel Greenberg
 * Last Updated: 1/14/20
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//The block class is the structure behind all environment tiles
public class Block : MonoBehaviour
{
    public string id;
    public int maxDurability, currentDurrability;
    private ScoringPoints score;
    private List<string> IDs = new List<string>{ "Aluminum", "Copper", "Bronze" };
    private int[] pointEquiv = { 100, 200, 300 };
    public GameObject stage1, stage2;
    private bool stage1Active = false, stage2Active = false;
    private DynamicAspectRatio dar;
    private float mod;

    private void Start()
    {
        dar = FindObjectOfType<DynamicAspectRatio>();
        mod = dar.GetModifier();
        currentDurrability = maxDurability;
    }

    //returns tile type
    public string GetID()
    {
        return id;
    }

    public void Deteriorate(int subtract)
    {
        currentDurrability -= subtract;
        DisplayDeterioration();
    }

    //Displays tile remaining durrability
    private void DisplayDeterioration()
    {
        if(currentDurrability > 2 * (maxDurability / 3) && !stage1Active)
        {
            stage1Active = true;
            GameObject instance = Instantiate(stage1);
            instance.transform.SetParent(transform);
            instance.transform.localScale *= mod * BlockManager.blockSize;
            instance.transform.localPosition = Vector2.zero;
        }
        else if(currentDurrability >= maxDurability / 3 && ! stage2Active)
        {
            stage2Active = false;
            GameObject instance = Instantiate(stage2);
            instance.transform.SetParent(transform);
            instance.transform.localScale *= mod;
            instance.transform.localPosition = Vector2.zero;
        }
        else if (currentDurrability <= 0)
        {
            if(IDs.FirstOrDefault(s => s.CompareTo(id) == 0) != null)
            {
                score = FindObjectOfType<ScoringPoints>();
                score.IncrementPoints(pointEquiv[IDs.IndexOf(id)]);
            }            
            Destroy(gameObject);
        }
    }
}
