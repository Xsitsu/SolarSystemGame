using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntry
{
    public Item item;
    public int amount;

    public double GetTotalVolume()
    {
        return item.volume * amount;
    }
    public double GetTotalWeight()
    {
        return item.weight * amount;
    }
}
