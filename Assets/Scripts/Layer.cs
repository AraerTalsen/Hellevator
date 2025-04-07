/*Daniel Greenberg
 * Last Updated: 1/14/20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Layer class is a row of chunks in the tunnle
public class Layer : MonoBehaviour
{
    public GameObject chunk;
    private GameObject left, right;
    private Camera cam;
    public static Vector2 widthBorders, heightBorders;

    //Sets the distance each column of chunks is from the border of the camera in segments of two chunks called a layer
    void Start()
    {       
        cam = Camera.main;
        left = Instantiate(chunk);
        right = Instantiate(chunk);

        left.transform.SetParent(transform);
        right.transform.SetParent(transform);

        //float numPixels = UnitSize();

        /*float width = left.transform.localScale.x;
        Vector2 adjustedOne = cam.WorldToScreenPoint(Vector2.zero);
        Vector2 adjustedTwo = cam.WorldToScreenPoint(new Vector2(width, 0));
        float sumX = (adjustedTwo.x - adjustedOne.x) / 2;
        print(adjustedOne + " " + adjustedTwo);

        float height = left.transform.localScale.x;
        adjustedOne = cam.WorldToScreenPoint(Vector2.zero);
        adjustedTwo = cam.WorldToScreenPoint(new Vector2(0, height));
        float sumY = (adjustedTwo.x - adjustedOne.x) / 2;

        Vector2 shapeDiam = new Vector2(sumX, sumY);*/

        /*int unitWidth = left.GetComponent<Chunk>().width;
        float centerPivot = numPixels / 2;
        float actualWidth = numPixels * unitWidth - centerPivot;

        Vector2 rightBorder = cam.ScreenToWorldPoint(new Vector2(cam.pixelRect.width - actualWidth, cam.pixelRect.height / 2), 0);
        Vector2 leftBorder = cam.ScreenToWorldPoint(new Vector2(centerPivot, cam.pixelRect.height / 2), 0);

        left.transform.localPosition = leftBorder;
        right.transform.localPosition = rightBorder;

        widthBorders.x = cam.ScreenToWorldPoint(new Vector2(0, 0), 0).x;
        widthBorders.y = cam.ScreenToWorldPoint(new Vector2(cam.pixelRect.width, 0), 0).x;
        heightBorders.x = cam.ScreenToWorldPoint(new Vector2(0, 0), 0).y;
        heightBorders.y = cam.ScreenToWorldPoint(new Vector2(0, cam.pixelRect.height), 0).y;*/

        SetTunnelWalls();

    }

    private float UnitSize()
    {
        /*Sprite unit = left.GetComponent<Chunk>().GetUnit();
        Vector2 unitStart = unit.rect.position;
        Vector2 unitEnd = unitStart + unit.rect.size;
        unitStart = cam.WorldToScreenPoint(unitStart);
        unitEnd = cam.WorldToScreenPoint(unitEnd);
        print(unitStart + " " + unitEnd);

        return cam.WorldToScreenPoint(unitEnd).x - cam.WorldToScreenPoint(unitStart).x;*/
        float mod = FindObjectOfType<DynamicAspectRatio>().GetModifier();
        float blockScale = BlockManager.blockSize;
        Vector2 start = cam.WorldToScreenPoint(Vector2.zero);
        Vector2 end = cam.WorldToScreenPoint(new Vector2(mod * blockScale, 0));
        return end.x - start.x;
    }

    private void SetTunnelWalls()
    {
        //Find edges of screen
        //Find width of each chunk
        //Set position of each chunk flush with the screen

        float screenRight =  Screen.width;
        //float screenTop =  Screen.height;

        Vector2 worldLeftPos = cam.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 worldRightPos = cam.ScreenToWorldPoint(new Vector2(screenRight, 0));

        Chunk chunk = left.GetComponent<Chunk>();
        float chunkWidth = chunk.GetWidth();
        float blockWidth = chunk.blockSize;
        //float chunkHeight = chunk.GetWidth();

        left.transform.localPosition = new Vector2(worldLeftPos.x + (blockWidth / 2), 0);
        right.transform.localPosition = new Vector2(worldRightPos.x - (chunkWidth * 2) + (blockWidth / 2), 0);
        print("The chunks should place at positions: " + left.transform.localPosition + " " + right.transform.localPosition);
        print("Chunk width should be " + chunkWidth);
    }
}
