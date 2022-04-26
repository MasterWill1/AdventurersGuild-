using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NameHandler : MonoBehaviour
{
    string[] humanMaleFirstNameArray = { "Graham", "Ronald", "Lesley", "Mitch", "Charles", "Alfred", "Darrel", "Nicholas", "Daren", "Alfie", "Jack", "Dino", "Errol", "Bhal" };
    string[] humanMaleLastNameArray = { "Buckley", "Eley", "Thomas", "Stokes", "Palmer", "Ball", "Beckett", "Haywood", "Lavin", "Brown", "Emmett", "Spurgeon" };

    string[] partyFirstName = { "Bloody", "Raiding", "Ravaging", "Holy", "Divine", "Lost", "Greedy", "Freakish", "Ordinary", 
        "Wrathful", "Red", "Dark", "Black", "Bronze", "Iron", "Singing", "Menacing", "Firey" };
    string[] partyLastName = { "Dominators", "Avengers", "Punishers", "Explorers", "Swordsmiths", "Songbirds", "Scouts", "Wolves", 
        "Angels", "Ghosts", "Slayers", "Kingsmen", "Immortals", "Plebs", "Acolytes" };

    [HideInInspector]
    public List<NameReference> nameList, partyNameList;

    //public GameObject GeneratorHandlerAccessor;
    //Generator GeneratorAccessor;
    public GameObject CharHandler, PartyHandler;
    CharStorage CharStorage;
    CharFacade CharFacade;
    PartyStorage PartyStorage;
    PartyFacade PartyFacade;

    private void Start()
    {
        CharStorage = CharHandler.GetComponent<CharStorage>();
        CharFacade = CharHandler.GetComponent<CharFacade>();
        PartyStorage = PartyHandler.GetComponent<PartyStorage>();
        PartyFacade = PartyHandler.GetComponent<PartyFacade>();
    }

    public NameReference generateRandomName(CharacterDetails character)
    {
        int firstNameInt = Random.Range(0, humanMaleFirstNameArray.Length);
        int lastNameInt = Random.Range(0, humanMaleLastNameArray.Length);

        NameReference nameReference = gameObject.AddComponent<NameReference>();
        nameReference.ID = character.ID;
        nameReference.FN = firstNameInt;
        nameReference.SN = lastNameInt;

        nameList.Add(nameReference);

        return nameReference;
    }

    public string createNameString(NameReference nameReference)
    {
        string fName = humanMaleFirstNameArray[nameReference.FN];
        string sName = humanMaleLastNameArray[nameReference.SN];
        string fullName = fName + " " + sName;
        return fullName;
    }

    public void LoadHumanName(int ID, int FN, int SN)
    {
        Debug.Log("Loading Name for ID: " + ID + ", FN: " + FN + ", SN: " + SN);
        NameReference nameReference = gameObject.AddComponent<NameReference>();
        nameReference.ID = ID;
        nameReference.FN = FN;
        nameReference.SN = SN;

        nameList.Add(nameReference);

        string fName = humanMaleFirstNameArray[FN];
        string sName = humanMaleLastNameArray[SN];
        string fullName = fName + " " + sName;

        foreach (CharacterDetails c in CharStorage.characterDetailsList)
        {
            if(ID == c.ID)
            {
                c.charName = fullName;

                bool found = false;

                //update visuals 
                foreach(GameObject G in CharFacade.employeeCharactersRows)
                {
                    if(G.GetComponent<aCharacter>().thisCharacterID == ID)
                    {
                        G.GetComponent<aCharacter>().updateVisuals();
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    foreach (GameObject G in CharFacade.recruitableCharactersRows)
                    {
                        if (G.GetComponent<aCharacter>().thisCharacterID == ID)
                        {
                            G.GetComponent<aCharacter>().updateVisuals();
                            break;
                        }
                    }
                }
                break;
            }
        }
    }

    public string createRandomPartyName(int partyID)
    {
        int partyFirstNameInt = Random.Range(0, partyFirstName.Length);
        int partyLastNameInt = Random.Range(0, partyLastName.Length);

        string fName = partyFirstName[partyFirstNameInt];
        string sName = partyLastName[partyLastNameInt];

        string fullName = "The " + fName + " " + sName;

        NameReference nameReference = gameObject.AddComponent<NameReference>();
        nameReference.ID = partyID;
        nameReference.FN = partyFirstNameInt;
        nameReference.SN = partyLastNameInt;

        partyNameList.Add(nameReference);

        return fullName;
    }

    public void LoadPartyName(int ID, int FN, int SN)
    {
        NameReference nameReference = gameObject.AddComponent<NameReference>();
        nameReference.ID = ID;
        nameReference.FN = FN;
        nameReference.SN = SN;

        partyNameList.Add(nameReference);

        string fName = partyFirstName[FN];
        string sName = partyLastName[SN];
        string fullName = "The " + fName + " " + sName;

        foreach (Party p in PartyStorage.allPartiesList)
        {
            if (ID == p.partyID)
            {
                p.partyName = fullName;

                foreach (GameObject G in PartyFacade.partyRow)
                {
                    if (G.GetComponent<aParty>().thisPartyID == ID)
                    {
                        G.GetComponent<aParty>().updateVisuals();
                        break;
                    }
                }
                break;
            }
        }
    }
}
