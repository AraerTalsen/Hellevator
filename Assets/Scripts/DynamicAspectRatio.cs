/*Daniel Greenberg
 * Last Updated: 1/14/20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The DynamicAspectRatio adjusts the sizes of the scenes based on screen size and aspect ratio
public class DynamicAspectRatio : MonoBehaviour
{
    public GameObject game, left, right, bottom, top;
    public Vector2 defaultRatio;
    public float oneForOne;
    public Camera cam;
    public static float modifier, borderX, borderY, worldWidth, worldBorderX, worldX0, worldY0;
    private float width, height;    
    private GameObject chosen1, chosen2;
    private bool widthOrHeight;

    //Finds screen size and sets a modifier based on the size compared to a publicly set default ratio
    void Awake()
    {
        /*Vector2 currentRatio = new Vector2(cam.pixelWidth, cam.pixelHeight);
        float modifierY = currentRatio.y / (defaultRatio.y * oneForOne);
        float modifierX = currentRatio.x / (defaultRatio.x * oneForOne);
        modifier = modifierX < modifierY ? modifierX : modifierY;

        game.transform.localScale *= modifier;

        width = defaultRatio.x * oneForOne * modifier;
        height = defaultRatio.y * oneForOne * modifier;

        borderX = cam.pixelWidth - width;
        borderY = cam.pixelHeight - height;

        worldX0 = cam.ScreenToWorldPoint(Vector2.zero).x;
        worldY0 = cam.ScreenToWorldPoint(Vector2.zero).y;

        ScaleBorder();*/
    }

    private void ScaleBorder()
    {
        float worldHeight, worldBorderY;

        Vector2 origin = cam.ScreenToWorldPoint(Vector2.zero);

        worldHeight = Vector2.Distance(cam.ScreenToWorldPoint(new Vector2(0, height + 1)), origin);
        worldWidth = Vector2.Distance(cam.ScreenToWorldPoint(new Vector2(width + 1, 0)), origin);
        worldBorderY = Vector2.Distance(cam.ScreenToWorldPoint(new Vector2(0, borderY)), origin);
        worldBorderX = cam.ScreenToWorldPoint(new Vector2(borderX, 0)).x - origin.x;

        if (borderX == 0)
        {
            
            left.transform.localScale = new Vector2(0, 0);
            right.transform.localScale = new Vector2(0, 0);
            top.transform.localScale = new Vector2(worldWidth, worldBorderY / 2);
            bottom.transform.localScale = new Vector2(worldWidth, worldBorderY / 2);

            left.transform.position = new Vector2(-worldWidth / 2 - 1, 0);
            right.transform.position = new Vector2(worldWidth / 2 + 1, 0);
            top.transform.localPosition = new Vector2(0, worldHeight / 2 + worldBorderY / 4);
            bottom.transform.localPosition = new Vector2(0, -worldHeight / 2 - worldBorderY / 4);
        }
        else
        {
            left.transform.localScale = new Vector2(worldBorderX / 2, worldHeight);
            right.transform.localScale = new Vector2(worldBorderX / 2, worldHeight);
            top.transform.localScale = new Vector2(0, 0);
            bottom.transform.localScale = new Vector2(0, 0);

            left.transform.position = new Vector2(-worldWidth / 2 - 1, 0);
            right.transform.position = new Vector2(worldWidth / 2 + 1, 0);
            top.transform.localPosition = new Vector2(0, worldHeight / 2 + 1);
            bottom.transform.localPosition = new Vector2(0, -worldHeight / 2 - 1);
        }
    }

    public float GetModifier()
    {
        return 1;//modifier;
    }
}
