using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
public class Generator : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> recruitableCharactersRows, employeeCharactersRows, partyRow;

    public GameObject NameHandlerObject;
    NameHandler NameHandlerAccessor;

    public GameObject TraitHandlerObject;
    TraitHandler TraitHandlerAccessor;

    public GameObject KWDGameObject;
    keyWorldDetailsHandler KWDaccessor;

    //public ScrollRect scrollView;
    [Header("Recruitable Characters Screen")]
    public GameObject rcScrollContent;
    public GameObject rcScrollItemPrefab;
    [HideInInspector]
    public List<Character> characterList,  newParty;
    [HideInInspector]
    public List<Party> parties;
    //[HideInInspector]
    //public List<GameObject> newPartyVisual;
    [HideInInspector]
    public bool isEditing = false;
    [HideInInspector]
    public int editingPartyID;
    int charID = 0;
    int partyID = 0;

    public Button generateRecruitsButton;
    
    //public RecruitableCharacter rcCaller;
    [Header("Employed Characters Screen")]
    public GameObject employeeScrollContent;
    public GameObject employeeScrollItemPrefab;

    [Header("Parties screens")]
    public GameObject partyScrollContent;
    public GameObject partyScrollItemPrefab;

    [Header("New Party screens")]
    public GameObject newPartyScrollContent;
    public GameObject newPartyMemberScrollItemPrefab;
    public Button closePartyScreen;
    public GameObject NewPartyScreen;
    public Button confirmNewPartyButton;


    private void Start()
    {
        Button genRecrtsBtn = generateRecruitsButton.GetComponent<Button>();
        genRecrtsBtn.onClick.AddListener(generateCharacters);

        Button closeNewPartyBtn = closePartyScreen.GetComponent<Button>();
        closeNewPartyBtn.onClick.AddListener(delegate { closeNewPartyScreen(true); });

        Button confirmNewPartBtn = confirmNewPartyButton.GetComponent<Button>();
        confirmNewPartBtn.onClick.AddListener(confirmNewParty);

        NameHandlerAccessor = NameHandlerObject.GetComponent<NameHandler>();
        TraitHandlerAccessor = TraitHandlerObject.GetComponent<TraitHandler>();
        KWDaccessor = KWDGameObject.GetComponent<keyWorldDetailsHandler>();

    }

    //TO BE STATIC'D
    void generateCharacters()
    {
        charID++;
        int xp = Random.Range(0, 475);
        int charClass = Random.Range(1, 6);
        int race = Random.Range(1, 6);
        int level = HelperFunctions.getCharactersLevel(xp);
        int cost = level * 10 + Random.Range(-level, level+1);

        Character generatedChar = gameObject.AddComponent<Character>();
        generatedChar.ID = charID;
        generatedChar.XP = xp;
        generatedChar.charClass = charClass;
        generatedChar.race = race;
        generatedChar.inParty = 0;
        generatedChar.isRecruited = false;
        generatedChar.charName = NameHandlerAccessor.createRandomHumanName(generatedChar);
        generatedChar.cost = cost;

        //set traits. these are stored elsewhere 
        TraitHandlerAccessor.getRandomAttributes(charID);
        TraitHandlerAccessor.getRandomTraits(charID);

        buildCharacter(generatedChar);
    }

    //CHARACTER STORAGE
    public void buildCharacter(Character character)
    {
        characterList.Add(character);

        if(character.isRecruited == true)
        {
            EmployeeRecruited(character.ID);
        }
        else
        {
            displayRecuitable(character);
        }

        if(character.inParty != 0)
        {
            foreach(GameObject p in partyRow)
            {
                if (p.GetComponent<aParty>().thisPartyID == character.inParty)
                {
                    findPartyFromID(character.inParty).members.Add(character);
                    p.GetComponent<aParty>().updateVisuals();
                    break;
                }
            }
        }
    }

    //PARTY STORAGE
    public Party findPartyFromID(int ID)
    {
        foreach (Party p in parties)
        {
            if (p.partyID == ID)
            {
                return p;
            }
        }
        Debug.Log("No party found with ID: "+ ID);
        return null;
    }

    //PARTY STORAGE
    public Party findPartyFromContractID(int ID)
    {
        foreach (Party p in parties)
        {
            if (p.onQuest == ID)
            {
                return p;
            }
        }
        Debug.Log("No party found with on contract ID: " + ID);
        return null;
    }

    //CHARACTER STORAGE
    public Character findCharacterFromID(int ID)
    {
        foreach (Character c in characterList)
        {
            if (c.ID == ID)
            {
                return c;
            }
        }
        Debug.LogError("No Character found with ID: " + ID);
        return null;
    }

    //CHARACTER VISUALS
    void displayRecuitable(Character gendChar)
    {
        //generate object
        GameObject scrollItemObj = Instantiate(rcScrollItemPrefab);

        //generate visuals
        scrollItemObj.transform.SetParent(rcScrollContent.transform, false);

        //assign character to object
        aCharacter objAccess = scrollItemObj.GetComponent<aCharacter>();
        objAccess.thisCharacterID = gendChar.ID;

        RecruitableCharacter rcObjAccess = scrollItemObj.GetComponent<RecruitableCharacter>();
        rcObjAccess.setCharCostVisual();

        //set visuals
        objAccess.updateVisuals();
        recruitableCharactersRows.Add(scrollItemObj);
    }

    //CHARACTER VISUALS
    //called when recuit character button is pressed
    public void EmployeeRecruited(int newEmployeeID)
    {

        foreach (Character c in characterList)
        {
            if (c.ID == newEmployeeID)
            {
                c.isRecruited = true;
                break;
            }
        }

        //generate object
        GameObject scrollItemObj = Instantiate(employeeScrollItemPrefab);
        employeeCharactersRows.Add(scrollItemObj);

        //set parent
        scrollItemObj.transform.SetParent(employeeScrollContent.transform, false);

        //assign character to object
        aCharacter objAccess = scrollItemObj.GetComponent<aCharacter>();
        objAccess.thisCharacterID = newEmployeeID;

        //set visuals
        objAccess.updateVisuals();
    }

    //PARTY VISUALS
    public void addEmployeeToNewParty(int addedCharID)
    {
        foreach (Character c in characterList)
        {
            if (c.ID == addedCharID)
            {
                newParty.Add(c);
                break;
            }
        }
        //generate object
        GameObject scrollItemObj = Instantiate(newPartyMemberScrollItemPrefab);

        //set parent
        scrollItemObj.transform.SetParent(newPartyScrollContent.transform, false);

        //assign character to object
        aCharacter objAccess = scrollItemObj.GetComponent<aCharacter>();
        objAccess.thisCharacterID = addedCharID;

        //set visuals
        objAccess.updateVisuals();
    }

    //PARTY STORAGE
    //removes character from new party, called from NewPartyChar
    public void removeCharFromNewParty(int characterID)
    {
        foreach (Character c in characterList)
        {
            if (c.ID == characterID)
            {
                newParty.Remove(c);
                break;
            }
        }
    }

    //PARTY STORAGE
    //wipes the new party as screen has been closed
    void closeNewPartyScreen(bool reactivateButtons)
    {
        newParty.Clear();

        GameObject[] rowsToDelete = GameObject.FindGameObjectsWithTag("NewPartyCharacter");
        foreach (GameObject n in rowsToDelete)
        {
            Destroy(n);
        }

        if (reactivateButtons)
        {
            foreach (GameObject n in employeeCharactersRows)
            {
                n.GetComponent<EmployeeCharacter>().decideOnReActivateAddToPartyButton();
            }
        }
    }

    //PARTY STORAGE
    //once new party has been confirmed, generate it
    void confirmNewParty()
    {
        int partyIDtemp;

        if (isEditing == false)
        {
            partyID++;
            partyIDtemp = partyID;
        }
        else
        {
            partyIDtemp = editingPartyID;
        }

        foreach (Character n in newParty)
        {
            n.inParty = partyID;
        }

        string partyName = NameHandlerAccessor.createRandomPartyName(partyID);

        BuildParty(partyIDtemp, partyName, newParty, 0);
    }

    //PARTY STATIC
    public void BuildParty(int ID, string partyName, List<Character> members, int isOnQuest)
    {
        Party newPartyToGen = gameObject.AddComponent<Party>();

        newPartyToGen.partyID = ID;
        newPartyToGen.members = new List<Character>(members);
        newPartyToGen.onQuest = isOnQuest;
        newPartyToGen.partyName = partyName;

        displayParty(newPartyToGen);
    }

    //PARTY VISUALS
    void displayParty(Party party)
    {
        //if editing, delete the old version
        if (isEditing == true)
        {
            foreach (Party n in parties)
            {
                if (n.partyID == party.partyID)
                {
                    Destroy(n);
                    parties.Remove(n);
                    break;
                }
            }
            //GameObject[] partyFinder = GameObject.FindGameObjectsWithTag("aParty");
            foreach (GameObject n in partyRow)
            {
                if (n.GetComponent<aParty>().thisPartyID == party.partyID)
                {
                    Destroy(n);
                    break;
                }
            }

            if (party.members.Count == 0)
            {
                Debug.Log("no members");
                return;
            }

            isEditing = false;
        }

        parties.Add(party);

        //generate object
        GameObject scrollItemObj = Instantiate(partyScrollItemPrefab);
        partyRow.Add(scrollItemObj);
        scrollItemObj.transform.SetParent(partyScrollContent.transform, false);


        //assign party to object
        aParty objAccess = scrollItemObj.GetComponent<aParty>();
        objAccess.thisPartyID = party.partyID;

        objAccess.updateVisuals();

        //new party is created, wipe create party screen
        closeNewPartyScreen(false);
    }

    //move to KWD
    public void payWages()
    {
        foreach(Character c in characterList)
        {
            if(c.isRecruited == true)
            {
                int thisCharWage = HelperFunctions.wageFromWorth(c.cost);
                KWDaccessor.changeTotalGold(-thisCharWage);
                Debug.Log("Payed " + c.charName + " " + thisCharWage + "gp");
            }
        }
    }

    // CHARACTER/PARTY STORAGE
    public void ClearAllData()
    {
        foreach (GameObject n in recruitableCharactersRows)
        {
            Destroy(n);
        }

        foreach (GameObject n in employeeCharactersRows)
        {
            Destroy(n);
        }
        
        foreach (GameObject n in partyRow)
        {
            Destroy(n);
        }

        characterList.Clear();
        parties.Clear();
        partyRow.Clear();
        recruitableCharactersRows.Clear();
        employeeCharactersRows.Clear();
        isEditing = false;

        //closeNewPartyScreen(false);
    }

    //CHARACTER STORAGE
    public void loadCharacter(int ID, int XP, int cClass, int race, int inParty, int isRecruited)
    {
        Character newCharacter = gameObject.AddComponent<Character>();
        newCharacter.ID = ID;
        newCharacter.XP = XP;
        newCharacter.charClass = cClass;
        newCharacter.race = race;
        newCharacter.inParty = inParty;
        if(isRecruited == 0)
        {
            newCharacter.isRecruited = false;
        }
        else if(isRecruited == 1)
        {
            newCharacter.isRecruited = true;
        }
        else
        {
            Debug.LogError("Incorrect value entered for isRecruited: " + isRecruited);
        }

        buildCharacter(newCharacter);
    }

    //PARTY STORAGE
    public void loadParty(int name, int onQuest)
    {
        List<Character> emptyList = new List<Character>();
        string emptyName = "placeholder";

        BuildParty(name, emptyName, emptyList, onQuest);
    }

    // CHARACTER/PARTY STORAGE
    //resets ID counter to 1 above the highest ID recorded in characterlist and partylist
    public void setCounters()
    {
        charID = 0;
        if (characterList.Count != 0)
        {
            charID = characterList[0].ID;
            foreach (Character c in characterList)
            {
                if (c.ID > charID)
                {
                    charID = c.ID;
                }
            }
            charID++;
        }

        if (parties.Count != 0)
        {
            partyID = parties[0].partyID;
            foreach (Party p in parties)
            {
                if (p.partyID > partyID)
                {
                    partyID = p.partyID;
                }
            }
            partyID++;
        }
    }
}
*/