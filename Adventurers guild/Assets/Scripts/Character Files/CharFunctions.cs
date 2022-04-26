using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Holds all the static functions related to characters
public class CharFunctions : MonoBehaviour
{
    public GameObject traitHandlerObject, contractHandler;
    TraitHandler TraitHandler;
    ContractDetailsStorage ContractDetailsStorage;

    [Header("Race generation weights")]
    public int HumanWeight;
    public int ElfWeight;
    public int DwarfWeight;
    public int HalfingWeight;
    public int HalfOrcWeight;
    [Header("Class generation weights")]
    public int FighterWeight;
    public int WizardWeight;
    public int BardWeight;
    public int RangerWeight;
    public int PaladinWeight;
    public int RogueWeight;
    // Start is called before the first frame update
    void Start()
    {
        TraitHandler = traitHandlerObject.GetComponent<TraitHandler>();
        ContractDetailsStorage = contractHandler.GetComponent<ContractDetailsStorage>();

    }

    /// <summary>
    /// Generates characters XP, Class, race, price (without equipment). Also adds set name and inparty flag 
    /// </summary>
    /// <param name="generatedCharDetails"></param>
    /// <param name="charName"></param>
    /// <returns></returns>
    public CharacterDetails generateCharacter(CharacterDetails generatedCharDetails)
    {
        generatedCharDetails.XP = generateCharXpWithWeighting();
        generatedCharDetails.charClass = generateCharClassViaWeights();
        generatedCharDetails.race = generateCharRaceViaWeights();
        int level = HelperFunctions.getCharactersLevel(generatedCharDetails.XP);
        generatedCharDetails.cost = level * 10;
        generatedCharDetails.isRecruited = false;
        generatedCharDetails.inParty = 0;

        return generatedCharDetails;
    }

    //gets a characters strength for a given contract
    public int getCharsStrengthForContract(Character character, int contractID)
    {
        List<Trait> traitsList = TraitHandler.getTraitListFromTagList(character.traitReference.traitList);

        int thisCharsStrength = HelperFunctions.calculateCharacterStrength(character.characterDetails, traitsList,
                TraitHandler.getAttributesFromID(character.charId), ContractDetailsStorage.getContractDetailsFromID(contractID), character.characterEquipment);

        return thisCharsStrength;
    }

    public CharacterTypes.CharacterRace generateCharRaceViaWeights()
    {
        int totalWeight = HumanWeight + ElfWeight + DwarfWeight + HalfingWeight + HalfOrcWeight;

        int chanceOutcome = Random.Range(1, totalWeight);

        int elfChance = HumanWeight + ElfWeight;
        int dwarfChance = elfChance + DwarfWeight;
        int halfingChance = dwarfChance + HalfingWeight;
        int halfOrcChance = halfingChance + HalfingWeight;

        if (chanceOutcome > 0 && chanceOutcome <= HumanWeight)
        {
            return CharacterTypes.CharacterRace.Human;
        }
        else if(chanceOutcome > HumanWeight && chanceOutcome <= elfChance)
        {
            return CharacterTypes.CharacterRace.Elf;
        }
        else if(chanceOutcome > elfChance && chanceOutcome <= dwarfChance)
        {
            return CharacterTypes.CharacterRace.Dwarf;
        }
        else if (chanceOutcome > dwarfChance && chanceOutcome <= halfOrcChance)
        {
            return CharacterTypes.CharacterRace.HalfOrc;
        }
        else
        {
            Debug.Log("Error chanceOutcome when generating Race: " + chanceOutcome);
            return CharacterTypes.CharacterRace.Human;
        }
    }

    public CharacterTypes.CharacterClass generateCharClassViaWeights()
    {
        int totalWeight = FighterWeight + WizardWeight + BardWeight + RangerWeight + PaladinWeight + RogueWeight;

        int chanceOutcome = Random.Range(1, totalWeight);

        int wizardChance = FighterWeight + WizardWeight;
        int bardChance = wizardChance + BardWeight;
        int rangerChance = bardChance + RangerWeight;
        int paladinChance = rangerChance + HalfingWeight;
        int rogueChance = paladinChance + RogueWeight;

        if (chanceOutcome > 0 && chanceOutcome <= FighterWeight)
        {
            return CharacterTypes.CharacterClass.Fighter;
        }
        else if (chanceOutcome > FighterWeight && chanceOutcome <= wizardChance)
        {
            return CharacterTypes.CharacterClass.Wizard;
        }
        else if (chanceOutcome > wizardChance && chanceOutcome <= rangerChance)
        {
            return CharacterTypes.CharacterClass.Ranger;
        }
        else if (chanceOutcome > rangerChance && chanceOutcome <= paladinChance)
        {
            return CharacterTypes.CharacterClass.Paladin;
        }
        else if (chanceOutcome > paladinChance && chanceOutcome <= rogueChance)
        {
            return CharacterTypes.CharacterClass.Rogue;
        }
        else
        {
            Debug.Log("Error chanceOutcome when generating Race: " + chanceOutcome);
            return CharacterTypes.CharacterClass.Fighter;
        }
    }

    public int generateCharXpWithWeighting()
    {
        int chanceRoll = Random.Range(0, 100);

        if (chanceRoll <= 40) //Chance of getting level 1 char
        {
            return Random.Range(0, 100);
        }
        else if(chanceRoll <= 70) //Chance of rolling a level 2 char 
        {
            return Random.Range(100, 250);
        }
        else if (chanceRoll <= 90) //Chance of rolling a level 2 char 
        {
            return Random.Range(250, 475);
        }
        else if (chanceRoll <= 98) //Chance of rolling a level 2 char 
        {
            return Random.Range(475, 800);
        }
        else
        {
            return Random.Range(800, 1250);
        }
    }
}
