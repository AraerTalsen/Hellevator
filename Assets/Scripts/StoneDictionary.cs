/*Daniel Greenberg
 * Last Updated: 1/14/20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//StoneDictionary is a list of cave wall tiles that can be used when generating chunks (excluding precious minerals)
public class StoneDictionary: MonoBehaviour
{
    public GameObject dirt, limestone, marble, granite, gneiss;

    public enum StoneTypes
    {
        DIRT, LIMESTONE, MARBLE, GRANITE, GNEISS
    }

    //Returns stone tile prefab from list
    public GameObject GetStone(StoneTypes stone)
    {
        switch(stone)
        {
            case StoneTypes.DIRT:
            {
                    return dirt;    
            }
            case StoneTypes.LIMESTONE:
            {
                return limestone;
            }
            case StoneTypes.MARBLE:
            {
                return marble;
            }
            case StoneTypes.GRANITE:
            {
                return granite;
            }
            case StoneTypes.GNEISS:
            {
                return gneiss;
            }
            default:
            {
                return dirt;
            }
        }
    }
}
