using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeGood
{
    public FactionTypes.tradeableGood tradeableGood;
    public int efficiency;

    public BiomeGood(FactionTypes.tradeableGood _tradeableGood, int _efficiency)
    {
        tradeableGood = _tradeableGood;
        efficiency = _efficiency;
    }
}
