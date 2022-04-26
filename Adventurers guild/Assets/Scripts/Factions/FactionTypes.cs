using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class FactionTypes
{
    public static List<factionTag> allFactionTagsNames =
        Enum.GetValues(typeof(factionTag)).Cast<factionTag>().ToList();

    public static List<factionTag> getAllFactionTags()
    {
        List<factionTag> outputList = allFactionTagsNames;
        outputList.Remove(factionTag.none);
        return outputList;
    }

    public enum factionTag
    {
        none, populationCenter, blacksmith, tannery, hunters, farmers, unemployedPopulace
    }
    public enum factionType
    {
        none, civilisation
    }
    public enum roleTag
    {
        none, mayor, councilor, businessOwner, worker, townElder, population
    }
    public enum tradeableGood
    {
        none, armour, rawIron, coal, hide, wildAnimals, wood, rawFood, farmland
    }
}
