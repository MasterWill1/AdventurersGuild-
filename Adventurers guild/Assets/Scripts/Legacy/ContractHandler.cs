using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
public class ContractHandler : MonoBehaviour
{
    int contractID = 0;
    public Button NewContractButton, closeChoosePartyForConButton;
    public GameObject ContractScrollContent, ContractScrollItemPrefab, PartyScrollItemPrefab, AssignpartyToContractCanvas, AssignPartyToContractContent, traitHandlerObject;

    GameObject genCaller;
    [HideInInspector]
    public List<GameObject> partiesForSelection, contractRows, completedContractRows;

    [HideInInspector]
    public List<Contract> contracts, completedContracts;
    [HideInInspector]
    public Contract contractToBeDelegated;

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

    [Header("Keyworld data")]
    public GameObject gameInfoObject;
    keyWorldDetailsHandler keyWorldDetailsAccessor;

    [Header("Contract Screens")]
    public Button availableContractsButton;
    public Button completedContractsButton;
    public GameObject availableContractsMenu;
    public GameObject completedContractsMenu;

    [Header("Completed Contract Screen")]
    public GameObject CompletedContractScrollItemContainer;

    TraitHandler TraitHandlerAccessor;


    // Start is called before the first frame update
    void Start()
    {
        Button NewContractBtn = NewContractButton.GetComponent<Button>();
        NewContractBtn.onClick.AddListener(generateContract);

        Button closeChoosePartyForConMenu = closeChoosePartyForConButton.GetComponent<Button>();
        closeChoosePartyForConMenu.onClick.AddListener(closeChoosePartyForConScreen);

        Button availableContractsBtn = availableContractsButton.GetComponent<Button>();
        availableContractsBtn.onClick.AddListener(switchToAvailableContractsMenu);

        Button completedContractsBtn = completedContractsButton.GetComponent<Button>();
        completedContractsBtn.onClick.AddListener(switchToCompletedContractsMenu);

        genCaller = GameObject.FindWithTag("GenHandler");
        TraitHandlerAccessor = traitHandlerObject.GetComponent<TraitHandler>();

        ResultScreenAccess = resultScreen.GetComponent<contractResultScreen>();

        keyWorldDetailsAccessor = gameInfoObject.GetComponent<keyWorldDetailsHandler>();

        ResultPredictionScreenAccess = resultPredictionScreen.GetComponent<contractResultPredictionScreen>();

        switchToAvailableContractsMenu();
    }

    //TO BE STATIC'D
    void generateContract()
    {
        contractID++;
        int difficulty = Random.Range(5, 50);

        Contract generatedContract = gameObject.AddComponent<Contract>();
        generatedContract.ID = contractID;
        generatedContract.difficulty = difficulty;
        generatedContract.reward = difficulty + Random.Range(-(int)(difficulty * 0.2), (int)(difficulty * 0.2));
        generatedContract.timeLeft = difficulty + Random.Range(-(int)(difficulty*0.3), -(int)(difficulty * 0.7));
        generatedContract.isOngoing = false;

        BuildContract(generatedContract);
        TraitHandlerAccessor.genRandomContractTraits(contractID);
    }

    //CONTRACT STORAGE
    public void BuildContract(Contract contract)
    {
        if(contract.timeLeft == 0)
        {
            completedContracts.Add(contract);
        }
        else if(contract.timeLeft > 0)
        {
            contracts.Add(contract);
        }
        else
        {
            Debug.LogError("contract to build has time left of less than 0. Time Left for contract ID: " + contract.ID + " is " + contract.timeLeft + " days");
        }
        displayContract(contract);
    }

    //TO CONTRACT FACADE
    void displayContract(Contract contract)
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

    //CONTRACT STORAGE
    public Contract findContractFromID(int ID)
    {
        foreach (Contract c in contracts)
        {
            if (c.ID == ID)
            {
                return c;
            }
        }
        foreach (Contract cc in completedContracts)
        {
            if (cc.ID == ID)
            {
                return cc;
            }
        }
        Debug.Log("No Contract found");
        return null;
    }

    //CONTRACT facade
    public GameObject findContractGameObjectFromId(int ID)
    {
        foreach(GameObject g in contractRows)
        {
            if (g.GetComponent<aContract>().thisContractID == ID)
            {
                return g;
            }
        }
        Debug.LogError("No Contract Row found with ID: " + ID);
        return null;
    }

    //TO CONTRACT FACADE
    public void displayAssignPartyToContractCanvas()
    {
        //display each availible party
        foreach (Party party in genCaller.GetComponent<Generator>().parties)
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

    //CONTRACT FACADE
    public void closeChoosePartyForConScreen()
    {
        if (partiesForSelection.Count > 0)
        {
            foreach (GameObject n in partiesForSelection)
            {
                Destroy(n);
            }

            partiesForSelection.Clear();
        }
    }

    //contract facade
    public void contractResultPrediction(Party party)
    {
        int partyStrength = CalculatePartyStrength(party, contractToBeDelegated.ID);
        float successChance = getSuccesChance(partyStrength, contractToBeDelegated.difficulty);

        resultPredictionScreen.SetActive(true);

        ResultPredictionScreenAccess.setVisuals(party.partyID, contractToBeDelegated.ID, successChance, party.members, party.partyName);
    }

    //storage
    //gets called when a contract is completed
    public void executeContract(int partyID, int contractID)
    {
        keyWorldDetailsAccessor.pauseTime();

        Contract completedContract = findContractFromID(contractID);

        Debug.Log("This Contracts Difficulty: " + completedContract.difficulty);

        Party thisParty = genCaller.GetComponent<Generator>().findPartyFromID(partyID);

        int partyStrength = CalculatePartyStrength(thisParty, contractID);

        float successChance = getSuccesChance(partyStrength, completedContract.difficulty);

        bool didPartySucceed = CalculateSuccess(successChance);
        Debug.Log("Did party succeed: " + didPartySucceed);

        //determine how much xp to award party
        int XpToAward = completedContract.difficulty;
        if (didPartySucceed == false)
        {
            XpToAward = (int)(XpToAward * 0.5);
        }
        //give the members xp and update their xp
        foreach(Character c in thisParty.members)
        {
            c.XP = c.XP + XpToAward;
            foreach(GameObject g in genCaller.GetComponent<Generator>().employeeCharactersRows)
            {
                if (g.GetComponent<aCharacter>().thisCharacterID == c.ID)
                {
                    g.GetComponent<aCharacter>().updateVisuals();
                    break;
                }
            }
        }
        
        ResultScreenAccess.setVisuals(didPartySucceed, XpToAward, completedContract.reward, thisParty.members);
        resultScreen.SetActive(true);

        keyWorldDetailsAccessor.changeTotalGold(completedContract.reward);
        keyWorldDetailsAccessor.changeRenown(completedContract.difficulty);
        keyWorldDetailsAccessor.changeReputation(HelperFunctions.goodnessScoreToModifier
            (TraitHandlerAccessor.getContractTraitsReferenceFromID(contractID).goodness));

        Destroy(findContractGameObjectFromId(completedContract.ID));
        displayContract(completedContract);
    }

    //static
    float getSuccesChance(int partyStrength, int contractDifficulty)
    {
        float fPartyStrength = partyStrength;
        float fContractDifficulty = contractDifficulty;
        float preDeductionSucessChance = fPartyStrength / fContractDifficulty;
        Debug.Log("preDeductionSucessChance: " + preDeductionSucessChance);

        float successChance = preDeductionSucessChance * (float)0.9;
        Debug.Log("Success Chance: " + successChance);

        return successChance;
    }

    //storage
    public void setContractAsOngoing(int partyID)
    {
        genCaller.GetComponent<Generator>().findPartyFromID(partyID).onQuest = contractToBeDelegated.ID;
        findContractFromID(contractToBeDelegated.ID).isOngoing = true;

        //update all contracts so ongoing ones are no longer able to be assigned
        updateAllContractVisuals();
    }

    //facade
    void updateAllContractVisuals()
    {
        foreach(GameObject g in contractRows)
        {
            g.GetComponent<aContract>().updateVisuals();
        }
    }

    void switchToAvailableContractsMenu()
    {
        availableContractsMenu.SetActive(true);
        completedContractsMenu.SetActive(false);
        availableContractsButton.interactable = false;
        completedContractsButton.interactable = true;
    }

    void switchToCompletedContractsMenu()
    {
        availableContractsMenu.SetActive(false);
        completedContractsMenu.SetActive(true);
        availableContractsButton.interactable = true;
        completedContractsButton.interactable = false;
    }

    //party static
    //callen from execute party to calculate party strength
    int CalculatePartyStrength(Party party, int conID)
    {
        int partyStrength = 0;

        foreach(Character character in party.members)
        {
            int thisCharsStrength = HelperFunctions.calculateCharacterStrength(character, TraitHandlerAccessor.getTraitsReferenceFromID(character.ID), 
                TraitHandlerAccessor.getAttributesFromID(character.ID), TraitHandlerAccessor.getContractTraitsReferenceFromID(conID));

            partyStrength = partyStrength + thisCharsStrength;
        }

        Debug.Log("This Parties strength before party modifiers: " + partyStrength);
        partyStrength = HelperFunctions.partyStrengthAfterModifiers(party, partyStrength);
        Debug.Log("This Parties final strength: " + partyStrength);
        return partyStrength;
    }

    //static
    bool CalculateSuccess(float successChance)
    {
        double score = Random.Range(0f, 1);
        Debug.Log("Contract Difficulty rating: " + score);

        if(score < successChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //storage
    //for each contract that is ongoing tick their days remaining down by 1
    public void tickContractsDown()
    {
        List<Contract> finishedContracts = new List<Contract>();
        foreach (Contract c in contracts)
        {
            if(c.isOngoing == true)
            {
                c.timeLeft--;

                findContractGameObjectFromId(c.ID).GetComponent<aContract>().updateVisuals();

                if(c.timeLeft == 0)
                {
                    executeContract(genCaller.GetComponent<Generator>().findPartyFromContractID(c.ID).partyID, c.ID);
                    finishedContracts.Add(c);
                }
            }
        }
        //moves completed contract to finished contract list
        if(finishedContracts.Count > 0)
        {
            foreach(Contract conToRemove in finishedContracts)
            {
                contracts.Remove(conToRemove);
                completedContracts.Add(conToRemove);
            }
        }
        finishedContracts.Clear();

        if(contractDetailsScreen.activeInHierarchy == true)
        {
            contractDetailsScreen.GetComponentInParent<contractDetailsScreen>().updateTimeLeft();
        }
    }

    //storage and facade
    public void ClearContractData()
    {
        contracts.Clear();
        partiesForSelection.Clear();
        contractRows.Clear();

        GameObject[] ContractRowsToDelete = GameObject.FindGameObjectsWithTag("aContract");
        foreach (GameObject n in ContractRowsToDelete)
        {
            Destroy(n);
        }
    }

    //storage
    public void LoadContract(int ID, int difficulty, int reward, int timeleft, int isOngoing)
    {
        Contract generatedContract = gameObject.AddComponent<Contract>();
        generatedContract.ID = ID;
        generatedContract.difficulty = difficulty;
        generatedContract.reward = reward;
        generatedContract.timeLeft = timeleft;
        generatedContract.isOngoing = HelperFunctions.IntToBool(isOngoing);

        BuildContract(generatedContract);
    }

    //storage
    public void LoadCompletedContract(int ID, int difficulty, int reward, int timeleft, int isOngoing)
    {
        Contract generatedContract = gameObject.AddComponent<Contract>();
        generatedContract.ID = ID;
        generatedContract.difficulty = difficulty;
        generatedContract.reward = reward;
        generatedContract.timeLeft = timeleft;
        generatedContract.isOngoing = HelperFunctions.IntToBool(isOngoing);

        BuildContract(generatedContract);
    }

    //storage
    //resets ID counter to 1 above the highest ID recorded in characterlist and partylist
    public void setCounters()
    {
        contractID = 0;
        if (contracts.Count != 0)
        {
            contractID = contracts[0].ID;
            foreach (Contract c in contracts)
            {
                if (c.ID > contractID)
                {
                    contractID = c.ID;
                }
            }
            contractID++;
        }
    }
}
*/