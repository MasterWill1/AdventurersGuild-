using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class FactionDef
{
    public FactionTypes.factionTag factionTag;
    public FactionTypes.factionType factionType;
    public string title;
    public string description;
    public int baseWealth;
    public int baseGoodness;
    public List<LocationTypes.locationSpecificTag> suitableLocations;
    public List<FactionApplicableRole> applicableRoles;
    public List<FactionTypes.tradeableGood> tradeableGoods; //goods that a faction will have to trade
    public List<FactionTypes.tradeableGood> wantedGoods; //goods that a faction may want
    public List<FactionTypes.tradeableGood> workedGoods; //raw resources a faction will travel to and harvest
    public List<ContractTypes.contractGoalTag> likelyQuests; //Quests that arent threat based - ie quests that a faction may want ie a magic shop may want magic items
    public List<FactionChildFaction> boundChildFactions;
    public List<FactionChildFaction> externalChildFactions;


    public FactionDef(XmlNode curFactionNode)
    {
        suitableLocations = new List<LocationTypes.locationSpecificTag>();
        applicableRoles = new List<FactionApplicableRole>();
        tradeableGoods = new List<FactionTypes.tradeableGood>();
        wantedGoods = new List<FactionTypes.tradeableGood>();
        likelyQuests = new List<ContractTypes.contractGoalTag>();
        boundChildFactions = new List<FactionChildFaction>();
        externalChildFactions = new List<FactionChildFaction>();

        Enum.TryParse(curFactionNode.Attributes["label"].Value, out factionTag);

        Enum.TryParse(curFactionNode["factionType"].InnerText, out factionType);

        title = curFactionNode["Title"].InnerText;
        description = curFactionNode["Description"].InnerText;

        baseWealth = int.Parse(curFactionNode["BaseWealth"].InnerText);
        baseGoodness = int.Parse(curFactionNode["BaseGoodness"].InnerText);

        XmlNode suitableLocationsNode = curFactionNode.SelectSingleNode("SuitableLocations");
        XmlNodeList locationNodesList = suitableLocationsNode.SelectNodes("Location");
        foreach(XmlNode node in locationNodesList)
        {
            Enum.TryParse(node.InnerText, out LocationTypes.locationSpecificTag locationTag);
            suitableLocations.Add(locationTag);            
        }

        XmlNode applicableRolesNode = curFactionNode.SelectSingleNode("ApplicableRoles");
        if (applicableRolesNode != null)
        {
            XmlNodeList rolesNodeList = applicableRolesNode.SelectNodes("Role");
            foreach (XmlNode node in rolesNodeList)
            {
                Enum.TryParse(node.Attributes["label"].Value, out FactionTypes.roleTag thisRoleTag);
                int quantity = int.Parse(node["maxNumber"].InnerText);
                int hierarchyPos = int.Parse(node["hierarchyPosition"].InnerText);

                FactionApplicableRole role = new FactionApplicableRole(thisRoleTag, quantity, hierarchyPos);
                applicableRoles.Add(role);
            }
        }

        XmlNode tradeableGoodsNode = curFactionNode.SelectSingleNode("TradeableGoods");
        if (tradeableGoodsNode != null)
        {
            XmlNodeList goodsNodeList = tradeableGoodsNode.SelectNodes("good");
            foreach (XmlNode node in goodsNodeList)
            {
                Enum.TryParse(node.InnerText, out FactionTypes.tradeableGood thisGoodTag);

                tradeableGoods.Add(thisGoodTag);
            }
        }
        else
        {
            tradeableGoods.Add(FactionTypes.tradeableGood.none);
        }

        XmlNode wantedGoodsNode = curFactionNode.SelectSingleNode("WantedGoods");
        if (wantedGoodsNode != null)
        {
            XmlNodeList goodsNodeList = wantedGoodsNode.SelectNodes("good");
            foreach (XmlNode node in goodsNodeList)
            {
                Enum.TryParse(node.InnerText, out FactionTypes.tradeableGood thisGoodTag);

                wantedGoods.Add(thisGoodTag);
            }
        }
        else
        {
            tradeableGoods.Add(FactionTypes.tradeableGood.none);
        }

        XmlNode workedGoodsNode = curFactionNode.SelectSingleNode("WorkedGoods");
        if (workedGoodsNode != null)
        {
            XmlNodeList goodsNodeList = workedGoodsNode.SelectNodes("good");
            foreach (XmlNode node in goodsNodeList)
            {
                Enum.TryParse(node.InnerText, out FactionTypes.tradeableGood thisGoodTag);

                wantedGoods.Add(thisGoodTag);
            }
        }
        else
        {
            tradeableGoods.Add(FactionTypes.tradeableGood.none);
        }

        XmlNode questsNode = curFactionNode.SelectSingleNode("LikelyQuests");
        if (questsNode != null)
        {
            XmlNodeList questNodeList = questsNode.SelectNodes("quest");
            foreach (XmlNode node in questNodeList)
            {
                Enum.TryParse(node.InnerText, out ContractTypes.contractGoalTag thisGoalTag);

                likelyQuests.Add(thisGoalTag);
            }
        }

        XmlNode boundChildFactionsNode = curFactionNode.SelectSingleNode("BoundChildFactions");
        if (boundChildFactionsNode != null)
        {
            XmlNodeList factionNodeList = boundChildFactionsNode.SelectNodes("Faction");
            foreach(XmlNode node in factionNodeList)
            {
                Enum.TryParse(node.Attributes["label"].Value, out FactionTypes.factionTag thisChildFactionTag);
                int hierarchyPos = int.Parse(node["hierarchyPosition"].InnerText);

                FactionChildFaction childFaction = new FactionChildFaction(thisChildFactionTag, hierarchyPos);
                boundChildFactions.Add(childFaction);
            }
        }

        XmlNode externalChildFactionsNode = curFactionNode.SelectSingleNode("ExternalChildFactions");
        if (externalChildFactionsNode != null)
        {
            XmlNodeList factionNodeList = externalChildFactionsNode.SelectNodes("Faction");
            foreach (XmlNode node in factionNodeList)
            {
                Enum.TryParse(node.Attributes["label"].Value, out FactionTypes.factionTag thisChildFactionTag);
                int hierarchyPos = int.Parse(node["hierarchyPosition"].InnerText);

                FactionChildFaction childFaction = new FactionChildFaction(thisChildFactionTag, hierarchyPos);
                externalChildFactions.Add(childFaction);
            }
        }

    }
}
