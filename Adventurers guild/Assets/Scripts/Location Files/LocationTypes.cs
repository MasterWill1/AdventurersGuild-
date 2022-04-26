using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LocationTypes
{
    public static List<locationBiomeTag> allLocationBiomeTags = 
        Enum.GetValues(typeof(locationBiomeTag)).Cast<locationBiomeTag>().ToList();
    public static List<locationSpecificTag> allLocationTags =
        Enum.GetValues(typeof(locationSpecificTag)).Cast<locationSpecificTag>().ToList();

    public enum locationBiomeTag
    {
        none, plains, hills, mountains, desert, jungle, forest
    }
    public enum locationSpecificTag
    {
        none, town, city, dungeon, camp, castle, ruins, wilds, caves
    }

    public static locationBiomeTag getRandomBiome()
    {
        return (locationBiomeTag)UnityEngine.Random.Range(1, System.Enum.GetValues(typeof(locationBiomeTag)).Length);
    }
    public static locationSpecificTag getRandomLocationSpecificTag()
    {
        return (locationSpecificTag)UnityEngine.Random.Range(1, System.Enum.GetValues(typeof(locationSpecificTag)).Length);
    }
    public static List<locationBiomeTag> getAllBiomeTags()
    {        
        List<locationBiomeTag> outputList = allLocationBiomeTags;
        outputList.Remove(locationBiomeTag.none);
        return outputList;
    }
    public static List<locationSpecificTag> getAllLocationTags()
    {
        List<locationSpecificTag> outputList = allLocationTags;
        outputList.Remove(locationSpecificTag.none);
        return outputList;
    }
}
