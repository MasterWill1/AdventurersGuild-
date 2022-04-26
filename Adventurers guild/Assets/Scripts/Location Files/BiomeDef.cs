using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class BiomeDef
{
    public LocationTypes.locationBiomeTag locationBiomeTag;
    public string title;
    public string description;
    public List<BiomeGood> availableGoods;
    public int travelSpeed;

    public BiomeDef(XmlNode curBiomeNode)
    {
        availableGoods = new List<BiomeGood>();

        Enum.TryParse(curBiomeNode.Attributes["label"].Value, out locationBiomeTag);

        title = curBiomeNode["Title"].InnerText;
        description = curBiomeNode["Description"].InnerText;

        XmlNode producedGoodsNode = curBiomeNode.SelectSingleNode("ProducedGoods");
        if(producedGoodsNode != null)
        {
            XmlNodeList goodsNode = producedGoodsNode.SelectNodes("good");
            foreach(XmlNode node in goodsNode)
            {
                Enum.TryParse(node.Attributes["label"].Value, out FactionTypes.tradeableGood thisGoodTag);
                int efficiency = int.Parse(node["efficiency"].InnerText);

                BiomeGood thisBiomeGood = new BiomeGood(thisGoodTag, efficiency);
                availableGoods.Add(thisBiomeGood);
            }
        }

        travelSpeed = int.Parse(curBiomeNode["TravelSpeed"].InnerText);
    }
}
