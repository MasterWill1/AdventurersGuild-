using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ContractTypes
{
    public static List<contractTargetTag> allTargetTags =
        Enum.GetValues(typeof(contractTargetTag)).Cast<contractTargetTag>().ToList();

    public enum pluralInfo
    {
        singlular, plural, changeable
    }
    public enum contractTargetTag
    {
        none,
        //humanoids
        goblin,
        goblinWarlord,
        human,
        elf,
        dwarf,
        halfling,
        halforc,
        //monsters
        giantSpider,
        ogre,
        //beasts
        wolve,
        griffon,
        //undead
        skeleton,
        lich
    }
    public enum contractGoalTag
    {
        none, kill//, gatherMaterials
    }
    public enum targetType
    {
        none, humanoid, beast, monster, undead
    }
    public static contractTargetTag getRandomContractTargetTag()
    {
        return (contractTargetTag)UnityEngine.Random.Range(1, System.Enum.GetValues(typeof(contractTargetTag)).Length);
    }
    public static contractGoalTag getRandomContractGoalTag()
    {
        return (contractGoalTag)UnityEngine.Random.Range(1, System.Enum.GetValues(typeof(contractGoalTag)).Length);
    }
    public static List<contractTargetTag> getAllTargetTags()
    {
        List<contractTargetTag> outputList = allTargetTags;
        outputList.Remove(contractTargetTag.none);
        return outputList;
    }
}
