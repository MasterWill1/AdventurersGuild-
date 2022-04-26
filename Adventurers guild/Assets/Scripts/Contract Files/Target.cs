using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class Target
{
    public ContractTypes.contractTargetTag contractTargetTag; //targets enum tag
    public string targetName; //targets string name
    public List<LocationTypes.locationBiomeTag> allowedBiomes; //biome that target will be found in 
    public List<LocationTypes.locationSpecificTag> allowedLocations; //locations that the target will inhabit within the tile
    public ContractTypes.targetType targetType; //what kind of target is this, ie a beast, undead, humanoid etc
    public int commonality; //how often a target of this type may be encountered
    public List<applicableGoal> applicableGoalsList; //applicable goals for contract for this target type

    public Target(XmlNode curItemNode)
    {
        allowedBiomes = new List<LocationTypes.locationBiomeTag>();
        allowedLocations = new List<LocationTypes.locationSpecificTag>();
        applicableGoalsList = new List<applicableGoal>();

        //target tag
        Enum.TryParse(curItemNode.Attributes["label"].Value, out contractTargetTag);
        
        //target name
        targetName = curItemNode["defName"].InnerText;

        //If the allowedBiome attribute is present
        if(curItemNode.SelectSingleNode("allowedBiome") != null)
        {
            XmlNode biomeListContainer = curItemNode.SelectSingleNode("allowedBiome");
            XmlNodeList biomeList = biomeListContainer.SelectNodes("li");

            foreach (XmlNode xmlNode in biomeList)
            {
                LocationTypes.locationBiomeTag thisBiome;
                Enum.TryParse(xmlNode.InnerText, out thisBiome);
                allowedBiomes.Add(thisBiome);
            }
        }
        //If the disallowed biome attribute is present
        else if(curItemNode.SelectSingleNode("disallowedBiome") != null)
        {
            List<LocationTypes.locationBiomeTag> allLocationBiomeTags = LocationTypes.getAllBiomeTags();

            XmlNode biomeListContainer = curItemNode.SelectSingleNode("disallowedLocations");
            XmlNodeList biomeList = biomeListContainer.SelectNodes("li");

            foreach (XmlNode xmlNode in biomeList)
            {
                LocationTypes.locationBiomeTag thisBiome;
                Enum.TryParse(xmlNode.InnerText, out thisBiome);
                allLocationBiomeTags.Remove(thisBiome);
            }
            allowedBiomes = allLocationBiomeTags;
        }
        //else there is no disallowed or allowed tag, therefore all biomes are allowed
        else
        {
            allowedBiomes = LocationTypes.getAllBiomeTags();
        }

        //If the allowedLocation attribute is present
        if (curItemNode.SelectSingleNode("allowedLocations") != null)
        {
            XmlNode locationListContainer = curItemNode.SelectSingleNode("allowedLocations");
            XmlNodeList locationList = locationListContainer.SelectNodes("li");

            foreach (XmlNode xmlNode in locationList)
            {
                LocationTypes.locationSpecificTag thisLocation;
                Enum.TryParse(xmlNode.InnerText, out thisLocation);
                allowedLocations.Add(thisLocation);
            }
        }
        //If the disallowedLocation attribute is present
        else if (curItemNode.SelectSingleNode("disallowedLocations") != null)
        {
            List<LocationTypes.locationSpecificTag> allLocationTags = LocationTypes.getAllLocationTags();

            XmlNode locationListContainer = curItemNode.SelectSingleNode("disallowedLocations");
            XmlNodeList locationList = locationListContainer.SelectNodes("li");

            foreach (XmlNode xmlNode in locationList)
            {
                LocationTypes.locationSpecificTag thisLocation;
                Enum.TryParse(xmlNode.InnerText, out thisLocation);
                allLocationTags.Remove(thisLocation);
            }
            allowedLocations = allLocationTags;
        }
        //else there is no disallowed or allowed tag, therefore all locations are allowed
        else
        {
            allowedLocations = LocationTypes.getAllLocationTags();
        }

        Enum.TryParse(curItemNode["targetType"].InnerText, out targetType);

        commonality = int.Parse(curItemNode["commonality"].InnerText);

        XmlNode applicableGoalsNode = curItemNode.SelectSingleNode("applicableGoals");
        foreach (XmlNode goalNode in applicableGoalsNode)
        {
            Enum.TryParse(goalNode.Attributes["label"].Value, out ContractTypes.contractGoalTag contractGoalTag);
            int difficultyMin = int.Parse(goalNode["difficultyMin"].InnerText);
            int difficultyMax = int.Parse(goalNode["difficultyMax"].InnerText);

            applicableGoal applicableGoal = new applicableGoal(contractGoalTag, difficultyMin, difficultyMax);
            applicableGoalsList.Add(applicableGoal);
        }

    }
}
