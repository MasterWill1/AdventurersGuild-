using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HelperFunctions
{
    //public GameObject genCaller = GameObject.FindGameObjectWithTag("Generator");
    //public GameObject conCaller = GameObject.FindGameObjectWithTag("ConHandler");
    //Generator genScriptCaller;
    //ContractHandler conScriptCaller;

    //each tait adds a percantage modifer to char strength. this is according to string list in traithandler. so (1,0) = brave which is +5% modifier on strength
    public static int[,] allTraitsStrengthModiferArray = new int[,] { { 0, 0}, { 5, -5 },{ 3, -3 },{ 5, -5 },{ 2, -2 },
        {5,-5 },{3,-3 },{1,-1 }  };

    public static string CharRaceEnumToString(CharacterTypes.CharacterRace Race)
    {
        switch (Race)
        {
            case CharacterTypes.CharacterRace.Human:
                return "Human";
            case CharacterTypes.CharacterRace.Elf:
                return "Elf";
            case CharacterTypes.CharacterRace.Dwarf:
                return "Dwarf";
            case CharacterTypes.CharacterRace.Halfling:
                return "Halfing";
            case CharacterTypes.CharacterRace.HalfOrc:
                return "Half-Orc";
        }
        return "error: " + Race.ToString();
    }

    public static string CharClassEnumToString(CharacterTypes.CharacterClass Class)
    {
        switch (Class)
        {
            case CharacterTypes.CharacterClass.Fighter:
                return "Fighter";
            case CharacterTypes.CharacterClass.Wizard:
                return "Wizard";
            case CharacterTypes.CharacterClass.Bard:
                return "Bard";
            case CharacterTypes.CharacterClass.Ranger:
                return "Ranger";
            case CharacterTypes.CharacterClass.Paladin:
                return "Paladin";
            case CharacterTypes.CharacterClass.Rogue:
                return "Rogue";
        }
        return "error: " + Class.ToString();
    }

    public static int getCharactersLevel(int xp)
    {
        if (0 <= xp && xp < 100)
        {
            return 1;
        }
        if (100 <= xp && xp < 250)
        {
            return 2;
        }
        if (250 <= xp && xp < 475)
        {
            return 3;
        }
        if (475 <= xp && xp < 800)
        {
            return 4;
        }
        if (800 <= xp && xp < 1250)
        {
            return 5;
        }
        if (1250 <= xp && xp < 1925)
        {
            return 6;
        }
        if (1925 <= xp && xp < 2900)
        {
            return 7;
        }
        if (2900 <= xp)
        {
            Debug.Log("Max level hit, xp= " + xp);
            return 8;
        }
        Debug.LogError("cant get level. xp = " + xp);
        return 0;
    }

    public static int xpTillLevelUp(int xp)
    {
        if (0 <= xp && xp < 100)
        {
            return (100-xp);
        }
        if (100 <= xp && xp < 250)
        {
            return (250-xp);
        }
        if (250 <= xp && xp < 475)
        {
            return (475-xp);
        }
        if (475 <= xp && xp < 800)
        {
            return (800-xp);
        }
        if (800 <= xp && xp < 1250)
        {
            return (1250-xp);
        }
        if (1250 <= xp && xp < 1925)
        {
            return (1925-xp);
        }
        if (1925 <= xp && xp < 2900)
        {
            return (2900-xp);
        }
        if (2900 <= xp)
        {
            Debug.Log("Max level hit, xp= " + xp);
            return 000;
        }
        Debug.LogError("cant get level. xp = " + xp);
        return 0;
    }

    public static bool didCharLevelUp(int oldXP, int newXP)
    {
        int oldLevel = getCharactersLevel(oldXP);
        int newLevel = getCharactersLevel(newXP);

        if(oldLevel == newLevel)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static int BoolToInt(bool boolFlag)
    {
        if (boolFlag == true)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public static bool IntToBool(int i)
    {
        if (i == 1)
        {
            return true;
        }
        else if (i == 0) 
        {
            return false;
        }
        else
        {
            Debug.LogError("invalid int to bool: i = " + i);
            return false;
        }
    }

    public static string intToGender(int g)
    {
        if (g == 0)
        {
            return "Female";
        }else if (g == 1)
        {
            return "Male";
        }
        else
        {
            Debug.LogError("invalid int passed for gender: " + g);
            return null;
        }
    }

    public static string goodnessIntToString(int g)
    {
        if (g >= 1 && g <= 2)
        {
            return "Evil";
        }else if (g >=3 && g<=4)
        {
            return "Minor Evil";
        }
        else if (g >= 5 && g <= 6)
        {
            return "Neutral";
        }
        else if (7 >= 3 && g <= 8)
        {
            return "Minor Good";
        }
        else if (g >= 9 && g <= 10)
        {
            return "Good";
        }
        Debug.LogError("Incorrect goodness int passed: " + g);
        return null;
    }

    public static string lawfulnessIntToString(int g)
    {
        if (g >= 1 && g <= 2)
        {
            return "Chaotic";
        }
        else if (g >= 3 && g <= 4)
        {
            return "Minor Chaotic";
        }
        else if (g >= 5 && g <= 6)
        {
            return "Neutral";
        }
        else if (7 >= 3 && g <= 8)
        {
            return "Minor Lawful";
        }
        else if (g >= 9 && g <= 10)
        {
            return "Lawful";
        }
        Debug.LogError("Incorrect lawfulness int passed: " + g);
        return null;
    }

    public static int calculateCharacterStrength(CharacterDetails character, List<Trait> traits, CharacterAttributes attributes,
        ContractDetails contractDetails, CharacterEquipment characterEquipment)
    {
        int level = getCharactersLevel(character.XP);
        int strength = getClassStrength(character.charClass) * level;

        strength = strength + getBaseCombatScoreFromAllCharEquipment(characterEquipment);

        double modifier = (getModifierFromTraits(traits, character.charClass) + getModifierFromContractTraits(attributes, contractDetails, character)) * 0.01 + 1;

        strength = (int)(strength * modifier);

        return strength;
    }

    static int getClassStrength(CharacterTypes.CharacterClass classID)
    {
        switch (classID)
        {
            //fighter
            case CharacterTypes.CharacterClass.Fighter:
                return 10;
            //wizard
            case CharacterTypes.CharacterClass.Wizard:
                return 11;
            //bard
            case CharacterTypes.CharacterClass.Bard:
                return 7;
            //ranger
            case CharacterTypes.CharacterClass.Ranger:
                return 8;
            //paladin
            case CharacterTypes.CharacterClass.Paladin:
                return 9;
            //rogue     
            case CharacterTypes.CharacterClass.Rogue:
                return 8;
        }
        Debug.LogError("Invalid class ID passed: " + classID);
        return 0;
    }

     static int getModifierFromTraits(List<Trait> traitList, CharacterTypes.CharacterClass characterClass)
    {
        int modifier = 0;
        foreach(Trait trait in traitList)
        {
            if(trait.traitTag == 0)
            {
                break;
            }
            
            switch (characterClass)
            {
                case CharacterTypes.CharacterClass.Fighter:
                    modifier = modifier + trait.TraitClassEffects.fighterCombatScoreEffect;
                    break;
                case CharacterTypes.CharacterClass.Bard:
                    modifier = modifier + trait.TraitClassEffects.bardCombatScoreEffect;
                    break;
                case CharacterTypes.CharacterClass.Rogue:
                    modifier = modifier + trait.TraitClassEffects.rogueCombatScoreEffect;
                    break;
                case CharacterTypes.CharacterClass.Wizard:
                    modifier = modifier + trait.TraitClassEffects.wizardCombatScoreEffect;
                    break;
                case CharacterTypes.CharacterClass.Ranger:
                    modifier = modifier + trait.TraitClassEffects.rangerCombatScoreEffect;
                    break;
                case CharacterTypes.CharacterClass.Paladin:
                    modifier = modifier + trait.TraitClassEffects.paladinCombatScoreEffect;
                    break;
                default:
                    Debug.LogError("Invalid character class passed in for trait combat score calculation: " + characterClass);
                    break;
            }
            
        }

        return modifier;
    }

    static int getModifierFromContractTraits(CharacterAttributes characterAttributes, ContractDetails contractDetails, CharacterDetails character)
    {
        int modifier = 0;

        //if contract isnt being passed (calculation are done from character details screen
        if(contractDetails == null)
        {
            return 0;
        }

        bool isContractRural = !(contractDetails.locationSpecificTag == LocationTypes.locationSpecificTag.town
            || contractDetails.locationSpecificTag == LocationTypes.locationSpecificTag.city);

        //if character is ranger and contract is in rural environment, give modifier
        if (character.charClass == CharacterTypes.CharacterClass.Ranger && isContractRural)
        {
            modifier = modifier + 10;
        }
        //if character is rogue and contract is in urban environement, give modifier
        if(character.charClass == CharacterTypes.CharacterClass.Rogue && isContractRural)
        {
            modifier = modifier + 10;
        }
        //if character is bard and contract is in urban environement, give modifier
        if (character.charClass == CharacterTypes.CharacterClass.Bard && isContractRural)
        {
            modifier = modifier + 5;
        }

        //get the difference in goodness scores then add 5. therefore equal goodnesses will get bonus but unequal will still get negative;
        int goodnessDifference = calculateGoodnessDifference(characterAttributes.goodness, contractDetails.goodness);
        goodnessDifference = goodnessDifference + 5;

        modifier = modifier + goodnessDifference;

        //if contract location matches character native biome, give modifier
        if(characterAttributes.nativeBiome == contractDetails.locationBiomeTag)
        {
            modifier = modifier + 5;
        }
        return modifier;
    }

    public static int calculateGoodnessDifference(int goodnessScore, int otherGoodnessScore)
    {
        //get the difference in goodness scores then add 5. therefore equal goodnesses will get bonus but unequal will still get negative;
        int goodnessDifference = -(int)Mathf.Sqrt(Mathf.Pow((goodnessScore - otherGoodnessScore), 2));
        return goodnessDifference;
    }

    public static int partyStrengthAfterModifiers(Party party, int strength)
    {
        int modifier = 0;

        modifier = modifier + modifierFromPartyBards(party);
        modifier = modifier + partySizeStrengthReduction(party);
        modifier = modifier + modifierFromVariedParty(party);

        double modiferPercentage = modifier * 0.01 + 1;
        Debug.Log("Total final party modifier: " + modifier + "%. Modifier to be multiplied: " + modiferPercentage);

        int finalPartyStrength = (int)(strength * modiferPercentage);

        return finalPartyStrength;
    }

    static int modifierFromPartyBards(Party party)
    {
        int modifier = 0;
        foreach(Character c in party.members)
        {
            if(c.characterDetails.charClass == CharacterTypes.CharacterClass.Bard)
            {
                modifier = modifier + 10;
            }
        }
        Debug.Log("Modifier from party bards: " + modifier + "%");
        return modifier;
    }

    static int partySizeStrengthReduction(Party party)
    {
        int partySize = party.members.Count;

        int overOptimalSize = 3 - partySize;

        int modifier = 0;
        if (overOptimalSize < 0)
        {
            modifier = 10 * overOptimalSize;
        }

        Debug.Log("Modifier from party size: " + modifier + "%");
        return modifier;
    }

    //needs rework
    static int modifierFromVariedParty(Party party)
    {
        List<CharacterTypes.CharacterClass> partyClassList = new List<CharacterTypes.CharacterClass>();

        foreach(Character c in party.members)
        {
            partyClassList.Add(c.characterDetails.charClass);
        }

        int modifier = 0;

        foreach(Character c in party.members)
        {
            CharacterTypes.CharacterClass thisCharsClass = c.characterDetails.charClass;

            partyClassList.Remove(thisCharsClass);

            if (!partyClassList.Contains(thisCharsClass))
            {
                modifier = modifier + 5;
            }

            partyClassList.Add(thisCharsClass);
        }
        Debug.Log("Modifier from varied party: " + modifier + "%");
        return modifier;
    }

    public static int wageFromWorth(int value)
    {
        int wage = (int)Mathf.Ceil((float)(value * 0.1));
        return wage;
    }

    public static string whatDayIsIt(int day)
    {
        switch (day)
        {
            case 7:
                return "Monday";
            case 6:
                return "Tuesday";
            case 5:
                return "Wednesday";
            case 4:
                return "Thursday";
            case 3:
                return "Friday";
            case 2:
                return "Saturday";
            case 1:
                return "Sunday";
        }
        Debug.LogError("Invalid day passed: " + day);
        return "error";
    }

    public static int goodnessScoreToModifier(int goodness)
    {
        int modifier = goodness - 5;
        return modifier;
    }

    //takes the double digit goodness score from KWD and returns a rating
    public static string goodnessRatingFromBigInt(int goodness)
    {
        int goodnessSimplied = (int)(goodness * 0.1);

        return goodnessIntToString(goodnessSimplied);
    }

    public static string partyMembersListToString(Party thisParty)
    {
        //create string of all members in party
        int index = 0;
        string collatedNames = "Empty";
        if (thisParty.members.Count != 0)
        {
            collatedNames = thisParty.members[0].characterDetails.charName;
            foreach (Character n in thisParty.members)
            {
                index++;
                if (index > 1)
                {
                    string name = n.characterDetails.charName;
                    collatedNames = collatedNames + ", " + name;
                }
            }
        }

        return collatedNames;
    }

    public static string ListOfIntToString(string OpeningString, List<int> list)
    {
        foreach (int item in list)
        {
            OpeningString += item.ToString() + ", ";
        }
        return OpeningString;
    }

    /// <summary>
    /// receives a chance between 0 and 100 and returns a bool depending on the coin toss. 50 is 50% chance of success. 30 is 30% chance of success
    /// </summary>
    /// <param name="chance"></param>
    /// <returns></returns>
    public static bool doWeightedCoinToss(int chance)
    {
        if(chance>100 || chance < 0)
        {
            Debug.LogError("Can't have a chance that is higher than 100 or lower than 0");
        }

        int chanceScore = UnityEngine.Random.Range(1, 101);

        if(chanceScore> chance)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Returns a random chars ID from ID list. Be sure to remove any IDs that you dont want returned before calling
    /// </summary>
    /// <param name="charIdList"></param>
    /// <returns></returns>
    public static int getRandomCharIdFromIdList(List<int> charIdList)
    {
        if (charIdList.Count == 0)
        {
            return 0;
        }
        //select a random character by their index and return their id
        int charIdIndex = UnityEngine.Random.Range(0, charIdList.Count);
        return charIdList[charIdIndex];
    }

    public static int getBaseCombatScoreFromAllCharEquipment(CharacterEquipment characterEquipment)
    {
        int combatScore = 0;

        combatScore = CharacterEquipmentTypes.equipmentBaseCombatScore
            (characterEquipment.headEquipmentItem);
        combatScore = combatScore + CharacterEquipmentTypes.equipmentBaseCombatScore
            (characterEquipment.bodyEquipmentItem);
        combatScore = combatScore + CharacterEquipmentTypes.equipmentBaseCombatScore
            (characterEquipment.handEquipmentItem);
        combatScore = combatScore + CharacterEquipmentTypes.equipmentBaseCombatScore
            (characterEquipment.strongHandItem);
        combatScore = combatScore + CharacterEquipmentTypes.equipmentBaseCombatScore
            (characterEquipment.offHandItem);
        combatScore = combatScore + CharacterEquipmentTypes.equipmentBaseCombatScore
            (characterEquipment.utilityEquipmentItem);

        return combatScore;
    }

    public static bool isXMLStringTrueOrFalse(string text)
    {
        if(text == "Y")
        {
            return true;
        }
        else if(text == "N")
        {
            return false;
        }
        else
        {
            Debug.LogError("incorrect string passed in for true false conversion. Valid values are Y or N. Value passed in: " + text);
            return false;
        }
    }
}
