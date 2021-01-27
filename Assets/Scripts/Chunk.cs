/*Daniel Greenberg
 * Last Updated: 1/14/20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Chunk class is for compiling a chunk of entities out of tiles and enemies
public class Chunk : MonoBehaviour
{
    public int width, height, blockSize, bufferUnit;
    private GameObject current, next;
    private GameObject[,] chunk;
    private StoneDictionary sDictionary;
    private VeinDictionary vDictionary;
    private DynamicAspectRatio dar;
    public int chunkID;
    private float mod;
    private Sprite unitSprite = null;

    //private string[,] proximity = new string[3, 3];//"f"=full, "e"=empty, "o"=out of bounds
    public GameObject block, corner;

    // Start is called before the first frame update
    void Awake()
    {
        chunkID = Random.Range(0, 999999);
        sDictionary = FindObjectOfType<StoneDictionary>();
        vDictionary = FindObjectOfType<VeinDictionary>();
        dar = FindObjectOfType<DynamicAspectRatio>();
        blockSize = BlockManager.blockSize;
        bufferUnit = 4 / blockSize;
        mod = dar.GetModifier();

        width /= blockSize;
        height /= blockSize;

        chunk = new GameObject[width, height];

        for(int i = height - 1; i >= 0; i--)
        {
            for (int j = 0; j < width; j++)
            {
                chunk[j, i] = Instantiate(sDictionary.GetStone(StoneDictionary.StoneTypes.DIRT));
                chunk[j, i].transform.SetParent(transform);
                chunk[j, i].transform.localScale *= blockSize * mod;
                chunk[j, i].transform.localPosition = new Vector2(j , i) * blockSize * mod;
                if (unitSprite == null) unitSprite = chunk[j, i].GetComponent<SpriteRenderer>().sprite;
            }
        }
        GenerateCaves();
    }
    /*private void CheckForDestruction()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                current = chunk[i, j];
                next = j + 1 < height ? chunk[i, j + 1] : current;
                if (current == null && next != null)
                    BuildSmoothCorners(new Vector2(i, j + 1));
                else if(current != null && next == null)
                    BuildSmoothCorners(new Vector2(i, j));
            }
        }
    }

    private void BuildSmoothCorners(Vector2 index)
    {
        if(index.y != 0 && index.y < height)
        {
            Destroy(chunk[(int)index.x, (int)index.y - 1]);
            Destroy(chunk[(int)index.x, (int)index.y + 1]);
            chunk[(int)index.x, (int)index.y] = Instantiate(corner, new Vector2(index.x, index.y - 1), Quaternion.identity);
            chunk[(int)index.x, (int)index.y].transform.SetParent(transform);
            chunk[(int)index.x, (int)index.y] = Instantiate(corner, new Vector2(index.x, index.y + 1), Quaternion.identity);
            chunk[(int)index.x, (int)index.y].transform.SetParent(transform);
        }
    }*/

     //Generates random values for the cave building blueprints in VeinDictionary
    private void GenerateCaves()
    {
        int pX, pY, sX, sY, quantity;
        VeinDictionary.VeinTypes type;

        quantity = 1;//Random.Range(1, chunk.Length / (width + 1));

        for(int i = 0; i < quantity; i++)
        {
            pX = Random.Range(0, width);
            pY = Random.Range(0, height);
            sX = Random.Range(bufferUnit, width - bufferUnit / 2);//These don't work if the chunk size is too small
            sY = Random.Range(bufferUnit, height - bufferUnit / 2);
            type = (VeinDictionary.VeinTypes)Random.Range(0, 4);

            vDictionary.ImplantVein(type, this, new Vector2(pX, pY), new Vector2(sX, sY));
        }
    }

    public GameObject[,] GetChunk() { return chunk;  }
    public int GetChunkID() { return chunkID;  }
    public int GetWidth() { return width; }
    public int GetHeight() { return height; }
    public Sprite GetUnit() { return unitSprite; }
}
