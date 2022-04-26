using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyFacade : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> partyRow;

    public GameObject PartyHandler;
    PartyStorage PartyStorage;

    [Header("Employed Characters Screen")]
    public GameObject employeeScrollContent;
    public GameObject employeeScrollItemPrefab;

    [Header("Parties screens")]
    public GameObject partyScrollContent;
    public GameObject partyScrollItemPrefab;

    [Header("New Party screens")]
    public GameObject newPartyScrollContent;
    public GameObject newPartyMemberScrollItemPrefab;
    public GameObject NewPartyScreen;

    // Start is called before the first frame update
    void Start()
    {
        PartyStorage = PartyHandler.GetComponent<PartyStorage>();
    }

    //adds character into the create new party screen
    public void addEmployeeToNewParty(int addedCharID)
    {
        //add character to the new party characters list
        PartyStorage.addCharacterToNewPartyList(addedCharID);

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

    //Creates and displays Party in party menu
    public void displayParty(Party party)
    {
        //generate object
        GameObject scrollItemObj = Instantiate(partyScrollItemPrefab);
        partyRow.Add(scrollItemObj);
        scrollItemObj.transform.SetParent(partyScrollContent.transform, false);


        //assign party to object
        aParty objAccess = scrollItemObj.GetComponent<aParty>();
        objAccess.thisPartyID = party.partyID;

        objAccess.updateVisuals();

        //new party is created, wipe create party screen
        PartyStorage.closeNewPartyScreen(false);
    }

    // Clears all storage and resets data
    public void ClearAllData()
    {
        foreach (GameObject n in partyRow)
        {
            Destroy(n);
        }

        partyRow.Clear();
    }

    public void deletePartyRowByID(int partyID)
    {
        GameObject rowToDelete = null;
        foreach (GameObject n in partyRow)
        {
            if (n.GetComponent<aParty>().thisPartyID == partyID)
            {
                rowToDelete = n;
                break;
            }
        }
        Destroy(rowToDelete);
    }

    public void updateAllPartyVisuals()
    {
        foreach (GameObject p in partyRow)
        {
            p.GetComponent<aParty>().updateVisuals();
        }
    }
}
