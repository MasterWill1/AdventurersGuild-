using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CharacterEquipmentTypes : MonoBehaviour
{
    // need to add ability to create lists for armours/weapons etc that are okay for each class.
    // ie list of armour types that a ranger can use, so can just pull from there when generating random armour

    static int basicQualityMultiplier = 1;
    static int advancedQualityMultiplier = 2;
    static int supremeQualityMultiplier = 3;

    public static List<equipmentName> allEquipmentNames =
        Enum.GetValues(typeof(equipmentName)).Cast<equipmentName>().ToList();

    public enum itemType
    {
        none, headEquipment, bodyEquipment, handEquipment, strongHandEquipment, offHandEquipment, utilityEquipment
    }

    public enum equipmentQuality //ideas to add: lousy/2nd hand/broken (so we have one before basic
    {
        none, basic, advanced, supreme
    }
    public static string equipmentQualityToString(equipmentQuality x)
    {
        switch (x)
        {
            case equipmentQuality.none:
                return "none";
            case equipmentQuality.basic:
                return "Basic";
            case equipmentQuality.advanced:
                return "Advanced";
            case equipmentQuality.supreme:
                return "Supreme";
            default:
                Debug.LogError("Invalid equipment quality passed in for string: " + x);
                return "Blank";
        }
    }

    public enum itemHandRequirement
    {
        none, oneHanded, twoHanded
    }

    public static itemHandRequirement XMLintToHandRequirement(int i)
    {
        switch (i)
        {
            case 0:
                return itemHandRequirement.none;
            case 1:
                return itemHandRequirement.oneHanded;
            case 2:
                return itemHandRequirement.twoHanded;
            default:
                Debug.LogError("Invalid hand requirement int passed in for conversion: " + i);
                return itemHandRequirement.none;
        }
    }

    public enum magicEffect
    {
        none
    }

    public enum equipmentName
    {
        none,
        //helmets
        leatherCap, skullCap, chainHat, ironHelmet, wizardHat,
        //body equipment
        wizardRobes, leatherArmour, chainShirt, ironChestplate, platemail,
        //hand equipment
        leatherGloves, chainGloves, plateGloves, wizardGloves,
        //strong hand equipment
        quarterStaff, shortsword, longsword, handaxe, greataxe, greatsword, wand,
        //off hand equipment
        shield, spellbook,
        //utility equipment
        potion
    }
  
    //----------------------------------ABILITY REQUIREMENT-------------------------------------
    public enum abilityRequirement
    {
        none, light, medium, heavy, spellcaster, martial //can also add bard, rogue, religous etc
    }
 
    //-----------------------BASE COST---------------------------------------
    public static int equipmentBaseCost(equipmentItem equipmentItem)
    {
        int cost = equipmentItem.baseCost;
        
        switch (equipmentItem.equipmentQuality)
        {
            case equipmentQuality.none:
                cost = 0;
                break;
            case equipmentQuality.basic:
                cost = cost * basicQualityMultiplier;
                break;
            case equipmentQuality.advanced:
                cost = cost * advancedQualityMultiplier;
                break;
            case equipmentQuality.supreme:
                cost = cost * supremeQualityMultiplier;
                break;
        }
        return cost;
    }

    //-------------------------------- BASE COMBAT SCORE-------------------------------
    public static int equipmentBaseCombatScore(equipmentItem equipmentItem)
    {
        int score = equipmentItem.baseCombatScore;
        
        switch (equipmentItem.equipmentQuality)
        {
            case equipmentQuality.none:
                score = 0;
                break;
            case equipmentQuality.basic:
                score = score * basicQualityMultiplier;
                break;
            case equipmentQuality.advanced:
                score = score * advancedQualityMultiplier;
                break;
            case equipmentQuality.supreme:
                score = score * supremeQualityMultiplier;
                break;
        }
        return score;
    }
 
    public static List<equipmentName> getAllEquipmentNames()
    {
        List<equipmentName> outputList = allEquipmentNames;
        outputList.Remove(equipmentName.none);
        return outputList;
    }
}
