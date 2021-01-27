using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class LaserActivation : MonoBehaviour
{
    public SpriteRenderer cannon;
    public Sprite[] laserTypes;
    public Vector2[] centerLaser;
    public Button[] activeLasers;
    private Color active, inactive;
    private LaserDictionary ld;
    // Start is called before the first frame update
    void Start()
    {
        ld = FindObjectOfType<LaserDictionary>();
        active = activeLasers[0].colors.pressedColor;
        inactive = activeLasers[0].colors.normalColor;
        activeLasers[0].GetComponent<Image>().color = active;
    }
    
    public void SetButtonActive(string laserName)
    {
        for(int i = 0; i < activeLasers.Length; i++)
        {
            if (activeLasers[i].name.CompareTo(laserName) == 0) activeLasers[i].gameObject.SetActive(true);
        }
    }

    public void ActivateLaserType()
    {
        string laser = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).name;
        string laserName = EventSystem.current.currentSelectedGameObject.name;
        int index;
        int.TryParse(laser, out index);
        ld.Directory(index);
        ChangeSprite(index);

        for (int i = 0; i < activeLasers.Length; i++)
        {
            Image img = activeLasers[i].GetComponent<Image>();
            if (activeLasers[i].name.CompareTo(laserName) == 0) img.color = active;
            else img.color = inactive;
        }
    }

    private void ChangeSprite(int index)
    {
        cannon.sprite = laserTypes[index];
        cannon.transform.localPosition = centerLaser[index];
    }
}
