using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStorage : MonoBehaviour
{
    [HideInInspector]
    public List<CharacterDetails> characterDetailsList;

    public GameObject CharacterHandler, PartyHandler, NameHandlerObject, TraitHandlerObject, HealthHandlerObject, AllCharacterStorageObject, KeyWorldDetailsObject;
    CharFacade CharFacade;
    PartyStorage PartyStorage;
    NameHandler NameHandler;
    TraitHandler TraitHandler;
    HealthHandler HealthHandler;
    AllCharacterStorage AllCharacterStorage;
    keyWorldDetailsHandler keyWorldDetailsHandler;
    CharFunctions CharFunctions;

    // Start is called before the first frame update
    void Start()
    {
        CharFacade = CharacterHandler.GetComponent<CharFacade>();
        PartyStorage = PartyHandler.GetComponent<PartyStorage>();
        NameHandler = NameHandlerObject.GetComponent<NameHandler>();
        TraitHandler = TraitHandlerObject.GetComponent<TraitHandler>();
        HealthHandler = HealthHandlerObject.GetComponent<HealthHandler>();
        AllCharacterStorage = AllCharacterStorageObject.GetComponent<AllCharacterStorage>();
        keyWorldDetailsHandler = KeyWorldDetailsObject.GetComponent<keyWorldDetailsHandler>();
        CharFunctions = CharacterHandler.GetComponent<CharFunctions>();
    }

    public CharacterDetails generateRandomCharacterDetails(int charId)
    {
        CharacterDetails generatedChar = gameObject.AddComponent<CharacterDetails>();
        generatedChar.ID = charId;

        generatedChar = CharFunctions.generateCharacter(generatedChar);
        characterDetailsList.Add(generatedChar);

        return generatedChar;
    }

    //recieves a character which it then creates
    public void buildCharacterVisual(CharacterDetails character)
    {
        if (character.isRecruited == true)
        {
            CharFacade.displayEmployee(character.ID);
        }
        else
        {
            CharFacade.displayRecuitable(character);
        }
    }

    //finds a character in the character list according to ID
    public CharacterDetails findCharacterFromID(int ID)
    {
        foreach (CharacterDetails c in characterDetailsList)
        {
            if (c.ID == ID)
            {
                return c;
            }
        }
        Debug.LogError("No Character found with ID: " + ID);
        return null;
    }

    //clears held data. Used when loading saved game
    public void ClearAllData()
    {
        characterDetailsList.Clear();
        CharFacade.ClearAllData();
    }

    /// <summary>
    /// recieves all details about a character which is then built. Used when loading game
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="XP"></param>
    /// <param name="cClass"></param>
    /// <param name="race"></param>
    /// <param name="inParty"></param>
    /// <param name="isRecruited"></param>
    /// <param name="cost"></param>
    public void loadCharacterDetails(int ID, int XP, int cClass, int race, int inParty, int isRecruited, int cost)
    {
        CharacterDetails newCharacter = gameObject.AddComponent<CharacterDetails>();
        newCharacter.ID = ID;
        newCharacter.XP = XP;
        newCharacter.charClass = (CharacterTypes.CharacterClass)cClass;
        newCharacter.race = (CharacterTypes.CharacterRace)race;
        newCharacter.inParty = inParty;
        newCharacter.cost = cost;

        if (isRecruited == 0)
        {
            newCharacter.isRecruited = false;
        }
        else if (isRecruited == 1)
        {
            newCharacter.isRecruited = true;
        }
        else
        {
            Debug.LogError("Incorrect value entered for isRecruited: " + isRecruited);
        }

        characterDetailsList.Add(newCharacter);

        buildCharacterVisual(newCharacter);
    }

    //give each character in the list a set amount of xp each
    public void awardXpToCharacters(List<Character> characters, int XpToAward)
    {      
        foreach (Character c in characters)
        {
            awardXpToSingleCharacter(c, XpToAward);
        }
    }
    
    void awardXpToSingleCharacter(Character c, int XpToAward)
    {
        //check if character leveled up
        bool didCharLevelUp = HelperFunctions.didCharLevelUp(c.characterDetails.XP, c.characterDetails.XP + XpToAward);
        //change the characters xp
        c.characterDetails.XP = c.characterDetails.XP + XpToAward;
        //if character did level up, update its health
        if (didCharLevelUp)
        {
            levelCharacterUp(c);
        }

        //update the visuals for the character
        CharFacade.updateCharVisuals(c.charId);
    }

    void levelCharacterUp(Character c)
    {
        Debug.Log("Character Leveled Up! ID: " + c.charId + " (" + c.characterDetails.charName + ").");
        HealthHandler.levelUpHealth(c.charId);

    }

    public void recruitCharacter(int Id)
    {
        //set character as recruited
        AllCharacterStorage.findAliveCharacterFromID(Id).characterDetails.isRecruited = true;
        //add newly recruited moodlet to char
        AllCharacterStorage.addMoodletToChar(Id, MoodTypes.moodletTag.newlyRecruited);
        //update everyones goodness difference opinionlets to account for new recruit
        AllCharacterStorage.updateAllGoodnessDifferenceOpinionlets();

        keyWorldDetailsHandler.changeTotalGold(-AllCharacterStorage.findAliveCharacterFromID(Id).characterDetails.cost);


        CharFacade.displayEmployee(Id);
    }


    //get list of all other chars recruited
    //for each char, find goodness difference
    //set opinion, remove existing opinion
}
