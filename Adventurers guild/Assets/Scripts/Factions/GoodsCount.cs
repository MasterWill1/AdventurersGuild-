using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsCount
{
    public FactionTypes.tradeableGood Good;
    public int Count;

    public GoodsCount(FactionTypes.tradeableGood _Good, int _count)
    {
        Good = _Good;
        Count = _count;
    }
}
