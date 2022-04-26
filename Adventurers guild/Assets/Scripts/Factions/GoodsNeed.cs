using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class GoodsNeed
{
    public FactionTypes.tradeableGood Good;
    public int numberPer10Pop;

    public GoodsNeed(XmlNode curNeedNode)
    {
        Enum.TryParse(curNeedNode.Attributes["label"].Value, out Good);
        numberPer10Pop = int.Parse(curNeedNode["numberPer10Pop"].InnerText);
    }
}
