/*Daniel Greenberg
 * Last Updated: 1/14/20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The MineralDictionary class is a list of valuable minerals deposited in the tunnel walls of the chunk
public class MineralDictionary: MonoBehaviour
{
    public GameObject littleMush, bigLittle, bigTough;//, bronze, silver, gold, platinum, diamond, ruby, emerald, saphire, lapis, corundum;

    public enum MineralTypes
    {
        LITTLE_MUSH, BIG_LITTLE, BIG_TOUGH//, BRONZE, SILVER, GOLD, PLATINUM, DIAMOND, RUBY, EMERALD, SAPHIRE, LAPIS_LAZULI, CORUNDUM  
    }

    //Returns a prefab of the requested mineral
    public GameObject GetMineral(MineralTypes stone)
    {
        switch (stone)
        {
            case MineralTypes.LITTLE_MUSH:
                {
                    return littleMush;
                }
            case MineralTypes.BIG_LITTLE:
                {
                    return bigLittle;
                }
            case MineralTypes.BIG_TOUGH:
                {
                    return bigTough;
                }
            /*case MineralTypes.BRONZE:
                {
                    return bronze;
                }
            case MineralTypes.SILVER:
                {
                    return silver;
                }
            case MineralTypes.GOLD:
                {
                    return gold;
                }
            case MineralTypes.PLATINUM:
                {
                    return platinum;
                }
            case MineralTypes.DIAMOND:
                {
                    return diamond;
                }
            case MineralTypes.RUBY:
                {
                    return ruby;
                }
            case MineralTypes.EMERALD:
                {
                    return emerald;
                }
            case MineralTypes.SAPHIRE:
                {
                    return saphire;
                }
            case MineralTypes.LAPIS_LAZULI:
                {
                    return lapis;
                }
            case MineralTypes.CORUNDUM:
                {
                    return corundum;
                }*/
            default:
                {
                    return null;
                }
        }
    }
}
