using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyStorage : MonoBehaviour
{
    [HideInInspector]
    public List<Party> allPartiesList;
    [HideInInspector]
    public bool isEditing = false;
    [HideInInspector]
    public List<Character> newParty;
    int partyIdCounter = 0;
    [HideInInspector]
    public int editingPartyID;

    public GameObject CharacterHandler, PartyHandler, NameHandlerGO, allCharacterHandler;
    PartyFacade PartyFacade;
    NameHandler NameHandler;
    CharFacade CharFacade;
    CharStorage CharStorage;
    AllCharacterStorage AllCharacterStorage;

    public Button closePartyScreen;
    public Button confirmNewPartyButton;

    // Start is called before the first frame update
    void Start()
    {
        PartyFacade = PartyHandler.GetComponent<PartyFacade>();
        NameHandler = NameHandlerGO.GetComponent<NameHandler>();
        CharFacade = CharacterHandler.GetComponent<CharFacade>();
        CharStorage = CharacterHandler.GetComponent<CharStorage>();
        AllCharacterStorage = allCharacterHandler.GetComponent<AllCharacterStorage>();

        Button closeNewPartyBtn = closePartyScreen.GetComponent<Button>();
        closeNewPartyBtn.onClick.AddListener(delegate { closeNewPartyScreen(true); });

        Button confirmNewPartBtn = confirmNewPartyButton.GetComponent<Button>();
        confirmNewPartBtn.onClick.AddListener(confirmNewParty);
    }

    //Finds a party from its ID
    public Party findPartyFromID(int ID)
    {
        foreach (Party p in allPartiesList)
        {
            if (p.partyID == ID)
            {
                return p;
            }
        }
        Debug.LogError("No party found with ID: " + ID);
        return null;
    }

    //Finds a party from the contract ID it is on
    public Party findPartyFromContractID(int ID)
    {
        foreach (Party p in allPartiesList)
        {
            if (p.onQuest == ID)
            {
                return p;
            }
        }
        Debug.LogError("No party found with on contract ID: " + ID);
        return null;
    }

    
    //removes character from new party, called from NewPartyChar
    public void removeCharFromNewParty(int characterID)
    {
        foreach (Character c in newParty)
        {
            if (c.charId == characterID)
            {
                newParty.Remove(c);
                break;
            }
        }
    }

    //counts number of Ids and returns a random number. Ie if party size was 5, it would return 1 < x < 5
    public int getRandomCountOfChars(int partyId)
    {
        int partySize = findPartyFromID(partyId).members.Count;
        int numberOfChars;
        if (partySize == 1)
        {
            numberOfChars = 1;
        }
        else
        {
            numberOfChars = Random.Range(2, partySize);
        }
        return numberOfChars;
    }

    //gets a random character from a party that is not the character passed in 
    public int getRandomOtherCharacterFromParty(int charId, int partyId)
    {
        List<int> charIdList = getAllCharIdsInParty(partyId);
        //remove the char id we dont want in the list
        charIdList.Remove(charId);

        return HelperFunctions.getRandomCharIdFromIdList(charIdList);

    }

    //gets a random selection of char Ids from a party equal to the number of characters specified
    public List<int> getNumberOfRandomCharactersIdFromParty(int partyID, int numberOfCharacters)
    {
        Party party = findPartyFromID(partyID);
        int sizeOfParty = party.members.Count;

        //list of IDs to choose from
        List<int> charIdList = getAllCharIdsInParty(partyID);

        if (sizeOfParty <= numberOfCharacters)
        {
            return charIdList;
        }

        List<int> charIdsToReturn= new List<int>();
        for (int i = 0; i < numberOfCharacters; i++)
        {
            int charIndex = Random.Range(0, charIdList.Count);
            int chosenId = charIdList[charIndex];
            charIdList.Remove(chosenId);
            charIdsToReturn.Add(chosenId);
        }
        return charIdsToReturn;
    }

    public List<int> getAllCharIdsInParty(int partyId)
    {
        Party party = findPartyFromID(partyId);

        List<int> charIdList = new List<int>();
        foreach (Character c in party.members)
        {
            charIdList.Add(c.charId);
        }
        return charIdList;
    }

    public void giveEachPartyMemberMoodlet(int partyId, MoodTypes.moodletTag moodletsEnum)
    {
        List<int> characterIds = new List<int>(getAllCharIdsInParty(partyId));
        foreach(int i in characterIds)
        {
            AllCharacterStorage.addMoodletToChar(i, moodletsEnum);
        }
    }

    public void giveEachPartyMemberOpinionletOfEachOther(int partyId, OpinionTypes.opinionletTag opinionletEnum)
    {
        List<int> characterIds = new List<int>(getAllCharIdsInParty(partyId));

        foreach(int charId in characterIds)
        {
            foreach(int targetCharId in characterIds)
            {
                if(charId != targetCharId)
                {
                    AllCharacterStorage.addOpinionletToChar(charId, targetCharId, opinionletEnum);
                }
            }
        }
    }

    //wipes the new party as screen has been closed
    public void closeNewPartyScreen(bool reactivateButtons)
    {
        newParty.Clear();

        GameObject[] rowsToDelete = GameObject.FindGameObjectsWithTag("NewPartyCharacter");
        foreach (GameObject n in rowsToDelete)
        {
            Destroy(n);
        }

        //now the new party screen is closed decide whether any add to party buttons need reactivating
        CharFacade.allSetAddToPartyButton();
    }

    //once new party has been confirmed, generate it
    void confirmNewParty()
    {
        int partyIDtemp;

        if (isEditing == false)
        {
            partyIdCounter++;
            partyIDtemp = partyIdCounter;
        }
        else
        {
            partyIDtemp = editingPartyID;
        }

        foreach (Character n in newParty)
        {
            n.characterDetails.inParty = partyIDtemp;
        }

        string partyName = NameHandler.createRandomPartyName(partyIdCounter);

        BuildParty(partyIDtemp, partyName, newParty, 0);
        AllCharacterStorage.updateAllCharMoodFromPartyMembers();
    }

    //creates a new party for when creating a new party or editing a existing one
    public void BuildParty(int ID, string partyName, List<Character> members, int isOnQuest)
    {
        Party newPartyToGen = gameObject.AddComponent<Party>();

        newPartyToGen.partyID = ID;
        newPartyToGen.members = new List<Character>(members);
        newPartyToGen.onQuest = isOnQuest;
        newPartyToGen.partyName = partyName;

        //if editing, delete the old version
        if (isEditing == true)
        {
            foreach (Party n in allPartiesList)
            {
                if (n.partyID == newPartyToGen.partyID)
                {
                    Destroy(n);
                    allPartiesList.Remove(n);
                    break;
                }
            }

            //delete the old row for the party
            PartyFacade.deletePartyRowByID(newPartyToGen.partyID);

            //if party is empty, return and leave it deleted
            if (newPartyToGen.members.Count == 0)
            {
                Debug.Log("no members");
                isEditing = false;
                return;
            }

            isEditing = false;
        }

        allPartiesList.Add(newPartyToGen);

        PartyFacade.displayParty(newPartyToGen);

        Debug.Log("Party created ID: " + newPartyToGen.partyID + ", members: " + HelperFunctions.partyMembersListToString(newPartyToGen));
    }

    // Clears all storage and resets data
    public void ClearAllData()
    {
        allPartiesList.Clear();
        PartyFacade.ClearAllData();
        isEditing = false;
    }

    //Creates a blank template for a party that is being loaded. Characters and name is added later
    public void loadParty(int name, int onQuest)
    {
        List<Character> emptyList = new List<Character>();
        string emptyName = "placeholder";

        BuildParty(name, emptyName, emptyList, onQuest);
    }

    //resets ID counter to 1 above the highest ID recorded in characterlist and partylist
    public void setCounters()
    {
        partyIdCounter = 0;
        if (allPartiesList.Count != 0)
        {
            partyIdCounter = allPartiesList[0].partyID;
            foreach (Party p in allPartiesList)
            {
                if (p.partyID > partyIdCounter)
                {
                    partyIdCounter = p.partyID;
                }
            }
            partyIdCounter++;
        }
    }

    //adds character to the new party character list
    public void addCharacterToNewPartyList(int charID)
    {
        newParty.Add(AllCharacterStorage.findAliveCharacterFromID(charID));
    }

    // adds a character to a existing party and updates the visuals
    public void addCharToExistingParty(Character character)
    {
        findPartyFromID(character.characterDetails.inParty).members.Add(character);
        PartyFacade.updateAllPartyVisuals();
    }

}
