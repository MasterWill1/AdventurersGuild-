using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractFacade : MonoBehaviour
{
    public Button NewContractButton, closeChoosePartyForConButton;
    public GameObject PartyHandler,  ContractScrollContent, ContractScrollItemPrefab, AssignpartyToContractCanvas, AssignPartyToContractContent;

    PartyFunctions PartyFunctions;
    PartyStorage PartyStorage;
    ContractStorage ContractStorage;

    [HideInInspector]
    public List<GameObject> partiesForSelection, contractRows, completedContractRows;

    [Header("Contract Result Prediction Screen")]
    public GameObject resultPredictionScreen;
    contractResultPredictionScreen ResultPredictionScreenAccess;

    [Header("Contract Result Screen")]
    public GameObject resultScreen;
    contractResultScreen ResultScreenAccess;

    [Header("Contract Details Screen")]
    public GameObject contractDetailsScreen;

    [Header("Choose Party for Contract Menu")]
    public GameObject choosePartyforContractMenu;
    public GameObject PartyScrollItemPrefab;

    [Header("Contract Screens")]
    public Button availableContractsButton;
    public Button completedContractsButton;
    public GameObject availableContractsMenu;
    public GameObject completedContractsMenu;

    [Header("Completed Contract Screen")]
    public GameObject CompletedContractScrollItemContainer;

    // Start is called before the first frame update
    void Awake()
    {
        Button closeChoosePartyForConMenu = closeChoosePartyForConButton.GetComponent<Button>();
        closeChoosePartyForConMenu.onClick.AddListener(delegate { clearAndCloseChoosePartyForConScreen(true); });

        Button availableContractsBtn = availableContractsButton.GetComponent<Button>();
        availableContractsBtn.onClick.AddListener(switchToAvailableContractsMenu);

        Button completedContractsBtn = completedContractsButton.GetComponent<Button>();
        completedContractsBtn.onClick.AddListener(switchToCompletedContractsMenu);

        PartyFunctions = PartyHandler.GetComponent<PartyFunctions>();
        PartyStorage = PartyHandler.GetComponent<PartyStorage>();
        ResultScreenAccess = resultScreen.GetComponent<contractResultScreen>();
        ResultPredictionScreenAccess = resultPredictionScreen.GetComponent<contractResultPredictionScreen>();
        ContractStorage = gameObject.GetComponent<ContractStorage>();
    }

    //displays a contract in either the completed contracts area or
    public void displayContract(Contract contract)
    {
        //generate object
        GameObject scrollItemObj = Instantiate(ContractScrollItemPrefab);

        if (contract.timeLeft > 0)
        {
            //set objects parent
            scrollItemObj.transform.SetParent(ContractScrollContent.transform, false);
            contractRows.Add(scrollItemObj);
        }
        else
        {
            //set objects parent
            scrollItemObj.transform.SetParent(CompletedContractScrollItemContainer.transform, false);
            completedContractRows.Add(scrollItemObj);
        }

        //assign contract to object
        aContract objAccess = scrollItemObj.GetComponent<aContract>();
        objAccess.thisContractID = contract.ID;

        //set visuals
        objAccess.updateVisuals();
    }

    //Finds the gameobject with the contract that matches the ID passed
    public GameObject findContractGameObjectFromId(int ID)
    {
        foreach (GameObject g in contractRows)
        {
            if (g.GetComponent<aContract>().thisContractID == ID)
            {
                return g;
            }
        }
        Debug.LogError("No Contract Row found with ID: " + ID);
        return null;
    }

    //makes the assign party to contract canvas visible
    public void displayAssignPartyToContractCanvas()
    {
        //display each availible party that isnt on a quest already
        foreach (Party party in PartyStorage.allPartiesList)
        {
            if (party.onQuest == 0)
            {
                //generate object and set parent
                GameObject scrollItemObj = Instantiate(PartyScrollItemPrefab);
                scrollItemObj.transform.SetParent(AssignPartyToContractContent.transform, false);

                //assign party to object
                aParty objAccess = scrollItemObj.GetComponent<aParty>();
                objAccess.thisPartyID = party.partyID;

                //generate visuals for new object
                objAccess.updateVisuals();

                objAccess.forChoosePartyScreen();
                partiesForSelection.Add
                    (scrollItemObj);

            }
        }
        AssignpartyToContractCanvas.SetActive(true);
    }

    //clears and closes the choose party for contract screen
    public void clearAndCloseChoosePartyForConScreen(bool doClose)
    {
        if (partiesForSelection.Count > 0)
        {
            foreach (GameObject n in partiesForSelection)
            {
                Destroy(n);
            }

            partiesForSelection.Clear();
            if (doClose)
            {
                AssignpartyToContractCanvas.SetActive(false);
            }
        }
    }

    //displays the contract result screen
    public void contractResultPrediction(int partyID)
    {
        Party party = PartyStorage.findPartyFromID(partyID);
        //int partyStrength = PartyFunctions.CalculatePartyStrength(1, 1);
        int partyStrength = PartyFunctions.CalculatePartyStrength(partyID, ContractStorage.contractToBeDelegated.ID);

        float successChance = ContractFunctions.getSuccessChance(ContractFunctions.getPartyEffectivenessForQuest(partyStrength, ContractStorage.contractToBeDelegated.difficulty));

        resultPredictionScreen.SetActive(true);

        ResultPredictionScreenAccess.setVisuals(party.partyID, ContractStorage.contractToBeDelegated.ID, successChance, party.members, party.partyName);
    }

    //displays the contract result screen
    public void displayContractResultsScreen(bool didPartySucceed, int XpToAward, int reward, List<Character> members)
    {
        ResultScreenAccess.setVisuals(didPartySucceed, XpToAward, reward, members);
        resultScreen.SetActive(true);
    }

    //updates all contracts visuals
    public void updateAllContractVisuals()
    {
        foreach (GameObject g in contractRows)
        {
            g.GetComponent<aContract>().updateVisuals();
        }
    }

    //makes the available contracts menu the current contract menu
    void switchToAvailableContractsMenu()
    {
        availableContractsMenu.SetActive(true);
        completedContractsMenu.SetActive(false);
        availableContractsButton.interactable = false;
        completedContractsButton.interactable = true;
    }

    //makes the completed contracts menu the current contract menu
    void switchToCompletedContractsMenu()
    {
        availableContractsMenu.SetActive(false);
        completedContractsMenu.SetActive(true);
        availableContractsButton.interactable = true;
        completedContractsButton.interactable = false;
    }

    public void ClearContractData()
    {
        contractRows.Clear();
        partiesForSelection.Clear();

        //TO DO update this
        GameObject[] ContractRowsToDelete = GameObject.FindGameObjectsWithTag("aContract");
        foreach (GameObject n in ContractRowsToDelete)
        {
            Destroy(n);
        }
    }

    //updates the time left for each contract when the time is ticked down
    public void updateContractsTimeLeft()
    {
        if (contractDetailsScreen.activeInHierarchy == true)
        {
            contractDetailsScreen.GetComponentInParent<contractDetailsScreen>().updateTimeLeft();
        }
    }
}
