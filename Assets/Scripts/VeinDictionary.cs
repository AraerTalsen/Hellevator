/*Daniel Greenberg
 * Last Updated: 1/14/20
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//The VeinDictionary class is a list of world structures and how they are generated
public class VeinDictionary : MonoBehaviour
{
    public GameObject littleMush, bigLittle, bigTough, shooter, follower;
    private Vector2[] checkForOpening = {Vector2.zero, Vector2.up, -Vector2.up, Vector2.right, -Vector2.right };
    private int chunkID, blockSize;
    private MineralDictionary dictionary;
    private DynamicAspectRatio dar;
    private List<string> blackList;
    private float mod;

    private void Start()
    {
        dar = FindObjectOfType<DynamicAspectRatio>();
        dictionary = FindObjectOfType<MineralDictionary>();
        mod = dar.GetModifier();
        blockSize = BlockManager.blockSize;
    }

    //World structure types
    public enum VeinTypes
    {
        SHOOTER_CAVE, LITTLE_MUSH_VEIN, BIG_LITTLE_VEIN, BIG_TOUGH_VEIN, GRUNT_CAVE, EMPTY_CAVE
    }

    //Locates place to root the world structure
    public GameObject[,] ImplantVein(VeinTypes vein, Chunk c, Vector2 root, Vector2 size)
    {
        chunkID = c.GetChunkID();
        GameObject[,] chunk = c.GetChunk();
        int x = (int)size.x, y = (int)size.y;

        switch (vein)
        {
            case VeinTypes.SHOOTER_CAVE:
                {
                    if (root.x <= c.GetWidth() - x && root.y <= c.GetHeight() - y)
                    {
                        chunk = CarveCave(chunk, root, root + size, VeinTypes.SHOOTER_CAVE, c);
                    }
                    else
                    {
                        Vector2 start = Vector2.zero;
                        Vector2 end = new Vector2(x, y);

                        chunk = CaveGenerationProcess(start, end, chunk, c, VeinTypes.SHOOTER_CAVE);
                    }
                    return chunk;
                }
             /*case VeinTypes.GRUNT_CAVE:
                {
                    if (root.x <= c.GetWidth() - x && root.y <= c.GetHeight() - y)
                    {
                        for (int i = 0; i < x; i++)
                        {
                            for (int j = 0; j < y; j++)
                            {
                                Destroy(chunk[(int)root.x + i, (int)root.y + j]);
                                chunk[(int)root.x + i, (int)root.y + j] = null;
                            }
                        }
                    }
                    else
                    {
                        Vector2 start = Vector2.zero;
                        Vector2 end = new Vector2(x, y);

                        chunk = CaveGenerationProcess(start, end, chunk, c, VeinTypes.GRUNT_CAVE);

                    }
                    return chunk;
                }*/            
            case VeinTypes.LITTLE_MUSH_VEIN:
                {
                    blackList = new List<string>
                    {
                        dictionary.GetMineral(MineralDictionary.MineralTypes.LITTLE_MUSH).GetComponent<Block>().GetID(),
                        null
                    };

                    return OreVeinGenerator(root, chunk, new Vector2(x, y), c, MineralDictionary.MineralTypes.LITTLE_MUSH);
                }
            case VeinTypes.BIG_LITTLE_VEIN:
                {
                    blackList = new List<string>
                    {
                        dictionary.GetMineral(MineralDictionary.MineralTypes.LITTLE_MUSH).GetComponent<Block>().GetID(),
                        dictionary.GetMineral(MineralDictionary.MineralTypes.BIG_LITTLE).GetComponent<Block>().GetID(),
                        null
                    };

                    return OreVeinGenerator(root, chunk, new Vector2(x, y), c, MineralDictionary.MineralTypes.BIG_LITTLE);
                }
            case VeinTypes.BIG_TOUGH_VEIN:
                {
                    blackList = new List<string>
                    {
                        dictionary.GetMineral(MineralDictionary.MineralTypes.LITTLE_MUSH).GetComponent<Block>().GetID(),
                        dictionary.GetMineral(MineralDictionary.MineralTypes.BIG_LITTLE).GetComponent<Block>().GetID(),
                        dictionary.GetMineral(MineralDictionary.MineralTypes.BIG_TOUGH).GetComponent<Block>().GetID(),
                        null
                    };

                    return OreVeinGenerator(root, chunk, new Vector2(x, y), c, MineralDictionary.MineralTypes.BIG_TOUGH);
                }
            /*case VeinTypes.EMPTY_CAVE:
                {                   
                    if (root.x <= c.GetWidth() - x && root.y <= c.GetHeight() - y)
                    {
                        for(int i = 0; i < x; i++)
                        {
                            for(int j = 0; j < y; j++)
                            {
                                Destroy(chunk[(int)root.x + i, (int)root.y + j]);
                                chunk[(int)root.x + i, (int)root.y + j] = null;
                            }                            
                        }                        
                    }
                    else
                    {
                        Vector2 start = Vector2.zero;
                        Vector2 end = new Vector2(x, y);

                        chunk = CaveGenerationProcess(start, end, chunk, c, VeinTypes.EMPTY_CAVE);
                        
                    }      
                    return chunk;
                }*/
            default:
                {
                    return chunk;
                }
        }
    }

    //Places a randomly shaped vein of ore starting from the root
    private GameObject[,] OreVeinGenerator(Vector2 root, GameObject[,] chunk, Vector2 size, Chunk c, MineralDictionary.MineralTypes type)
    {
        Vector2 current = root;
        int x = (int)size.x, y = (int)size.y;
        int z = x * y;
        Vector2[] previous = new Vector2[x * y];       
        int head = 0;
        Vector2 adjust;
        bool lowerX, lowerY, upperX, upperY, empty;
        for (int i = 0; i < z; i++)
        {
            List<int> rando = new List<int>();

            for (int r = 1; r < checkForOpening.Length; r++) rando.Add(r);

            for (int k = 0; k < 5; k++)
            {
                int num;
                if (k == 0)
                    num = 0;
                else
                {
                    int r = Random.Range(0, rando.Count);
                    num = rando[r];
                    rando.RemoveAt(r);
                }
                    
                adjust = current + checkForOpening[num];
                lowerX = adjust.x * blockSize > -1;
                lowerY = adjust.y * blockSize > -1;
                upperX = adjust.x * blockSize <= c.GetWidth() * blockSize - 1;
                upperY = adjust.y * blockSize <= c.GetHeight() * blockSize - 1;                    
                    
                if (lowerX && lowerY && upperX && upperY)
                {
                    empty = blackList.TrueForAll(g => chunk[(int)adjust.x, (int)adjust.y].GetComponent<Block>().GetID().CompareTo(g) != 0);

                    if (empty)
                    {
                        Destroy(chunk[(int)adjust.x, (int)adjust.y]);
                        chunk[(int)adjust.x, (int)adjust.y] = Instantiate(dictionary.GetMineral(type));
                        chunk[(int)adjust.x, (int)adjust.y].transform.SetParent(c.transform);
                        chunk[(int)adjust.x, (int)adjust.y].transform.localScale *= blockSize * mod;
                        chunk[(int)adjust.x, (int)adjust.y].transform.localPosition = new Vector2((int)adjust.x, (int)adjust.y) * blockSize * mod;
                        previous[head] = current;
                        head++;
                        current = adjust;
                        break;
                    }
                }
                else if (k == 4)
                {
                    head--;
                    current = previous[head];
                }
            }
        }
        return chunk;
    }

    //Locates a place to root a cave so that the entire cave can be generated (cave has specific shape)
    private GameObject[,] CaveGenerationProcess(Vector2 start, Vector2 end, GameObject[,] chunk, Chunk c, VeinTypes type)
    {
        int x = (int)end.x;//(int)(end.x - start.x);//Start is a vector2.zero, is there a point of even having start?
        int y = (int)end.y;//(int)(end.y - start.y);
        int num;
        Vector2 debug = new Vector2(x, y);

        List<int> blocks = new List<int>();//each tile in chunk matrix
        for (int i = 0; i < chunk.Length; i++) blocks.Add(i);

        for (int i = blocks.Count, d = 0; i > 0; i = blocks.Count, d++)
        {
            int r = Random.Range(0, blocks.Count);//choose random tile
            num = blocks[r];

            //Convert tile number to location in matrix
            int indexX = num / c.GetWidth();
            int indexY = num % c.GetWidth();
            start = new Vector2(indexX, indexY);
            end = new Vector2(start.x + x, start.y + y);

            //Determine if cave will extend outside of chunk
            bool endXBig = end.x > c.GetWidth() - 1, endYBig = end.y > c.GetHeight() - 1, breakout = false;
            
            for (int j = (int)start.x; j < (int)end.x; j++)
            {
                for (int k = (int)start.y; k < (int)end.y; k++)
                {
                    /*If the starting point is occupied (or empty such as for another cave) or the 
                    if the cave would stretch outside of the chunk, remove suggested tiles from list
                    of potential generation points. Otherwise, carve out the cave.*/
                    if (chunk[j, k] == null || endXBig || endYBig)//chunk[j, k]?
                    {
                        bool check1 = j >= indexX && j <= end.x, check2 = end.x >= c.GetWidth() - 1;
                        bool check3 = end.y >= c.GetHeight() - 1;

                        List<Vector2> range = new List<Vector2>
                        {
                            new Vector2(j - x >= 0 ? j - x : 0, j),
                            new Vector2(k - y >= 0 ? k - y : 0, k),
                            new Vector2(c.GetWidth() - x, c.GetWidth() - 1),
                            new Vector2(0, c.GetHeight() - 1),
                            new Vector2(0, c.GetWidth() - 1),
                            new Vector2(c.GetHeight() - y, c.GetHeight() - 1)
                        };

                        if(check1) DeductList(range[0], range[1], blocks, c);
                        if(check2) DeductList(range[2], range[3], blocks, c);
                        if(check3) DeductList(range[4], range[5], blocks, c);
                        /*bool a = indexX <= x, b = c.GetWidth() - 1 - indexX >= c.GetWidth() - 1 - x;//index <= x or y should always be true
                        bool d = c.GetHeight() - 1 - indexY >= c.GetHeight() - 1 - y, e = indexY <= y;
                        List<Vector2> points = new List<Vector2>
                        {
                            new Vector2(0, indexX),
                            new Vector2(indexY, c.GetWidth() - 1),//ad
                            new Vector2(0, indexX),
                            new Vector2(indexX, 1),//a
                            new Vector2(0, indexX),
                            new Vector2(0, indexY),//ae                            
                            new Vector2(indexX, c.GetWidth() - 1),
                            new Vector2(indexX, 1),//b
                            new Vector2(indexX, c.GetWidth() - 1),
                            new Vector2(indexY, c.GetHeight() - 1),//bd
                            new Vector2(indexY, c.GetWidth() - 1),
                            new Vector2(indexX, 0),//d
                            new Vector2(indexX, c.GetWidth() - 1),
                            new Vector2(0, indexY),//be 
                            new Vector2(0, indexY),
                            new Vector2(indexX, 0),//e
                        };
                        List<bool> checks = new List<bool>
                        {
                            a && d, a && e, a, b && d, b && e, b, d, e, true
                        };*/

                        /*int index = -1;//never -1
                        for (int l = 0; l < checks.Count; l++) if (checks[l]) { index = l; break; }
                        if (index == -1)
                        {
                            try//Attempt to remove tiles from list of potential cave generation spots
                            {
                                if (index % 2 != 0)
                                {
                                    int xVal = (int)points[index + 1].x;//converts check index to points index
                                    bool orientation = (int)points[index + 1].y == 1;
                                    DeductList(points[index], xVal, orientation, blocks, c);
                                }
                                else if (index < 8) DeductList(points[index], points[index + 1], blocks, c);
                                else blocks.Remove(num);
                            }
                            catch (System.Exception ex)
                            {
                                print("It's fucked");
                            }
                        }*/
                        breakout = true;
                        break;
                    }
                    else if (j == end.x - 1 && k == end.y - 1) return chunk = CarveCave(chunk, start, end, type, c);
                    if(k > 3) print("Too tall: " + type + " " + debug); break;
                }
                if (breakout) break;
                if (j > 3) print("Too wide: " + type + " " + debug); break;
            }
            if (d > 15) print("Cycle through tiles break: " + type + " " + debug + " " + blocks.Count); break;
        }
        return chunk;
    }

    //Removes tiles where a cave would be unable to generate (if multiple layers of tiles are to be removed)
    private void DeductList(Vector2 width, Vector2 height, List<int> blocks, Chunk c)
    {
        for (int i = (int)height.y; i >= (int)height.x; i--)
        {
            for (int j = (int)width.x; j < (int)width.y; j++)
            {
                int val = i * (c.GetHeight() - 1) + j;
                blocks.Remove(val);
            }
        }
    }

    /*//Removes tiles where a cave would be unable to generate (if one layer of tiles is to be removed)
    private void DeductList(Vector2 loop1, int xVal, bool orientation, List<int> blocks, Chunk c)
    {
        for (int i = (int)loop1.x; i < (int)loop1.y; i++)
        {
            int val = (xVal + (orientation ? i : 0)) * (c.GetHeight() - 1) + (orientation ? i : 0);
            blocks.Remove(val);
        }
    }*/

    //When available space for a cave to generate is located, place cave there
    private GameObject[,] CarveCave(GameObject[,] chunk, Vector2 start, Vector2 end, VeinTypes type, Chunk c)
    {
        for (int i = (int)start.x; i < (int)end.x; i++)
        {
            for (int j = (int)start.y; j < (int)end.y; j++)
            {
                Destroy(chunk[i, j]);
                chunk[i, j] = null;
            }
        }
        if(type != VeinTypes.EMPTY_CAVE) EnemyPlacer(start, end, type, c);

        return chunk;
    }

    //Place enemy in cave if an enemy belongs in the cave
    private void EnemyPlacer(Vector2 start, Vector2 end, VeinTypes type, Chunk c)
    {
        GameObject instance;
        FollowerSpawner.enemyCount++;
        switch(type)
        {
            case VeinTypes.SHOOTER_CAVE:
                {
                    Vector2 spawnPoint;
                    int x = (int)(start.x - 1 + ((end.x + 1 - start.x) / 2));
                    int y = (int)start.y;
                    
                    spawnPoint = new Vector2(x, y) * blockSize * mod;
                    instance = Instantiate(shooter);
                    instance.transform.SetParent(c.transform);
                    instance.transform.localScale *= blockSize * mod;
                    instance.transform.localPosition = spawnPoint;
                    break;
                }
            /*case VeinTypes.GRUNT_CAVE:
                {                    
                    Vector2 spawnPoint;
                    int x = (int)(start.x - 1 + ((end.x + 1 - start.x) / 2));
                    int y = (int)end.y - 1;
                    spawnPoint = new Vector2(x, y) * blockSize * mod;
                    instance = Instantiate(follower);
                    instance.transform.SetParent(c.transform);
                    instance.transform.localScale *= blockSize * mod;
                    instance.transform.localPosition = spawnPoint;
                    break;
                }*/
        }
    }
}
