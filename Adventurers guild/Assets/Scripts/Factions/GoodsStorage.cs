using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsStorage
{
    public List<GoodsCount> goodsCountList;

    public GoodsStorage()
    {

    }

    public void addGoods(FactionTypes.tradeableGood tradeableGood, int count)
    {
        bool goodFound = false;
        foreach(GoodsCount goodsCount in goodsCountList)
        {
            if(goodsCount.Good == tradeableGood)
            {
                goodsCount.Count = goodsCount.Count + count;
                goodFound = true;
                break;
            }
        }

        if(goodFound == false)
        {
            GoodsCount newGoodsCount = new GoodsCount(tradeableGood, count);
            goodsCountList.Add(newGoodsCount);
        }
    }
}
