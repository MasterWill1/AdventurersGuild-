using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class aParty : MonoBehaviour
{
    public GameObject editpartyButtonGO;
    public Button editPartyButton;
    public GameObject choosePartyForContractButtonGO;
    public Button choosePartyForContractButton;

    //GameObject genCaller, conCaller;
    //Generator genScriptCaller;
    //ContractHandler conScriptCaller;

    GameObject PartyHandler, ContractHandler;
    PartyFacade PartyFacade;
    PartyStorage PartyStorage;
    ContractFacade ContractFacade;

    [HideInInspector]
    public int thisPartyID;

    // Start is called before the first frame update
    void Awake()
    {
        Button editPartyBtn = editPartyButton.GetComponent<Button>();
        editPartyBtn.onClick.AddListener(editParty);

        Button choosePartyBtn = choosePartyForContractButton.GetComponent<Button>();
        choosePartyBtn.onClick.AddListener(partyChosenForContract);

        //genCaller = GameObject.FindWithTag("GenHandler");
        //conCaller = GameObject.FindWithTag("ConHandler");

        PartyHandler = GameObject.FindWithTag("PartyHandler");
        ContractHandler = GameObject.FindWithTag("ContractHandler");

        //genScriptCaller = genCaller.GetComponent<Generator>();
        //conScriptCaller = conCaller.GetComponent<ContractHandler>();

        PartyFacade = PartyHandler.GetComponent<PartyFacade>();
        PartyStorage = PartyHandler.GetComponent<PartyStorage>();
        ContractFacade = ContractHandler.GetComponent<ContractFacade>();

    }

    void editParty()
    {
        PartyFacade.NewPartyScreen.SetActive(true);

        Party partyToEdit = PartyStorage.findPartyFromID(thisPartyID); 

        foreach(Character n in partyToEdit.members)
        {
            PartyFacade.addEmployeeToNewParty(n.charId);
        }

        PartyStorage.isEditing = true;
        PartyStorage.editingPartyID = partyToEdit.partyID;
    }

    public void activateChoosePartyButton()
    {
        choosePartyForContractButtonGO.SetActive(true);
    }

    public void forChoosePartyScreen()
    {
        editpartyButtonGO.SetActive(false);
        choosePartyForContractButtonGO.SetActive(true);
    }

    void partyChosenForContract()
    {
        //Party partyChosen = PartyStorage.findPartyFromID(thisPartyID);
        Debug.Log("Party chosen for contract: ID: " + thisPartyID);
        ContractFacade.contractResultPrediction(thisPartyID);        
    }

    public void updateVisuals()
    {
        Party thisParty = PartyStorage.findPartyFromID(thisPartyID);

        //create string of all members in party
        string collatedNames = HelperFunctions.partyMembersListToString(thisParty);

        //generate visuals
        gameObject.transform.Find("partyName").gameObject.GetComponent<Text>().text = thisParty.partyName;
        gameObject.transform.Find("members").gameObject.GetComponent<Text>().text = collatedNames;
        if (thisParty.onQuest != 0)
        {
            gameObject.transform.Find("status").gameObject.GetComponent<Text>().text = "On Quest";
        }
        else
        {
            gameObject.transform.Find("status").gameObject.GetComponent<Text>().text = "Idle";
        }
    }
}
