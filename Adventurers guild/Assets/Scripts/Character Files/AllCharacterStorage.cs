using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllCharacterStorage : MonoBehaviour
{
    public List<Character> allAliveCharactersList, allDeadCharacterList;

    public GameObject characterHandler, NameHandlerObject, TraitHandlerObject, HealthHandlerObject, MoodHandler, RelationshipsHandler, EventHandlerObject, PartyStorageObject, equipmentHandlerObject;

    NameHandler NameHandler;
    TraitHandler TraitHandler;
    HealthHandler HealthHandler;
    CharStorage CharStorage;
    CharMoodStorage CharMoodStorage;
    CharRelationshipStorage CharRelationshipStorage;
    EventHandler EventHandler;
    PartyStorage PartyStorage;
    CharFunctions CharFunctions;
    EquipmentHandler EquipmentHandler;

    public Button generateRecruitsButton;

    bool traitReferenceSet, attributeSet, nameReferenceSet, healthSet, moodlSet, relationshipSet, equipmentSet;

    //character ID always starts at 0 and is used to identify characters
    int charID = 0;


    // Start is called before the first frame update
    void Start()
    {
        CharStorage = characterHandler.GetComponent<CharStorage>();
        NameHandler = NameHandlerObject.GetComponent<NameHandler>();
        TraitHandler = TraitHandlerObject.GetComponent<TraitHandler>();
        HealthHandler = HealthHandlerObject.GetComponent<HealthHandler>();
        CharMoodStorage = MoodHandler.GetComponent<CharMoodStorage>();
        CharRelationshipStorage = RelationshipsHandler.GetComponent<CharRelationshipStorage>();
        EventHandler = EventHandlerObject.GetComponent<EventHandler>();
        PartyStorage = PartyStorageObject.GetComponent<PartyStorage>();
        CharFunctions = characterHandler.GetComponent<CharFunctions>();
        EquipmentHandler = equipmentHandlerObject.GetComponent<EquipmentHandler>();

        Button genRecrtsBtn = generateRecruitsButton.GetComponent<Button>();
        genRecrtsBtn.onClick.AddListener(generateCharacters);

        setBuildBoolsFalse();
    }

    /// <summary>
    /// randomly creates a character according to current biases
    /// </summary>
    void generateCharacters()
    {
        charID++;

        //generate base details
        CharacterDetails generatedChar = CharStorage.generateRandomCharacterDetails(charID);

        //set name
        NameReference nameReference = NameHandler.generateRandomName(generatedChar);
        string charName = NameHandler.createNameString(nameReference);
        generatedChar.charName = charName;

        //set traits. these are stored elsewhere 
        CharacterAttributes characterAttributes = TraitHandler.getRandomAttributes(charID);
        TraitReference traitReference = TraitHandler.getRandomTraits(charID);
        CharacterHealth characterHealth = HealthHandler.generateCharHealth(HelperFunctions.getCharactersLevel(generatedChar.XP), generatedChar.charClass, charID);
        CharacterMood characterMood = CharMoodStorage.generateBlankMood(charID);
        CharacterRelationships characterRelationships = CharRelationshipStorage.generateBlankRelationships(charID);
        CharacterEquipment characterEquipment = EquipmentHandler.createRandomStartingEquipment
            (charID, HelperFunctions.getCharactersLevel(generatedChar.XP), generatedChar.charClass);
                
        //Add the cost of the equipment to character cost
        generatedChar.cost = generatedChar.cost + EquipmentHandler.calculateTotalEquipmentCost(characterEquipment);

        CharStorage.buildCharacterVisual(generatedChar);
        buildCharacter(generatedChar, nameReference, characterAttributes, traitReference, characterHealth,
            characterMood, characterRelationships, characterEquipment);
    }

    //used when creating character. will build a character
    public void buildCharacter(CharacterDetails characterDetails, NameReference nameReference,
        CharacterAttributes characterAttributes, TraitReference traitReference, CharacterHealth characterHealth,
        CharacterMood characterMood, CharacterRelationships characterRelationships, CharacterEquipment characterEquipment)
    {
        Character character = gameObject.AddComponent<Character>();
        character.charId = characterDetails.ID;
        character.characterDetails = characterDetails;
        character.nameReference = nameReference;
        character.characterAttributes = characterAttributes;
        character.traitReference = traitReference;
        character.characterHealth = characterHealth;
        character.characterMood = characterMood;
        character.characterRelationships = characterRelationships;
        character.characterEquipment = characterEquipment;

        if (character.characterDetails.inParty != 0) 
        {
            PartyStorage.addCharToExistingParty(character);
        }

        allAliveCharactersList.Add(character);
    }

    //finds a character in the character list according to ID
    public Character findAliveCharacterFromID(int ID)
    {
        foreach (Character c in allAliveCharactersList)
        {
            if (c.charId == ID)
            {
                return c;
            }
        }
        Debug.LogError("No Character found with ID: " + ID);
        return null;
    }

    public List<int> findAllRecruitedCharsIds()
    {
        List<int> IdList = new List<int>();
        foreach (Character c in allAliveCharactersList)
        {
            if (c.characterDetails.isRecruited)
            {
                IdList.Add(c.charId);
            }
        }
        return IdList;
    }

    public int GetRandomCharIdNotOnQuestOrInSameParty(int ownCharId, int partyId)
    {
        List<int> IdList = new List<int>();

        //loops through each character in alive char list to add each character that is not on contract or in same party to id list
        foreach (Character c in allAliveCharactersList)
        {
            if (ownCharId != c.characterDetails.ID)
            {
                //if character is not in same party or character is not in party
                if (c.characterDetails.inParty != partyId || c.characterDetails.inParty == 0)
                {
                    //if char is not in party it can be added straight to list as cant be on contract
                    if (c.characterDetails.inParty == 0)
                    {
                        IdList.Add(c.charId);
                    }
                    else //char is in party
                    {
                        //if party isnt on contract
                        if (PartyStorage.findPartyFromID(c.characterDetails.inParty).onQuest == 0)
                        {
                            IdList.Add(c.charId);
                        }
                    }
                }
            }
        }
        //return a char id randomly selected from list
        return HelperFunctions.getRandomCharIdFromIdList(IdList);
    }


    /// HEALING ///
    public void damageCharacter(int charId, int damage)
    {
        HealthHandler.damageCharacter(charId, damage);
    }
    //heals the character by the amount specified
    public void healCharacter(int charId, int healing)
    {
        HealthHandler.healCharacter(charId, healing);
    }
    public void dailyHealRecovery()
    {
        HealthHandler.dailyHealRecovery();
    }

    ///MOOD///
    public void addMoodletToChar(int charId, MoodTypes.moodletTag moodletsEnum)
    {
        CharMoodStorage.addMoodletToChar(charId, moodletsEnum);
    }
    public void removeMoodletFromChar(int charId, MoodTypes.moodletTag moodletsEnum)
    {
        CharMoodStorage.removeMoodletFromChar(charId, moodletsEnum);
    }
    public void tickAllMoodletsDown()
    {
        CharMoodStorage.tickAllMoodletsDown();
    }
    public int getCharsTotalMood(int charId)
    {
        return CharMoodStorage.getCharsTotalMood(charId);
    }
    public void setEachPartyCharMoodFromContractGoodness(List<Character> partyChars, int contractGoodness, bool isCompleted)
    {
        foreach(Character partyChar in partyChars)
        {
            CharMoodStorage.setGoodnessDifferenceOpinionlet(partyChar.charId, contractGoodness, isCompleted);
        }
    }
    public void updateAllCharMoodFromPartyMembers()
    {
        CharMoodStorage.updateAllCharMoodFromPartyMembers();
    }

    //OPINION//
    
    /// <summary>
    /// Add a opinionlet about a character to a character
    /// </summary>
    /// <param name="charId"></param>
    /// <param name="targetCharId"></param>
    /// <param name="opinionletEnum"></param>
    public void addOpinionletToChar(int charId, int targetCharId, OpinionTypes.opinionletTag opinionletEnum)
    {
        CharRelationshipStorage.addOpinionletToChar(charId, targetCharId, opinionletEnum);
    }
    /// <summary>
    /// Removes a opinionlet about another character from a character
    /// </summary>
    /// <param name="charId"></param>
    /// <param name="targetCharId"></param>
    /// <param name="opinionletEnum"></param>
    public void removeOpinionletFromChar(int charId, int targetCharId, OpinionTypes.opinionletTag opinionletEnum)
    {
        CharRelationshipStorage.removeOpinionletFromChar(charId, targetCharId, opinionletEnum);
    }
    public void tickAllOpinionletsDown()
    {
        CharRelationshipStorage.tickAllOpinionletsDown();
    }
    public List<int> getIdsOfCharsCharacterHasOpinionletFor(int charId)
    {
        return CharRelationshipStorage.getIdsOfCharsCharacterHasOpinionletFor(charId);
    }
    public int getCharsCumulativeOpinionOfTargetChar(int charId, int targetId)
    {
        return CharRelationshipStorage.getCharsCumulativeOpinionOfTargetChar(charId, targetId);
    }
    //set 1 chars goodness difference moodlet
    public void setGoodnessDifferenceOpinionletForAllOtherChars(int charId)
    {
        List<int> AllOtherRecruitedChars = new List<int>(findAllRecruitedCharsIds());
        AllOtherRecruitedChars.Remove(charId);

        foreach(int targetCharId in AllOtherRecruitedChars)
        {
            CharRelationshipStorage.setGoodnessDifferenceOpinionlet(charId, targetCharId);
        }
    }
    //as peoples goodness will change, peoples opinions of each other will change because of this, therefore need to update opinionlets
    public void updateAllGoodnessDifferenceOpinionlets()
    {
        List<int> allRecruitedChars = new List<int>(findAllRecruitedCharsIds());
        foreach(int charId in allRecruitedChars)
        {
            setGoodnessDifferenceOpinionletForAllOtherChars(charId);
        }
    }

    /// EVENTS///
    public void doEachCharsDailySocialEvent()
    {
        foreach(Character character in allAliveCharactersList)
        {
            //do a coin flip to see if a character has a social event happen
            if (HelperFunctions.doWeightedCoinToss(50))
            {
                EventHandler.doRandomSocialEventForChar(character.charId);
            }
        }
    }

    //resets ID counter to 1 above the highest ID recorded 
    public void setCounters()
    {
        charID = 0;
        if (allAliveCharactersList.Count != 0)
        {
            charID = allAliveCharactersList[0].charId;
            foreach (Character c in allAliveCharactersList)
            {
                if (c.charId > charID)
                {
                    charID = c.charId;
                }
            }
        }
        if (allDeadCharacterList.Count != 0)
        {
            charID = allDeadCharacterList[0].charId;
            foreach (Character c in allDeadCharacterList)
            {
                if (c.charId > charID)
                {
                    charID = c.charId;
                }
            }
        }
        charID++;
    }

    //used when loading. will rebuild all characters using data stored
    public void rebuildAllCharacters()
    {
        //build each character
        foreach(CharacterDetails characterDetails in CharStorage.characterDetailsList)
        {
            Character character = gameObject.AddComponent<Character>();
            character.charId = characterDetails.ID;
            character.characterDetails = characterDetails;

            foreach(TraitReference traitReference in TraitHandler.charTraitList)
            {
                if(traitReference.ID == character.charId)
                {
                    character.traitReference = traitReference;
                    traitReferenceSet = true;
                    break;
                }
            }
            foreach (CharacterAttributes characterAttributes in TraitHandler.charAttributesList)
            {
                if (characterAttributes.charID == character.charId)
                {
                    character.characterAttributes = characterAttributes;
                    attributeSet = true;
                    break;
                }
            }
            foreach (NameReference nameReference in NameHandler.nameList)
            {
                if (nameReference.ID == character.charId)
                {
                    character.nameReference = nameReference;
                    nameReferenceSet = true;
                    break;
                }
            }
            foreach (CharacterHealth characterHealth in HealthHandler.AllCharactersHealths)
            {
                if (characterHealth.ID == character.charId)
                {
                    character.characterHealth = characterHealth;
                    healthSet = true;
                    break;
                }
            }
            foreach(CharacterMood characterMood in CharMoodStorage.allCharacterMoodsList)
            {
                if(characterMood.Id == character.charId)
                {
                    character.characterMood = characterMood;
                    moodlSet = true;
                    break;//may need to add case to create a blank one in the case that char has neutral mood
                }
            }
            foreach(CharacterRelationships characterRelationships in CharRelationshipStorage.allCharacterRelationshipsList)
            {
                if(characterRelationships.Id == character.charId)
                {
                    character.characterRelationships = characterRelationships;
                    relationshipSet = true;
                    break; //may need to add case to create a blank one in the case that char has no relationships

                }
            }
            foreach(CharacterEquipment characterEquipment in EquipmentHandler.getAllCharEquipmentList())
            {
                if(characterEquipment.charId == character.charId)
                {
                    character.characterEquipment = characterEquipment;
                    equipmentSet = true;
                }
            }
            if (traitReferenceSet && attributeSet && nameReferenceSet && healthSet && moodlSet && relationshipSet && equipmentSet)
            {
                Debug.Log("Character ID: " + character.charId + "built correctly");
            }
            else
            {
                Debug.LogError("Character ID: " + character.charId + "built unsuccessfully. Sets built: traitReferenceSet = "
                    + traitReferenceSet + ", attributeSet = " + attributeSet +", nameReferenceSet = " + nameReferenceSet + 
                    ", healthSet = " + healthSet + ", MoodSet = " + moodlSet + ", RelationshipSet = " + relationshipSet
                    + ", EquipmentSet = " + equipmentSet);
            }
            setBuildBoolsFalse();

            if(character.characterHealth.currentHealth <= 0)
            {
                allDeadCharacterList.Add(character);
            }
            else
            {
                allAliveCharactersList.Add(character);
            }

            if(character.characterDetails.inParty != 0)
            {
                PartyStorage.addCharToExistingParty(character);
            }
        }

    }

    void setBuildBoolsFalse()
    {
        traitReferenceSet = false;
        attributeSet = false;
        nameReferenceSet = false;
        healthSet = false;
        moodlSet = false;
        relationshipSet = false;
        equipmentSet = false;
    }
}
