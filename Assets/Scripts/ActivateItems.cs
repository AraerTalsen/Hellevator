using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ActivateItems : MonoBehaviour
{
    public SpriteRenderer identifier;
    public Animator anim;
    public Sprite[] sprites;
    public GameObject[] physicalItems;
    public static string selectedItem;
    public static List<string> items = new List<string>();
    private GameObject obj;
    private Possessions pos;
    private int index;
    private bool emptyTrigger = false, notEmptyTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        pos = FindObjectOfType<Possessions>();
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(items.Count != 0 && !emptyTrigger)
        {
            SwitchItem();
            UseItem();
        }
        else if(items.Count != 0 && emptyTrigger)
        {
            StartCoroutine("DipIn");
        }
        else if(items.Count == 0 && !emptyTrigger)
        {
            StartCoroutine("DipOut");
        }
    }

    public void SetFirstItem(string item)
    {
        selectedItem = item;
        IdentifyItem(selectedItem);
    }

    private void SwitchItem()
    {
        if (Input.GetKeyUp(KeyCode.A) && items.Count != 0)
        {
            NextItem();
            index = (index + 1) % items.Count;
            selectedItem = items[index];
            IdentifyItem(selectedItem);
        }
    }

    private void UseItem()
    {
        if(Input.GetKeyUp(KeyCode.Mouse1))
        {            
            switch (selectedItem)
            {
                case "Grenade":
                    {
                        Vector2 spawn = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        spawn = spawn.normalized * 4.5f;
                        GameObject obj = Instantiate(Find(selectedItem), spawn, Quaternion.identity);
                        Grenade g = obj.GetComponent<Grenade>();
                        obj.transform.localScale *= DynamicAspectRatio.modifier;  
                        g.UseItem();                        
                        break;
                    }
                case "RepairKit":
                    {
                        RepairKit r = new RepairKit();
                        r.UseItem();
                        break;
                    }
            }
            NextItem();
        }
    }

    private void IdentifyItem(string item)
    {
        switch(item)
        {
            case "Grenade":
                {
                    identifier.sprite = sprites[0];
                    break;
                }
            case "RepairKit":
                {
                    identifier.sprite = sprites[1];
                    break;
                }
            default:
                {
                    identifier.sprite = null;
                    break;
                }
        }
        
    }

    private GameObject Find(string name)
    {
        for(int i = 0; i < physicalItems.Length; i++)
        {
            if (physicalItems[i].name.CompareTo(name) == 0) return physicalItems[i];
        }
        return null;
    }

    private void NextItem()
    {
        //If there is more than one item left, remove current item. Otherwise, clear the list
        if (items.Count > 1)
        {
            items.RemoveAt(index);
            pos.RemoveFromInventory(index);
        }
        else
        {
            items.Clear();
            index = 0;
            pos.RemoveFromInventory(-1);
            StartCoroutine("DipOut");
        }

        //Assign selected item
        if (items.Count != 0)
        {
            //If the index reaches the end of the list, loop back to the beginning
            if (index < items.Count)
            {
                selectedItem = items[index];
                IdentifyItem(selectedItem);
            }
            else
            {
                selectedItem = items[0];
                IdentifyItem(selectedItem);
                index = 0;
            }
        }
        //If list is empty, selected item holds nothing
        else
        {
            selectedItem = null;
            IdentifyItem(selectedItem);
            index = 0;
        }
    }

    private IEnumerator DipOut()
    {
        yield return new WaitForSeconds(.5f);
        anim.SetTrigger("empty");
        yield return new WaitForSeconds(.58f);
        anim.gameObject.SetActive(false);
        emptyTrigger = true;
    }

    private IEnumerator DipIn()
    {
        yield return new WaitForSeconds(.5f);
        emptyTrigger = false;
        notEmptyTrigger = true;
        anim.gameObject.SetActive(false);
        anim.SetTrigger("unEmpty");
        yield return new WaitForSeconds(.58f);                    
    }
}
