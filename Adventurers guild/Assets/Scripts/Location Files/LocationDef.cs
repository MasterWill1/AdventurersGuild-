using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class LocationDef
{
    public LocationTypes.locationSpecificTag locationTag;
    public string title;
    public string description;
    public bool isRemovable;
    public List<FactionTypes.tradeableGood> producedGoods;
    public int combatScoreEffect;

    public LocationDef(XmlNode curLocationNode)
    {
        producedGoods = new List<FactionTypes.tradeableGood>();

        Enum.TryParse(curLocationNode.Attributes["label"].Value, out locationTag);

        title = curLocationNode["Title"].InnerText;
        description = curLocationNode["Description"].InnerText;

        isRemovable = HelperFunctions.isXMLStringTrueOrFalse(curLocationNode["IsRemovable"].InnerText);

        XmlNode producedGoodsNode = curLocationNode.SelectSingleNode("ProducedGoods");
        if (producedGoodsNode != null)
        {
            XmlNodeList goodsNode = producedGoodsNode.SelectNodes("good");
            foreach (XmlNode node in goodsNode)
            {
                Enum.TryParse(node.Attributes["label"].Value, out FactionTypes.tradeableGood thisGoodTag);
                producedGoods.Add(thisGoodTag);
            }
        }

        combatScoreEffect = int.Parse(curLocationNode["CombatScoreEffect"].InnerText);
    }
}
